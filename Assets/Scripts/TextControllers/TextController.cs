using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using fastJSON;

public class TextController : MonoBehaviour{
    static TextController _instance;
    public static TextController instance{ get{ return _instance; } }

    [SerializeField] private Object languageFile;
    [SerializeField] private Dictionary<string, string> textStrings;
    [SerializeField] private string languageId, loadedLanguage = "ptBR";
    [SerializeField] private bool languageChanged{ get{ return loadedLanguage != languageId; }}

    public void Awake(){
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);
        }else if (_instance != this) {
            Destroy(gameObject);
        }
    }

    public void Start(){
        LoadLanguage(languageId);
    }

    public void LoadLanguage(string languageCode){
        languageFile = Resources.Load("language/" + languageCode);
        textStrings = JSON.ToObject<Dictionary<string,string>>(languageFile.ToString());
        loadedLanguage = languageCode;
    }


    public static string getString(string stringId){
        if(_instance.languageChanged) _instance.LoadLanguage(_instance.languageId);
        string text = stringId;
        _instance.textStrings.TryGetValue(stringId, out text);
        return text;
    }
}
