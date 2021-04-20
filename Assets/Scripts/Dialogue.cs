using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string talkSound;

    public GameObject dBox;
    public Text dtext;
    public List<string> phraseLines;

    public BattleManager combat;

    // Start is called before the first frame update
    void Start()
    {
        Creation = this;
    }

    /// <summary>
    /// Display dialogue box after choosing a random line
    /// </summary>
    public static Dialogue Creation {get; private set;}

    public void ShowLines(Dialogue dialogue, Collider2D enem)
    {
        dBox.SetActive(true);
        StartCoroutine(dLine(phraseLines[Random.Range(0, phraseLines.Count)], enem));
    }

    /// <summary>
    /// Write it letter by letter
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns></returns>
    public IEnumerator dLine(string phrase, Collider2D enem)
    {
        dtext.text = "";
        
        foreach (var i in phrase.ToCharArray())
        {
            dtext.text += i;
            FMODUnity.RuntimeManager.PlayOneShot(talkSound);
            yield return new WaitForSeconds(1f/10);
        }

        yield return new WaitForSeconds(2f);
        combat.BeginBattle();
        Destroy(enem.gameObject);
        dBox.SetActive(false);
    }

}
