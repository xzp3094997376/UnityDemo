using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assetbundle/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = Application.streamingAssetsPath+ "/AssetBundles";
        assetBundleDirectory = Application.dataPath + "/AssetbundleTest/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }
}