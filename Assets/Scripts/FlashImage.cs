using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashImage : MonoBehaviour
{
    Image image;
    Coroutine coroutine;
    private void Awake()
    {
        image = GetComponent<Image>(); 
    }

    public void FlashIn(float seconds, float maxAlpha, Color newColor)
    {
        image.color = newColor;
        maxAlpha = Mathf.Clamp(maxAlpha, 0f, 1f);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FlashP1(seconds, maxAlpha));
    }
    public void FlashOut(float seconds, float maxAlpha, Color newColor)
    {
        image.color = newColor;
        maxAlpha = Mathf.Clamp(maxAlpha, 0f, 1f);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FlashP2(seconds, maxAlpha));
    }

    public void StartFlash(float seconds, float maxAlpha, Color newColor)
    {
        image.color = newColor;
        maxAlpha = Mathf.Clamp(maxAlpha, 0f, 1f);

        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(Flash(seconds, maxAlpha));
    }

    IEnumerator Flash(float seconds, float maxAlpha)
    {
        float flashInDuration = seconds / 2;
        for (float t = 0; t <= flashInDuration; t+= Time.deltaTime)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(0, maxAlpha, t / flashInDuration);
            image.color = tempColor;
            yield return null;
        }

        for (float t = 0; t <= flashInDuration; t += Time.deltaTime)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(maxAlpha, 0, t / flashInDuration);
            image.color = tempColor;
            yield return null;
        }
        image.color = new Color32(0, 0, 0, 0);

    }

    IEnumerator FlashP1(float seconds, float maxAlpha)
    {
        for (float t = 0; t <= seconds; t += Time.deltaTime)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(0, maxAlpha, t / seconds);
            image.color = tempColor;
            yield return null;
        }
    }
    IEnumerator FlashP2(float seconds, float maxAlpha)
    {
        for (float t = 0; t <= seconds; t += Time.deltaTime)
        {
            Color tempColor = image.color;
            tempColor.a = Mathf.Lerp(maxAlpha, 0, t / seconds);
            image.color = tempColor;
            yield return null;
        }
        image.color = new Color32(0, 0, 0, 0);
    }
}
