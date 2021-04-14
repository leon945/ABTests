using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAssetBundles
{
    public static string assetBundleDirectory = "Assets/AssetBundles/";

    [MenuItem("Assets/Build Asset Bundles")]
    static void BuildAssetBunddles()
    {
        //Delete and recreate the asset bunddle directory.
        if (Directory.Exists(assetBundleDirectory))
        {
            Directory.Delete(assetBundleDirectory, true);
        }
        Directory.CreateDirectory(assetBundleDirectory);

        //BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
        //AppendPlatformToFileName("IOS");
        //Debug.Log("IOS bundle created...");


        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneOSX);
        AppendPlatformToFileName("Mac");
        Debug.Log("Mac bundle created...");

        //BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneLinux64);
        //AppendPlatformToFileName("Linux");
        //Debug.Log("Linux bundle created...");

        //BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
        //AppendPlatformToFileName("Android");
        //Debug.Log("Android bundle created...");

        RemoveSpacesInFileNames();

        AssetDatabase.Refresh();
    }

    static void RemoveSpacesInFileNames()
    {
        foreach (string path in Directory.GetFiles(assetBundleDirectory))
        {
            string oldName = path;
            string newName = path.Replace(' ', '-');
            File.Move(oldName, newName);
        }
    }

    static void AppendPlatformToFileName(string platform)
    {
        foreach (string path in Directory.GetFiles(assetBundleDirectory))
        {
            //get filename
            string[] files = path.Split('/');
            string fileName = files[files.Length - 1];

            //delete files we dont need
            if (fileName.Contains(".") || fileName.Contains("Bundle"))
            {
                File.Delete(path);
            }
            else if (!fileName.Contains("-"))
            {
                //append platform to filename
                FileInfo info = new FileInfo(path);
                info.MoveTo(path + "-" + platform);
            }
        }
    }

}
