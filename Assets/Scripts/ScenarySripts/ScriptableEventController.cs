using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableEventController : MonoBehaviour{

    private static ScriptableEventController _instance;

    public static ScriptableEventController instance{ get => _instance; }

    public void Awake(){
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);
        }else if (_instance != this) {
            Destroy(gameObject);
        }
    }

    public void Start(){
        
    }

    private void GameStart(){
        
    }
}
