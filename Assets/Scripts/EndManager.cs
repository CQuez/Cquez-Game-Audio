using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{

    FMOD.Studio.Bus MasterB;

    // Start is called before the first frame update
    void Start()
    {
        MasterB = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnReplayButton()
    {
        MasterB.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        SceneManager.LoadScene("MainWorld");
    }
}
