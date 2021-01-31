using System.Collections;
using UnityEngine;
using TMPro;

public class Speaker : MonoBehaviour{
    
    // TODO - Make the text appear in separated dialog box linked to the characters - for now show all the messages on the same box area
    // For now using different colors with character prefix to identify the speaker

    //Singleton pattern - it's not pretty, but i like
    private static Speaker _instance;
    public static Speaker instance{ get => _instance; }

    public DialogBox targetBox;

    public void Awake(){
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);
        }else if (_instance != this) {
            Destroy(gameObject);
        }
    }
    //end singleton pattern
    
    private Color textColor = Color.black;

    private string fullText = "";
    private float slowSpeed = 0.2f;
    private float normalSpeed = 0.1f;
    private float rapidSpeed = 0.05f;
    private float spd = 0f;
    private float waitDelay = 4f;
    public float speed { get { return spd; } }
    public bool showNext = false;
    private bool completedText = false;
    public static bool completed { get { return _instance.completedText; } }
    
    public void WriteTextById(string textId, bool hasNext) {
        //EnableCanvas();
        _instance.textColor = TextController.GetTextColor(textId);
        completedText = false;
        StopAllCoroutines();
        showNext = hasNext;
        StartCoroutine(WriteDelay(TextController.GetString(textId)));
    }

    public static void WriteTextById(string textId) {
        _instance.WriteTextById(TextController.GetString(textId),false);
    }

    public static void UpdateText(string txt) {
        instance.targetBox.SetText(txt);
    }
    
    public static void Skip() {
        if(instance.completedText) {
            DisableCanvas();
        } else {
            instance.StopAllCoroutines();
            UpdateText(instance.fullText);
            instance.completedText = true;
            instance.StartCoroutine(instance.AutoNext(instance.waitDelay*2));
        }
    }
    
    private IEnumerator WriteDelay(string text) {
        char[] newText;
        _instance.targetBox.SetColor(_instance.textColor);
        newText = new char[text.Length];
        fullText = text;
        int count = 0;
        int i = 0;
        UpdateText(new string(newText));
        while (count < fullText.Length) {
            yield return new WaitForSecondsRealtime(speed);
            newText[i++] = fullText[count++];
            UpdateText(new string(newText));
        }
        _instance.completedText = true;
        StartCoroutine(AutoNext(_instance.waitDelay));
    }
    
    private IEnumerator AutoNext(float interval) {
        yield return new WaitForSecondsRealtime(interval);
        if(ShowDialogText.instance.hasNextMessage)
            ShowDialogText.WriteMessage();
        else
            DisableCanvas();
    }

    public static void DisableCanvas(){
        instance.targetBox.gameObject.SetActive(false);
    }
    
    public static void SetTextSpeed(TextSpeed spd) {
        switch(spd) {
            case TextSpeed.slow:
                _instance.spd = _instance.slowSpeed;
                break;
            case TextSpeed.normal:
                _instance.spd = _instance.normalSpeed;
                break;
            case TextSpeed.fast:
                _instance.spd = _instance.rapidSpeed;
                break;
            default:
                _instance.spd = _instance.normalSpeed;
                break;
        }
    }
    
    public enum TextSpeed { slow,normal,fast }
}
