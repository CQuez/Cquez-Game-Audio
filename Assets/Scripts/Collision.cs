using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    Dialogue dialogue;
    public PlayerControl player;



    /// <summary>
    /// When the player collides with an Enemy
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //Cue Dialogue when interacting with Enemies
            Dialogue.Creation.ShowLines(dialogue, collision);
            player.OnDisable();
        }
        if (collision.gameObject.name == "LevelEnd")
        {
            SceneManager.LoadScene("EndOfLevel");
        }
    }

}
