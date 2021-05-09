using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    //Sounds
    [FMODUnity.EventRef]
    public string hitSound;
    [FMODUnity.EventRef]
    public string critSound;
    [FMODUnity.EventRef]
    public string recievehitSound;
    [FMODUnity.EventRef]
    public string missSound;
    [FMODUnity.EventRef]
    public string defeatedSound;
    [FMODUnity.EventRef]
    public string monsterDeathSound;
    [FMODUnity.EventRef]
    public string menuSelect;
    [FMODUnity.EventRef]
    public string HealthWarning;
    [FMODUnity.EventRef]
    public string BuildUp;
    [FMODUnity.EventRef]
    public string EnemBuildUp;

    //Music
    [FMODUnity.EventRef]
    public string CombatMusic;

    FMOD.Studio.EventInstance HWarn;
    FMOD.Studio.EventInstance CMusic;

    //HUD Accessors
    public GameObject combatBox;
    public Text healthText;
    public Text ResultText;

    //Player and Camera
    public PlayerControl player;
    public List<Camera> Cams;

    //Player Stats
    int Health;
    int attack = 5;

    //float critChance;
    float missChance;

    //Enemy Stats
    int EnHealth = 10;

    public int selector;

    bool inBattle = false;
    public bool EnemyTurn = true;

    public enum Availstate
    {
        Victory, 
        Defeat,
        PlayTurn, 
        EnemTurn, 
    }

    public Availstate state;

    private Button attackButton;
    private Button spcAttackButton;

    /// <summary>
    /// Player begins combat full health and stats
    /// </summary>
    public void BeginBattle()
    {
        SwitchToBattle();

        attackButton = GameObject.Find("Attack").GetComponent<Button>();
        spcAttackButton = GameObject.Find("Special Attack").GetComponent<Button>();

        inBattle = true;
        Health = 25;
        attack = 3;

        EnHealth = 10;
        

        HWarn = FMODUnity.RuntimeManager.CreateInstance(HealthWarning);
        HWarn.start();
        HWarn.setParameterByName("Health Warning", 10f);

        CMusic = FMODUnity.RuntimeManager.CreateInstance(CombatMusic);
        CMusic.start();
        CMusic.setParameterByName("Combat Music Level", 1f);

        //critChance = .10f;
        missChance = .50f;

        state = Availstate.EnemTurn;
        StartCoroutine(theEnemyTurn());
    }

    /// <summary>
    /// Player can move again
    /// </summary>
    public void EndBattle()
    {
        inBattle = false;
        player.OnEnable();
        SwitchToOpen();
    }

    /// <summary>
    /// Switch between cameras
    /// </summary>
    /// 
    private void SwitchToBattle()
    {
        Cams[1].gameObject.SetActive(true);
        Cams[1].enabled = true;
        Cams[0].gameObject.SetActive(false);
        Cams[0].enabled = false;
    }
    private void SwitchToOpen()
    {
        Cams[0].gameObject.SetActive(true);
        Cams[0].enabled = true;
        Cams[1].gameObject.SetActive(false);
        Cams[1].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        healthText.text = "Health: " + Health;


        if (Health < 18)
        {
            HWarn.setParameterByName("Health Warning", 50f);
        }
        if (Health < 11)
        {
            HWarn.setParameterByName("Health Warning", 100f);
        }
        else if(inBattle == false)
        {
            HWarn.setParameterByName("Health Warning", 0f);
            CMusic.setParameterByName("Combat Music Level", 0f);
        }
    }

    public void OnAttackButton()
    {

        attackButton.enabled = false;
        spcAttackButton.enabled = false;
        selector = 0;
        StartCoroutine(thePlayerTurn());
    }

    public void OnspcAttackButton()
    {

        attackButton.enabled = false;
        spcAttackButton.enabled = false;
        selector = 1;
        StartCoroutine(thePlayerTurn());
    }

    public void buttonEnter()
    {
        if(attackButton.enabled)
        {
            FMODUnity.RuntimeManager.PlayOneShot(menuSelect);
        }
    }

    private int Attack(int pts)
    {
        float result = Random.Range(0, 101);

        if (result < (missChance/100))
        {
            FMODUnity.RuntimeManager.PlayOneShot(missSound);
            ResultText.text = "Result: PLAYER MISSED";
            return 0;
        }

        FMODUnity.RuntimeManager.PlayOneShot(hitSound);
        ResultText.text = "Result: ENEMY IS HIT FOR " + attack + " DMG.";
        return pts;
    }

    private int spcAttack(int pts)
    {
        FMODUnity.RuntimeManager.PlayOneShot(critSound);
        ResultText.text = "Result: CRIT FOR " + pts + " DMG!";
        return pts;
    }

    private int isHit(int pts)
    {
        float result = Random.Range(0, 101);

        if (result < (missChance / 100))
        {
            FMODUnity.RuntimeManager.PlayOneShot(missSound);
            ResultText.text = "Result: ENEMY MISSED";
            return 0;
        }

        FMODUnity.RuntimeManager.PlayOneShot(recievehitSound);
        ResultText.text = "Result: PLAYER IS HIT FOR " + pts + " DMG.";
        return pts;
    }

    public IEnumerator theEnemyTurn()
    {
        attackButton.enabled = false;
        spcAttackButton.enabled = false;
        FMODUnity.RuntimeManager.PlayOneShot(EnemBuildUp);
        yield return new WaitForSeconds(3f);
        Health -= isHit(4);

        attackButton.enabled = true;
        spcAttackButton.enabled = true;
    }

    public IEnumerator thePlayerTurn()
    {
        FMODUnity.RuntimeManager.PlayOneShot(BuildUp);
        yield return new WaitForSeconds(2f);
        if (selector == 0)
        {
            EnHealth -= Attack(4);
        }
        if (selector == 1)
        {
            EnHealth -= spcAttack(7);
        }


        if (EnHealth <= 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(monsterDeathSound);
            EndBattle();
        }
        else if (Health <= 0)
        {
            EndBattle();
            SceneManager.LoadScene("EndOfLevel");
        }
        else
        {
            StartCoroutine(theEnemyTurn());
        }
    }


}
