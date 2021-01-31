using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = System.Object;
using fastJSON;

public class SettingsScript : MonoBehaviour
{
    static SettingsScript _instance;
    public static SettingsScript instance { get { return _instance; } }


    public Slider textSpeedSlider;
    public Slider volumeSlider;

    public TMPro.TMP_Dropdown languageDropdown;
    [SerializeField]
    List<string> availableLanguages;

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        textSpeedSlider.onValueChanged.AddListener(delegate { ChangeTextSpeed(); });
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });

        volumeSlider.value = AudioListener.volume;


        languageDropdown.ClearOptions();

        languageDropdown.AddOptions(availableLanguages);
        languageDropdown.onValueChanged.AddListener(delegate { ChangeLanguage(); });
        gameObject.SetActive(false);
    }

    public void Back()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
    }
    public void ShowSettings()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        gameObject.SetActive(true);
    }


    public void ChangeTextSpeed()
    {
        Debug.Log("Implement change textSpeed here using " + textSpeedSlider.value);
    }
    public void ChangeLanguage()
    {
        Debug.Log("Implement change language here using " + languageDropdown.value);
        TextController.instance.LoadLanguage(languageDropdown.options[languageDropdown.value].text);
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
