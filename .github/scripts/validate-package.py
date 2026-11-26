import json
from pathlib import Path
import re
import xml.etree.ElementTree as ET


root = Path(__file__).resolve().parents[2]
manifest = json.loads((root / "package.json").read_text(encoding="utf-8"))
version = manifest["version"]
if not isinstance(version, str) or not re.fullmatch(r"[0-9]+\.[0-9]+\.[0-9]+", version):
    raise SystemExit("package.json must declare a numeric major.minor.patch version")

runtime_versions = re.findall(
    r'\bSdkVersion\s*\{\s*get;\s*\}\s*=\s*"([^"]+)"\s*;',
    (root / "Runtime/BaseTenjin.cs").read_text(encoding="utf-8"),
)
if runtime_versions != [version]:
    raise SystemExit(
        f"Runtime/BaseTenjin.cs SdkVersion {runtime_versions!r} does not match {version}"
    )

release_heading = re.search(
    r"^(?:##[ \t]+[^\r\n]+|v[0-9][^\r\n]*)$",
    (root / "RELEASE_NOTES.md").read_text(encoding="utf-8"),
    re.MULTILINE,
)
release = re.match(
    r"(?:##[ \t]+\[([0-9]+\.[0-9]+\.[0-9]+)\](?=\(|\s|$)"
    r"|##[ \t]+([0-9]+\.[0-9]+\.[0-9]+)(?=\s|$)"
    r"|v([0-9]+\.[0-9]+\.[0-9]+)$)",
    release_heading.group() if release_heading else "",
)
release_version = next((value for value in release.groups() if value), None) if release else None
if release_version != version:
    raise SystemExit(f"The latest release heading does not match package.json {version}")

assemblies = sorted(root.rglob("*.asmdef"))
for path in assemblies:
    assembly = json.loads(path.read_text(encoding="utf-8"))
    if not isinstance(assembly, dict) or not isinstance(assembly.get("name"), str) or not assembly["name"]:
        raise SystemExit(f"{path.relative_to(root)} must declare an assembly name")
    print(f"Parsed assembly definition: {path.relative_to(root)}")

ET.parse(root / "Editor/Dependencies.xml")
print("Parsed dependency XML: Editor/Dependencies.xml")
print(f"Package, runtime and release versions agree: {version}")
