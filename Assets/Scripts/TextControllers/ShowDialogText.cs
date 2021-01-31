using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialogText : MonoBehaviour{

    private static ShowDialogText _instance;
    public static ShowDialogText instance { get => _instance;}
    public void Awake(){
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);
        }else if (_instance != this) {
            Destroy(gameObject);
        }
    }
    
    private string[] messageIDs;
    private int index = 0;
    public string nextMessage { get { return messageIDs[index++]; } }
    public bool hasNextMessage { get { return index < messageIDs.Length; } }
    
    public static void WriteMessage() {
        if(instance.hasNextMessage) {
            Speaker.instance.WriteTextById(instance.nextMessage,instance.hasNextMessage);
        }
    }

}
