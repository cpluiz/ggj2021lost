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
    [SerializeField] private Dictionary<string, TextStructure> textStrings;
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
        textStrings = JSON.ToObject<Dictionary<string,TextStructure>>(languageFile.ToString());
        loadedLanguage = languageCode;
    }


    public static string GetString(string stringId){
        if(_instance.languageChanged) _instance.LoadLanguage(_instance.languageId);
        TextStructure textStructure;
        if(_instance.textStrings.TryGetValue(stringId, out textStructure))
            return textStructure.text;
        return stringId;
    }

    public static string GetCharacterName(string stringId){
        if(_instance.languageChanged) _instance.LoadLanguage(_instance.languageId);
        TextStructure textStructure;
        if(_instance.textStrings.TryGetValue(stringId, out textStructure))
            return textStructure.characterName;
        return stringId;
    }
    
    public static string GetTargetType(string stringId){
        if(_instance.languageChanged) _instance.LoadLanguage(_instance.languageId);
        TextStructure textStructure;
        if(_instance.textStrings.TryGetValue(stringId, out textStructure))
            return textStructure.targetType;
        return stringId;
    }
    
    public static Color GetTextColor(string stringId){
        if(_instance.languageChanged) _instance.LoadLanguage(_instance.languageId);
        TextStructure textStructure;
        Debug.Log(stringId);
        Color color = Color.white;
        if (_instance.textStrings.TryGetValue(stringId, out textStructure))
            ColorUtility.TryParseHtmlString(textStructure.textColor, out color);
        Debug.Log(textStructure.textColor);
        return color;
    }
    
}

public class TextStructure{
    public string targetType;
    public string characterName;
    public string textColor;
    public string text;
}