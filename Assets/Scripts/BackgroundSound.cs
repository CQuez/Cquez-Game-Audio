using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string WoodBackSound;
    [FMODUnity.EventRef]
    public string BackMusic;

    FMOD.Studio.EventInstance NormMusic;

    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Invoke("BackNoise", 0f);

        NormMusic = FMODUnity.RuntimeManager.CreateInstance(BackMusic);
        NormMusic.start();
        NormMusic.setParameterByName("Background Level", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam.enabled == false)
        {
            NormMusic.setParameterByName("Background Level", 0f);
        }
        else
        {
            NormMusic.setParameterByName("Background Level", 1f);
        }
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
