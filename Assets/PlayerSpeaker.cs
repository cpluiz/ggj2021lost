using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeaker : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text text;
    public string playerName = "Player";

    [SerializeField]
    string playerNameIdentifier = "$PLAYERNAME";
    [SerializeField]
    string textStringIdentifier = "$TEXT";


    string baseText = "<b><color=#ff2222>$PLAYERNAME:</color></b><color=white>$TEXT</color>";


    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        Speak("I'm speaking nonsense for 5 secs!", 5);
    }

    public void Speak(string speakText, float secs)
    {
        Speak(speakText);
        StartCoroutine(DeactivateDialogue(secs));
    }

    IEnumerator DeactivateDialogue(float secs)
    {
        yield return new WaitForSeconds(secs);
        dialogueBox.SetActive(false);

    }

    public void Speak(string speakText)
    {
        var newText = baseText;

        newText = newText.Replace(playerNameIdentifier, playerName);
        newText = newText.Replace(textStringIdentifier, speakText);

        text.text = newText;

        dialogueBox.SetActive(true);

    }
    public void Speak(string charactername, string speakText)
    {
        var newText = baseText;

        newText = newText.Replace(playerNameIdentifier, charactername);
        newText = newText.Replace(textStringIdentifier, speakText);

        text.text = newText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
