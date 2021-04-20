﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int EnAttack = 5;

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

    /// <summary>
    /// Player begins combat full health and stats
    /// </summary>
    public void BeginBattle()
    {
        SwitchToBattle();

        inBattle = true;
        Health = 25;
        attack = 3;

        EnHealth = 10;

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

    }

    public void OnAttackButton()
    {
        selector = 0;
        StartCoroutine(thePlayerTurn());
    }

    public void OnspcAttackButton()
    {
        selector = 1;
        StartCoroutine(thePlayerTurn());
    }

    public void buttonEnter()
    {
        FMODUnity.RuntimeManager.PlayOneShot(menuSelect);
    }

    private int Attack(int pts)
    {
        float result = Random.Range(0, 101);

        if (result < (missChance/100))
        {
            FMODUnity.RuntimeManager.PlayOneShot(missSound);
            ResultText.text = "Result: MISSED";
            return 0;
        }

        FMODUnity.RuntimeManager.PlayOneShot(hitSound);
        ResultText.text = "Result: ENEMY HIT FOR " + attack + " DMG.";
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
        ResultText.text = "Result: HIT FOR " + pts + " DMG.";
        return pts;
    }

    public IEnumerator theEnemyTurn()
    {
        yield return new WaitForSeconds(2f);
        Health -= isHit(4);
    }

    public IEnumerator thePlayerTurn()
    {

        yield return new WaitForSeconds(1f);
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
            EndBattle();
        }
        else
        {
            StartCoroutine(theEnemyTurn());
        }
    }


}
