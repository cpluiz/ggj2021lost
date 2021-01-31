using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioAmbienceController : MonoBehaviour{

    private static AudioAmbienceController _instance;
    public static AudioAmbienceController instance { get => _instance;}
    
    [Header("Ambience Effects")]
    public AudioClip[] waterDroplets;
    [Header("Main Character Effects")]
    [SerializeField] private AudioClip[] solidSurfaceStep;
    [SerializeField] private AudioClip[] solidSurfaceStepEchoed;
    [SerializeField] private AudioClip[] mudSurfaceStep;
    [SerializeField] private AudioClip[] puddleSurfaceStep;

    [Header("Character voices variants")] [SerializeField]
    private AudioClip[] playerVoice;
    [SerializeField] private AudioClip[] radioVoice, grandmaVoice, lumenVoice, armlessVoice, dogVoice;
    
    [Header("Audio Source Reference")]
    [SerializeField] private AudioSource ambienceAudio;

    private bool radioIsOn = false;

    public void Awake(){
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }else if (_instance != this) {
            Destroy(gameObject);
        }
    }

    public static void PlayDropletAudio(){
        _instance.ambienceAudio.PlayOneShot(_instance.waterDroplets[Random.Range(0,_instance.waterDroplets.Length-1)]);
    }

    void Start(){
        ambienceAudio = GetComponent<AudioSource>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (SceneManager.GetActiveScene().name != "MainMenu"){
            StartCoroutine(nameof(RandomDropletSounds), Random.Range(2, 4.2f));
        }
        else{
            StopCoroutine(nameof(RandomDropletSounds));
        }
    }

    private IEnumerator RandomDropletSounds(float seconds){
        yield return new WaitForSeconds(seconds);
        PlayDropletAudio();
        StartCoroutine(nameof(RandomDropletSounds), Random.Range(2, 4.2f));
    }
    
    

    public static void PlayStepAudio(LayerMask layerMask){
        if (layerMask == LayerMask.NameToLayer("RockFloor")){
            if(_instance.radioIsOn)
                _instance.ambienceAudio.PlayOneShot(_instance.solidSurfaceStep[Random.Range(0,_instance.solidSurfaceStep.Length)]);
            else
                _instance.ambienceAudio.PlayOneShot(_instance.solidSurfaceStepEchoed[Random.Range(0,_instance.solidSurfaceStepEchoed.Length)]);
        }else if (layerMask == LayerMask.NameToLayer("MudFloor")){
            _instance.ambienceAudio.PlayOneShot(_instance.mudSurfaceStep[Random.Range(0,_instance.mudSurfaceStep.Length)]);
        }else if (layerMask == LayerMask.NameToLayer("PuddleFloor")){
            _instance.ambienceAudio.PlayOneShot(_instance.puddleSurfaceStep[Random.Range(0,_instance.puddleSurfaceStep.Length)]);
        }
    }

    public static void PlayCharacterVoice(string characterId){
        if (characterId == "radio")
            _instance.ambienceAudio.PlayOneShot(_instance.radioVoice[Random.Range(0,_instance.radioVoice.Length)]);
        else if (characterId == "grandma")
            _instance.ambienceAudio.PlayOneShot(_instance.grandmaVoice[Random.Range(0,_instance.grandmaVoice.Length)]);
        else if (characterId == "lumen")
            _instance.ambienceAudio.PlayOneShot(_instance.lumenVoice[Random.Range(0,_instance.lumenVoice.Length)]);
        else if (characterId == "armless")
            _instance.ambienceAudio.PlayOneShot(_instance.armlessVoice[Random.Range(0,_instance.armlessVoice.Length)]);
        else
            _instance.ambienceAudio.PlayOneShot(_instance.playerVoice[Random.Range(0,_instance.playerVoice.Length)]);
    }

    public static void StopCharacterVoice(){
        _instance.ambienceAudio.Stop();
    }
}
