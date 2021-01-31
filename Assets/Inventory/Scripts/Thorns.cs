using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Thorns : MonoBehaviour
{
    public SpriteRenderer thornsImage;

    bool soaked = false;
    public bool IsSoaked { get => soaked; }


    void Start()
    {
        if(thornsImage == null)
        {
            TryGetComponent<SpriteRenderer>(out thornsImage);
        }
    }

    public void CallDestroyThorns(int fadeTime = 5)
    {
        StartCoroutine(DestroyThorns(fadeTime));
    }
    public void CallSoakThree()
    {
        soaked = true;
    }
    public void CallLightThorn()
    {
        if (soaked)
            CallDestroyThorns();
    }

    IEnumerator DestroyThorns(int duration)
    {
        var tempColor = thornsImage.color;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;

            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            tempColor.a = Mathf.Lerp(thornsImage.color.a, 0, normalizedTime);
            thornsImage.color = tempColor;

            yield return null;
        }
        tempColor.a = 0;
        thornsImage.color = tempColor; //without this, the value will end at something like 0.9992367

        Destroy(gameObject);
    }
}
