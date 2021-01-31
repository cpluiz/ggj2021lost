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
    public AudioClip locutor, click, staticEffect;
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

    private IEnumerator StaticAfterLocutor(){
        audioSource.PlayOneShot(staticEffect);
        yield return new WaitForSeconds(staticEffect.length + 0.5f);
        audioSource.PlayOneShot(click);
        yield return new WaitForSeconds(click.length + 0.5f);
        clickButton.transform.gameObject.SetActive(true);
    }

    private void LateUpdate(){
        if (!audioSource.isPlaying && audioSource.clip == locutor && !locutorHasTalked){
            locutorHasTalked = true;
            StartCoroutine(nameof(StaticAfterLocutor));
        }
    }

    public void OnClickPressed(){
        StartCoroutine(nameof(RadioClickChangeToMenu));
    }
}
