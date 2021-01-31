using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {
        //since it's not the game playing, doesn't need to save curState
        SettingsScript.instance.ShowSettings();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
