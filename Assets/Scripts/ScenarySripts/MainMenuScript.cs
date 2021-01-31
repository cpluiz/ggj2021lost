using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour{

    public void NewGame(){
        SceneManager.LoadScene("GameScene");
    }

    public void Settings(){
        Debug.Log("Settings here");
    }

    public void Credits(){
        Debug.Log("Credits here \\o>");
    }

    public void CloseGame(){
        Application.Quit();
    }
}
