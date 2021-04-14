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
        var atlas = Instantiate(ab.LoadAsset<SpriteAtlas>("test_ab_sa"));
        var sprite = atlas.GetSprite("CBR_UI_displayPanel_360x480_iphone5");
        var ui = Instantiate(ab.LoadAsset<GameObject>("testui"));
        ui.GetComponent<UiRefs>().panel.sprite = sprite;
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
