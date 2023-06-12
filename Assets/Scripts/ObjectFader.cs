using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{

    [SerializeField] private float fadeSpeed = 10;
    [SerializeField] private float fadeAmount = 0.2f;

    private float originalOpacity;
    private Material material;

    [SerializeField] private bool doFade;


    public void SetDoFade(bool isDoFade) {
        doFade = isDoFade;
    }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        originalOpacity = material.color.a;
    }
    private void Update()
    {
        if (doFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }

    private void FadeNow()
    {
        Color currentColor = material.color;
        // slowly changes the alpha
        float alpha = Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime); // The transperancy of the object (between 0-1)
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        material.color = smoothColor;

    }

    private void ResetFade()
    {
        Color currentColor = material.color;
        float alpha = Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime);
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        material.color = smoothColor;
    }
}
