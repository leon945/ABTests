using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

    }

    public void loadScene(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
            return;
        }

        var deps = AssetBundleManager.getSceneDependencies(sceneIndex);

        if (deps.Count > 0)
        {
            DownloadAssetsController.assetBundlesToDownload.Clear();
            DownloadAssetsController.assetBundlesToDownload.AddRange(deps);
            DownloadAssetsController.sceneAfterDownload = sceneIndex;
            DownloadAssetsController.loadToMemory = true;
            StartCoroutine(SceneSwitch(Constants.downloadAssetsSceneIndex));
        }
        else
        {
            StartCoroutine(SceneSwitch(sceneIndex));
        }
    }

    private IEnumerator SceneSwitch(int sceneIndex)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneIndex);
        yield return load;
        AssetBundleManager.unloadAssetBundlesForScene(currentSceneIndex);
    }
}
