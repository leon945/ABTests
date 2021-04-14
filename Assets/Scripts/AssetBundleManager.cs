using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
    public static bool areAssetsDownloaded = false;

    private static Dictionary<int, AssetBundle> loadedAssetsBundles = new Dictionary<int, AssetBundle>();

    public static void addAssetBundle(Enums.AssetBundleIdentifiers bundle, AssetBundle assetBundle)
    {
        if (loadedAssetsBundles.ContainsKey((int)bundle))
        {
            loadedAssetsBundles.Remove((int)bundle);
        }

        loadedAssetsBundles.Add((int)bundle, assetBundle);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundle"></param>
    /// <returns>Returns the asset bundle or null if it's not loaded.</returns>
    public static AssetBundle getAssetBundle(Enums.AssetBundleIdentifiers bundle)
    {
        if (loadedAssetsBundles.ContainsKey((int)bundle))
        {
            return loadedAssetsBundles[(int)bundle];
        }
        else
        {
            return null;
        }
    }

    public static void removeAssetBundle(Enums.AssetBundleIdentifiers bundle)
    {
        if (loadedAssetsBundles.ContainsKey((int)bundle))
        {
            Debug.Log("Unloading asset bundle: " + bundle.ToString());
            if (loadedAssetsBundles[(int)bundle] != null)
                loadedAssetsBundles[(int)bundle].Unload(true);

            loadedAssetsBundles.Remove((int)bundle);
        }
    }

    private static Dictionary<int, int> assetBundleVersions = new Dictionary<int, int>();

    public static void setAssetBundleVersion(Enums.AssetBundleIdentifiers bundle, int version)
    {
        if (assetBundleVersions.ContainsKey((int)bundle))
        {
            assetBundleVersions.Remove((int)bundle);
        }

        assetBundleVersions.Add((int)bundle, version);

        PlayerPrefs.SetInt("bundle-version-" + (int)bundle, version);
    }

    public static int getAssetBundleVersion(Enums.AssetBundleIdentifiers bundle)
    {
        if (assetBundleVersions.ContainsKey((int)bundle))
        {
            return assetBundleVersions[(int)bundle];
        }
        else
        {
            if (PlayerPrefs.GetInt("bundle-version-" + (int)bundle, -1) == -1)
            {
                return 1;
            }
            else
            {
                return PlayerPrefs.GetInt("bundle-version-" + (int)bundle, -1);
            }
        }
    }

    public static bool areAssetBundleVersionsLoaded()
    {
        return assetBundleVersions.Keys.Count > 0;
    }

    public static void unloadAssetBundlesForScene(int sceneIndex)
    {
        var deps = getSceneDependencies(sceneIndex);
        foreach (Enums.AssetBundleIdentifiers id in deps)
        {
            removeAssetBundle(id);
        }
    }

    public static List<Enums.AssetBundleIdentifiers> getSceneDependencies(int sceneIndex)
    {
        var deps = new List<Enums.AssetBundleIdentifiers>();
        switch (sceneIndex)
        {            
            case Constants.testSceneIndex:
                //deps.Add(Enums.AssetBundleIdentifiers.UI_2D_TEXT_IPHONE5);
                //deps.Add(Enums.AssetBundleIdentifiers.UI_2D_IPHONE5);
                deps.Add(Enums.AssetBundleIdentifiers.TEST_AB);
                break;
        }

        return deps;
    }

    public static AssetBundle get2DUIAssetBundle()
    {
        return getAssetBundle(Enums.AssetBundleIdentifiers.TEST_AB);
    }
}
