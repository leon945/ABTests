using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{    
    public GameObject sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(sceneLoader);

        Application.targetFrameRate = 60;

        StartCoroutine(waitThenStartGame());
    }

    private IEnumerator waitThenStartGame()
    {
        yield return new WaitForSeconds(0.1f);

        if (!AssetBundleManager.areAssetsDownloaded)
        {
            Caching.ClearCache();
            DownloadAssetsController.assetBundlesToDownload.Clear();
            DownloadAssetsController.assetBundlesToDownload.AddRange(AssetBundleManager.getSceneDependencies(Constants.testSceneIndex));
            DownloadAssetsController.sceneAfterDownload = Constants.loadingSceneIndex;
            DownloadAssetsController.loadToMemory = false;
            SceneManager.LoadScene(Constants.downloadAssetsSceneIndex);
        }
        else
        {            
            SceneLoader.instance.loadScene(Constants.testSceneIndex);            
        }
    }

}
