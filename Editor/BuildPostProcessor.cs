//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif

public class BuildPostProcessor : MonoBehaviour
{
    private static readonly string UPM_PACKAGE_PATH = "Packages/com.tenjin.sdk";

    private static string FindXCFrameworkZip()
    {
        // Try UPM package path first
        string upmPath = UPM_PACKAGE_PATH + "/Plugins/iOS/TenjinSDK.xcframework.zip";
        if (File.Exists(upmPath))
            return upmPath;

        // Fall back to Assets path (.unitypackage install)
        string assetsPath = "Assets/Plugins/iOS/TenjinSDK.xcframework.zip";
        if (File.Exists(assetsPath))
            return assetsPath;

        Debug.LogError("TenjinSDK: Could not find TenjinSDK.xcframework.zip");
        return null;
    }

    private static List<string> GetTenjinAssets()
    {
        List<string> tenjinAssets = new List<string>();

        // Editor files
        tenjinAssets.Add("Assets/Editor/BuildPostProcessor.cs");
        tenjinAssets.Add("Assets/Editor/Dependencies.xml");

        // Editor scripts in Tenjin/Scripts/Editor (auto-discover .cs files)
        foreach (string file in Directory.GetFiles("Assets/Tenjin/Scripts/Editor", "*.cs"))
        {
            tenjinAssets.Add(file.Replace("\\", "/"));
        }

        // Gradle Templates
        tenjinAssets.Add("Assets/Plugins/Android/GradleTemplates/m2repository.gradle");

        // iOS plugins (auto-discover Tenjin-related files, excluding .meta)
        foreach (string file in Directory.GetFiles("Assets/Plugins/iOS"))
        {
            if (file.EndsWith(".meta")) continue;
            string normalized = file.Replace("\\", "/");
            if (normalized.Contains("Tenjin"))
                tenjinAssets.Add(normalized);
        }

        // Runtime scripts in Tenjin/Scripts (auto-discover .cs files, exclude Editor subfolder)
        foreach (string file in Directory.GetFiles("Assets/Tenjin/Scripts", "*.cs"))
        {
            tenjinAssets.Add(file.Replace("\\", "/"));
        }

        return tenjinAssets;
    }

    [MenuItem("Assets/Tenjin/Export Unity Package")]
    public static void ExportTenjinUnityPackage()
    {
        string exportedFileName = "TenjinUnityPackage.unitypackage";
        List<string> tenjinAssets = GetTenjinAssets();
        tenjinAssets.Add("Assets/Tenjin/tenjin.unitypackage.manifest");

        AssetDatabase.ExportPackage(
            tenjinAssets.ToArray(),
            exportedFileName,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }

    [MenuItem("Assets/Tenjin/Export UPM Package")]
    public static void ExportTenjinUpmPackage()
    {
        string upmOutputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../upm-package"));
        List<string> assets = GetTenjinAssets();
        int count = 0;

        foreach (string srcPath in assets)
        {
            string dstRelative;

            if (srcPath.StartsWith("Assets/Tenjin/Scripts/Editor/"))
                dstRelative = "Editor/" + Path.GetFileName(srcPath);
            else if (srcPath.StartsWith("Assets/Tenjin/Scripts/"))
                dstRelative = "Runtime/" + Path.GetFileName(srcPath);
            else if (srcPath.StartsWith("Assets/Editor/"))
                dstRelative = "Editor/" + Path.GetFileName(srcPath);
            else if (srcPath.StartsWith("Assets/Plugins/"))
                dstRelative = srcPath.Replace("Assets/Plugins/", "Plugins/");
            else
                continue;

            string dstPath = Path.Combine(upmOutputPath, dstRelative);
            string dstDir = Path.GetDirectoryName(dstPath);
            if (!Directory.Exists(dstDir))
                Directory.CreateDirectory(dstDir);

            string fullSrcPath = Path.GetFullPath(srcPath);
            if (File.Exists(fullSrcPath))
            {
                File.Copy(fullSrcPath, dstPath, true);
                count++;
            }
            else
            {
                Debug.LogWarning($"TenjinSDK UPM: Source file not found: {srcPath}");
            }
        }

        Debug.Log($"TenjinSDK UPM: Exported {count} files -> {upmOutputPath}");
    }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            BuildiOS(path);
        }
        else if (buildTarget == BuildTarget.Android)
        {
            BuildAndroid(path);
        }
    }

    private static void BuildAndroid(string path = "")
    {
        Debug.Log("TenjinSDK: Starting Android Build");
    }

    private static void BuildiOS(string path = "")
    {
#if UNITY_IOS
        Debug.Log("TenjinSDK: Starting iOS Build");

        string projectPath = Path.Combine(path, "Unity-iPhone.xcodeproj/project.pbxproj");
        PBXProject project = new PBXProject();
        project.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
        string buildTarget = project.GetUnityFrameworkTargetGuid();
#else
        string buildTarget = project.TargetGuidByName("Unity-iPhone");
#endif

        AddFrameworksToProject(project, buildTarget);
        AddLinkerFlags(project, buildTarget);
        UpdatePlist(path);

        File.WriteAllText(projectPath, project.WriteToString());
#endif
    }

