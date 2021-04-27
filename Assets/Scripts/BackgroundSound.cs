using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string WoodBackSound;

    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Invoke("BackNoise", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackNoise()
    {
        if (mainCam.enabled)
        {
            FMODUnity.RuntimeManager.PlayOneShot(WoodBackSound);
        }
        else
        {
            CancelInvoke("BackNoise");
        }
    }
}
