using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class PreIntroScript : MonoBehaviour{
    public AudioSource audioSource;
    public bool locutorHasTalked = false;
    public AudioClip locutor, click;
    public Button clickButton;


    void Awake(){
        clickButton.transform.gameObject.SetActive(false);
    }
    void Start(){
        StartCoroutine(nameof(WaitForLocutor));
    }

    private IEnumerator WaitForLocutor(){
        yield return new WaitForSeconds(4);
        audioSource.clip = locutor;
        audioSource.Play();
    }

    private IEnumerator RadioClickChangeToMenu(){
        audioSource.PlayOneShot(click);
        yield return new WaitForSeconds(click.length + 0.5f);
        SceneManager.LoadScene("MainMenu");
    }

    private void LateUpdate(){
        if (!audioSource.isPlaying && audioSource.clip == locutor && !locutorHasTalked){
            audioSource.PlayOneShot(click);
            locutorHasTalked = true;
            clickButton.transform.gameObject.SetActive(true);
        }
    }

    public void OnClickPressed(){
        StartCoroutine(nameof(RadioClickChangeToMenu));
    }
}