#if UNITY_IOS
    [PostProcessBuild(50)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            EmbedSignFramework(path);
        }
    }

    public static void EmbedSignFramework(string path)
    {
        string projPath = PBXProject.GetPBXProjectPath(path);
        if (!File.Exists(projPath))
        {
            Debug.LogError("Project file does not exist: " + projPath);
            return;
        }

        PBXProject proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(projPath));

        // Get the target GUID
        string unityFrameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();
        string targetGuid = proj.GetUnityMainTargetGuid();

        string zipPathInUnity = FindXCFrameworkZip();
        if (zipPathInUnity == null) return;
        string extractionPath = Path.Combine(path, "Frameworks");
        string frameworkPath = Path.Combine(extractionPath, "TenjinSDK.xcframework");

        if (Directory.Exists(frameworkPath))
        {
            Directory.Delete(frameworkPath, true);
        }

        try
        {
            ZipFile.ExtractToDirectory(zipPathInUnity, extractionPath);

            // Delete --MACOSX metadata folder
            string macosxMetaFolder = Path.Combine(extractionPath, "__MACOSX");
            if (Directory.Exists(macosxMetaFolder))
            {
                Directory.Delete(macosxMetaFolder, true);
            }
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to extract zip file: " + e.Message);
            return;
        }

        // Add the .xcframework to the Xcode project and embed it in the main target
        AddFrameworkToXcodeProject(proj, targetGuid, unityFrameworkTargetGuid, frameworkPath);

        File.WriteAllText(projPath, proj.WriteToString());
    }

    private static void AddFrameworkToXcodeProject(PBXProject proj, string targetGuid, string unityFrameworkTargetGuid, string frameworkPath)
    {
        string fileGuid = proj.AddFile(frameworkPath, "Frameworks/TenjinSDK.xcframework");
        proj.AddFileToEmbedFrameworks(targetGuid, fileGuid);

        proj.AddFileToBuildSection(targetGuid, proj.GetFrameworksBuildPhaseByTarget(targetGuid), fileGuid);
        proj.AddFileToBuildSection(unityFrameworkTargetGuid, proj.GetFrameworksBuildPhaseByTarget(unityFrameworkTargetGuid), fileGuid);
    }

    private static void AddFrameworksToProject(PBXProject project, string buildTarget)
    {
        List<string> frameworks = new List<string>
        {
            "AdServices.framework",
            "AdSupport.framework",
            "AppTrackingTransparency.framework",
            "StoreKit.framework"
        };

        foreach (string framework in frameworks)
        {
            Debug.Log("TenjinSDK: Adding framework: " + framework);
            project.AddFrameworkToProject(buildTarget, framework, true);
        }
    }

    private static void AddLinkerFlags(PBXProject project, string buildTarget)
    {
        Debug.Log("TenjinSDK: Adding -ObjC flag to other linker flags (OTHER_LDFLAGS)");
        project.AddBuildProperty(buildTarget, "OTHER_LDFLAGS", "-ObjC");
    }

    private static void UpdatePlist(string path)
    {
        string plistPath = Path.Combine(path, "Info.plist");
        PlistDocument plist = new PlistDocument();

        plist.ReadFromFile(plistPath);

        plist.root.SetString("NSUserTrackingUsageDescription",
                "We request to track data to enhance ad performance and user experience. Your privacy is respected.");

        File.WriteAllText(plistPath, plist.WriteToString());
    }

#endif
}
