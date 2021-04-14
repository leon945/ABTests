using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using UnityEngine.Events;

public class Downloader
{    
    const string baseUrl = "https://sg-cbr-test.uc.r.appspot.com/";
    //const string baseUrl = "http://localhost:8080/";

#if HEADLESS_CLIENT && UNITY_STANDALONE_LINUX
    const string assetBundlePathOnDisk = "/home/ubuntu/unity/asset_bundles/";
#elif HEADLESS_CLIENT && (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
    const string assetBundlePathOnDisk = "./Assets/AssetBundles/";
#else
    const string bundleBaseUrl = "https://cbrwebgl.s3.us-east-2.amazonaws.com/";
#endif

    private static UnityWebRequest makePostRequest(string url, string json)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        return webRequest;
    }

    public static IEnumerator getAssetBundleVersions(Action<bool, List<AssetBundleVersionDTO>> onServerCallComplete)
    {
        bool success = false;
        string urlString = baseUrl + "assets/versions";

        List<AssetBundleVersionDTO> versions = new List<AssetBundleVersionDTO>();

        UnityWebRequest webRequest = UnityWebRequest.Get(urlString);

        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError(webRequest.error);
        }
        else
        {
            // Show results as text
            Debug.Log("Attempted to get asset bundle versions:");
            Debug.Log("Response: " + webRequest.downloadHandler.text);

            CbrResponse<List<AssetBundleVersionDTO>> response = JsonUtility.FromJson<CbrResponse<List<AssetBundleVersionDTO>>>(webRequest.downloadHandler.text);

            if (response == null)
            {
                Debug.LogError("RESPONSE IS NULL");
            }
            else
            {
                int statusCode = response.statusCode;
                Debug.Log("Status code: " + statusCode);

                if (statusCode == StatusCodes.SUCCESS)
                {
                    versions = response.data;
                    success = true;
                    Debug.Log("Asset versions obtained!");
                }
            }
        }

        onServerCallComplete(success, versions);
    }


    public static IEnumerator loadBundle(Enums.AssetBundleIdentifiers bundleIdentifier, UnityAction<AssetBundle> callback, UnityAction<UnityWebRequest> downloadProgress, bool shouldBeCached)
    {

        string platformBundleName = bundleIdentifier.ToString().ToLower();
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        platformBundleName += "-Mac";
#elif UNITY_ANDROID
        platformBundleName += "-Android";
#elif UNITY_IOS
        platformBundleName += "-IOS";
#elif UNITY_STANDALONE_LINUX
        platformBundleName += "-Linux";
#endif

#if HEADLESS_CLIENT
        Debug.Log("Loading asset bundle from disk: " + assetBundlePathOnDisk + platformBundleName);
        
        AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePathOnDisk + platformBundleName);
        if (assetBundle != null)
        {
            Debug.Log("Asset bundle loaded from disk!");
            AssetBundleManager.addAssetBundle(bundleIdentifier, assetBundle);
            callback(assetBundle);
        }
        else
        {
            Debug.LogError("BUNDLE IS NULL");
        }
        yield return null;
#else
        List<Hash128> cachedVersions = new List<Hash128>();
        Caching.GetCachedVersions(platformBundleName, cachedVersions);
        Hash128 versionCheckHash = Hash128.Compute(AssetBundleManager.getAssetBundleVersion(bundleIdentifier).ToString());
        if (!cachedVersions.Contains(versionCheckHash) && shouldBeCached)
        {
            Debug.LogError("=============");
            Debug.LogError("Asset bundle: '" + platformBundleName + "' should have been cached, but isn't.");
            Debug.LogError("=============");
        }

        if (!cachedVersions.Contains(versionCheckHash) && AssetBundleManager.getAssetBundle(bundleIdentifier) != null)
        {
            Debug.Log("Removing old asset bundle version from cache.");
            AssetBundleManager.removeAssetBundle(bundleIdentifier);
        }

        string bundleUrl = bundleBaseUrl + platformBundleName;
        Debug.Log("Bundle URL: " + bundleUrl);

        UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl, versionCheckHash, 0);
        if (downloadProgress != null)
        {
            downloadProgress(req);
        }
        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            Debug.LogError("Network error. " + req.responseCode);
        }
        else
        {
            if (AssetBundleManager.getAssetBundle(bundleIdentifier) != null)
            {
                Debug.Log("Asset bundle loaded from memory!");
                callback(AssetBundleManager.getAssetBundle(bundleIdentifier));
            }
            else
            {
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(req);
                if (assetBundle != null)
                {
                    Debug.Log("Asset bundle loaded!");
                    AssetBundleManager.addAssetBundle(bundleIdentifier, assetBundle);
                    callback(assetBundle);
                }
                else
                {
                    Debug.LogError("BUNDLE IS NULL");
                }
            }
        }
#endif
    }
}
