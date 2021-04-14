using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DownloadAssetsController : MonoBehaviour
{
    public static List<Enums.AssetBundleIdentifiers> assetBundlesToDownload = new List<Enums.AssetBundleIdentifiers>();
    public static int sceneAfterDownload;
    public static bool loadToMemory;

    public Text percentText;
    public Text loadingText;
    public Text assetBundleLoadingText;
    public Text errorText;
    public GameObject retryButton;
    public GameObject loadbarFg;

    private float loadbarFgWidth;

    private int index = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        retryButton.SetActive(false);
        var distinctList = new List<Enums.AssetBundleIdentifiers>();
        foreach (Enums.AssetBundleIdentifiers id in assetBundlesToDownload)
        {
            if (!distinctList.Contains(id))
            {
                distinctList.Add(id);
            }
        }

        assetBundlesToDownload = distinctList;

        Rect r = loadbarFg.GetComponent<RectTransform>().rect;
        loadbarFgWidth = r.width;
        loadbarFg.GetComponent<RectTransform>().sizeDelta = new Vector2(0, r.height);
    }

    void Start()
    {
        index = 0;
        percentText.text = "0%";
        if (loadToMemory)
        {
            loadingText.text = "LOADING";
        }
        else
        {
            loadingText.text = "DOWNLOADING";
        }

        if (AssetBundleManager.areAssetBundleVersionsLoaded())
        {
            StartDownload();
        }
        else
        {
            checkForNewVersions();
        }
    }

    public void checkForNewVersions()
    {
        StartCoroutine(Downloader.getAssetBundleVersions(checkForNewVersionsCallback));
    }

    private void checkForNewVersionsCallback(bool success, List<AssetBundleVersionDTO> versions)
    {
        if (success)
        {
            foreach (AssetBundleVersionDTO abv in versions)
            {
                AssetBundleManager.setAssetBundleVersion((Enums.AssetBundleIdentifiers)abv.bundleId, abv.bundleVersion);
            }
        }

        StartDownload();
    }

    public void StartDownload()
    {
        Debug.Log("Starting assets download...");
        index = 0;
        percentText.text = "0%";
        errorText.text = "";
        retryButton.SetActive(false);
        if (index < assetBundlesToDownload.Count)
        {
            StartCoroutine(Downloader.loadBundle(assetBundlesToDownload[index], bundleLoaded, downloadProgress, false));
        }
    }

    private void downloadProgress(UnityWebRequest req)
    {
        StartCoroutine(downloadProgressRoutine(req));
    }

    private IEnumerator downloadProgressRoutine(UnityWebRequest req)
    {

        long time = DateTime.Now.Ticks;
        float prevProgress = 0;
        assetBundleLoadingText.text = assetBundlesToDownload[index].ToString();
        while (!req.isDone && !req.isHttpError && !req.isNetworkError)
        {
            if (prevProgress == req.downloadProgress)
            {
                long ttime = DateTime.Now.Ticks;
                long secondsPassed = (ttime - time) / 10000 / 1000;
                if (secondsPassed > 10)
                {
                    Debug.LogError("Aborted download. Timeout.");
                    req.Abort();
                }
            }
            else
            {
                time = DateTime.Now.Ticks;
            }

            prevProgress = req.downloadProgress;

            float p = ((float)(index) / (float)assetBundlesToDownload.Count * 100) + req.downloadProgress * (1 / (float)assetBundlesToDownload.Count) * 100f;
            if (p < 0) p = 0;
            percentText.text = Mathf.RoundToInt(p) + "%";
            Rect r = loadbarFg.GetComponent<RectTransform>().rect;
            r.width = loadbarFgWidth * (p / 100);
            loadbarFg.GetComponent<RectTransform>().sizeDelta = new Vector2(r.width, r.height);
            yield return null;
        }

        if (req.isDone && !req.isHttpError && !req.isNetworkError)
        {
            float p = ((float)index / (float)assetBundlesToDownload.Count) * 100f;
            percentText.text = Mathf.RoundToInt(p) + "%";

            Rect r = loadbarFg.GetComponent<RectTransform>().rect;
            r.width = loadbarFgWidth * (p / 100);
            loadbarFg.GetComponent<RectTransform>().sizeDelta = new Vector2(r.width, r.height);
        }
        else if (req.isHttpError || req.isNetworkError)
        {
            percentText.text = "";
            errorText.text = "Unable to download.\nCheck your internet connection.";
            retryButton.SetActive(true);

            Rect r = loadbarFg.GetComponent<RectTransform>().rect;
            loadbarFg.GetComponent<RectTransform>().sizeDelta = new Vector2(0, r.height);
        }

    }

    private void bundleLoaded(AssetBundle assetBundle)
    {
        if (!loadToMemory)
        {
            AssetBundleManager.removeAssetBundle(assetBundlesToDownload[index]);
        }

        index++;

        if (index < assetBundlesToDownload.Count)
        {
            StartCoroutine(Downloader.loadBundle(assetBundlesToDownload[index], bundleLoaded, downloadProgress, false));
        }
        else
        {
            AssetBundleManager.areAssetsDownloaded = true;
            StartCoroutine(loadNextScene(.25f));
        }
    }

    private IEnumerator loadNextScene(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        if (sceneAfterDownload == Constants.testSceneIndex)
        {
            Debug.Log("Sprite Atlas loading...");
            
            //var load = AssetBundleManager.get2DUIAssetBundle().LoadAsset<SpriteAtlas>("test_ab_sa");
            //yield return load;
        }
        assetBundlesToDownload.Clear();
        SceneManager.LoadScene(sceneAfterDownload);
    }
}
