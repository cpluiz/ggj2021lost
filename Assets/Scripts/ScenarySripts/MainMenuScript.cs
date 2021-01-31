using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour{

    public void NewGame(){
        SceneManager.LoadScene("SampleScene");
    }

    public void Settings(){
        SettingsScript.instance.ShowSettings();
    }

    public void Credits(){
        SceneManager.LoadScene("CreditsScene");
    }

    public void CloseGame(){
        Application.Quit();
    }
}
