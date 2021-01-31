using System;
using System.Collections;
using System.Collections.Generic;
using ScenarySripts;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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

    [SerializeField] private GameScriptSequence startSequence;
    private GameScriptSequence currentSequence;
    private bool insideSequence = false;
    private int sequenceStep = 0;
    private AudioSource audioSource;
    public static bool isInsideSequence{ get => instance.insideSequence; }

    void Start(){
        audioSource = GetComponent<AudioSource>();
        PlaySequence(startSequence);
    }
    
    public static void PlaySequence(GameScriptSequence sequence){
        instance.currentSequence = sequence;
        instance.insideSequence = true;
        instance.sequenceStep = 0;
        instance.StartCoroutine(nameof(PlayCurrentSequence));
    }

    private IEnumerator PlayCurrentSequence(){
        while (sequenceStep < currentSequence.segment.Length){
            if (currentSequence.segment[sequenceStep].type == SegmentType.audio){
                audioSource.PlayOneShot(currentSequence.segment[sequenceStep].audio);
                yield return new WaitForSeconds(currentSequence.segment[sequenceStep].audio.length);
            }else if (currentSequence.segment[sequenceStep].type == SegmentType.dialog){
                string target = TextController.GetTargetType(currentSequence.segment[sequenceStep].textId);
                Speaker.WriteTextById(currentSequence.segment[sequenceStep].textId);
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                while (!Speaker.completed){
                    yield return new WaitForEndOfFrame();
                } 
            }
            sequenceStep++;
        }
    }
    
}
