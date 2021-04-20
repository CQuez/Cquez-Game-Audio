using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{

    [FMODUnity.EventRef]
    public string WoodBackSound;

    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(WoodBackSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
