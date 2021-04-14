using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RetryDownloadBtn : MonoBehaviour
{
    public UnityEvent retryEvent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        Debug.Log("Retry clicked");
        retryEvent.Invoke();
    }
}
