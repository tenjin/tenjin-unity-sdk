//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tenjin
{
    //a simple wrapper class because Unity's json serializer can't deal with primitives..
    [Serializable]
    internal class ManifestWrapper
    {
        public string[] files;
    }

    public static class Exporter
    {
        static void Package()
        {
            var files = TenjinPackager.LoadManifest();
            TenjinPackager.PublishPackage(files);
        }

        [MenuItem("Tenjin/Export Package")]
        internal static void PackageInteractively()
        {
            var files = TenjinPackager.LoadManifest();
            TenjinPackager.PublishPackage(files, TenjinPackager.EXPORTED_PACKAGE_PATH, true);
        }
    }

    internal class TenjinPackager
    {
        const string MANIFEST_PATH = "Assets/tenjin.unitypackage.manifest";
        public const string EXPORTED_PACKAGE_PATH = "TenjinUnityPackage.unitypackage";

        internal static void SaveManifestFile(IEnumerable<String> assets)
        {
            if (File.Exists(MANIFEST_PATH))
                File.Delete(MANIFEST_PATH);

            var wtf = new ManifestWrapper() { files = assets.ToArray() };
            var json = JsonUtility.ToJson(wtf);

            var writer = new StreamWriter(MANIFEST_PATH, false);

            writer.WriteLine(json);
            writer.Close();
        }

        internal static IEnumerable<string> LoadManifest()
        {
            var reader = new StreamReader(MANIFEST_PATH);
            var jsonString = reader.ReadToEnd();
            reader.Close();

            var wrappedJson = JsonUtility.FromJson<ManifestWrapper>(jsonString);

            return wrappedJson.files;
        }

        internal static void PublishPackage(IEnumerable<string> enumerable, string path = EXPORTED_PACKAGE_PATH, bool interactive = false)
        {
            if (File.Exists(path))
                File.Delete(path);

            var options = ExportPackageOptions.IncludeDependencies;
            if (interactive)
                options = options | ExportPackageOptions.IncludeDependencies;
            var filePaths = enumerable.ToArray();

            Debug.Log("Exporting files :\n" + string.Join("\n", filePaths));

            AssetDatabase.ExportPackage(filePaths, path, options);
        }


    }
}