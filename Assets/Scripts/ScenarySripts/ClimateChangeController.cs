using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;


public class ClimateChangeController : MonoBehaviour{
    private Vignette vg;
    private ColorGrading cg;
    private CinemachinePostProcessing pp;
    [Range(0,1)]
    public float vignetteTargetRange;

    private static ClimateChangeController _instance;
    public static ClimateChangeController instance{ get{ return _instance; } }

    public Color gloomerColor;

    [Range(0, 0.1f)] public float intensityChange;

    public void Awake(){
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this);
        }else if (_instance != this) {
            Destroy(gameObject);
        }
    }

    void Start(){
        pp = GetComponent<CinemachinePostProcessing>();
        pp.m_Profile.TryGetSettings(out vg);
        pp.m_Profile.TryGetSettings(out cg);
        vg.intensity.value = 0;
        cg.temperature.value = 0;
        cg.colorFilter.value = Color.white;
        //ChangeSceneTemperatureToCold();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.A)){
            ChangeSceneColorToNormal();
        }
        if (Input.GetKeyDown(KeyCode.S)){
            ChangeSceneColorToGloomer();
        }
    }

    public static void VignetteToBrighter(){
        _instance.intensityChange = -Mathf.Abs(_instance.intensityChange);
    }

    public static void VignetteToGloomer(){
        _instance.intensityChange = Mathf.Abs(_instance.intensityChange);
    }

    public static void VignetteTotalGloom(){
        instance.vg.intensity.value = 1;
    }

    private IEnumerator ChangeVignetteIntensityToGloomer(){
        StopCoroutine(nameof(ChangeVignetteIntensityToBrighter));
        while (vg.intensity.value <= vignetteTargetRange){
            vg.intensity.value += intensityChange;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator ChangeVignetteIntensityToBrighter(){
        StopCoroutine(nameof(ChangeVignetteIntensityToGloomer));
        while (vg.intensity.value > 0){
            vg.intensity.value = Mathf.Clamp01(vg.intensity.value += intensityChange);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
    }

    public static void ChangeSceneTemperatureToCold(){
        _instance.cg.temperature.value = -60f;
    }

    public static void ChangeSceneTemperatureToNormal(){
        _instance.StartCoroutine(nameof(SceneTemperatureToNormal));
    }

    private IEnumerator SceneTemperatureToNormal(){
        while (_instance.cg.temperature.value > -100){
            _instance.cg.temperature.value = Mathf.Clamp(_instance.cg.temperature.value - 0.1f, -100, 0);
            yield return new WaitForEndOfFrame();
        }
        while (_instance.cg.temperature.value < 0){
            _instance.cg.temperature.value = Mathf.Clamp(_instance.cg.temperature.value + 0.2f, -100, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ChangeColorToGloomer(){
        StopCoroutine(nameof(ChangeColorToNormal));
        for (float i = 0; i < 1; i+=0.001f){
            cg.colorFilter.value = Color.Lerp(Color.white, gloomerColor, i);
            yield return new WaitForEndOfFrame();
        }
    }
    
    private IEnumerator ChangeColorToNormal(){
        StopCoroutine(nameof(ChangeColorToGloomer));
        for (float i = 0; i < 1; i+=0.01f){
            cg.colorFilter.value = Color.Lerp(gloomerColor, Color.white, i);
            yield return new WaitForEndOfFrame();
        }
    }

    public static void ChangeSceneColorToGloomer(){
        _instance.StartCoroutine(nameof(ChangeColorToGloomer));
    }

    public static void ChangeSceneColorToNormal(){
        _instance.StartCoroutine(nameof(ChangeColorToNormal));
    }
    
}
[Flags]
public enum AmbienceEffectType{
    temperatureGloom,
    temperatureNormal,
    colorGloom,
    colorNormal,
    vignetteTotalGloom,
    vignetteGloom,
    vignetteNormal,
}