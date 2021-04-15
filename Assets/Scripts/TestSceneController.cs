using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TestSceneController : MonoBehaviour
{
    public GameObject uiParent;

    private void Awake()
    {
        var ab = AssetBundleManager.get2DUIAssetBundle();        
        var ui = Instantiate(ab.LoadAsset<GameObject>("testui"));        
        ui.transform.parent = uiParent.transform;
        ui.transform.localPosition = Vector3.zero;
        ui.transform.localScale = Vector3.one;
        ui.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
