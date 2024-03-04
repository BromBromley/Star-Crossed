using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // this script manages the UI elements in the main scene of the game

    [SerializeField] private GameObject fadeScreen;
    private Color transparentColor;
    private float fadeTime;
    private float speed = 3f;

    void Start()
    {
        transparentColor = fadeScreen.GetComponent<UnityEngine.UI.Image>().color;
        PlayerInteractions.onUsingDoor += FadeToBlack;
    }

    private void FadeToBlack()
    {
        StartCoroutine(FadeOutAndIn());
    }

    // fades the screen to black by changing the color of the fade screen
    private IEnumerator FadeOutAndIn()
    {
        fadeTime = 0f;

        while (fadeScreen.GetComponent<UnityEngine.UI.Image>().color != Color.black)
        {
            fadeTime += speed * Time.deltaTime;
            fadeScreen.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(transparentColor, Color.black, fadeTime);
            yield return null;
        }

        fadeScreen.GetComponent<UnityEngine.UI.Image>().color = Color.black;
        fadeTime = 0f;

        while (fadeScreen.GetComponent<UnityEngine.UI.Image>().color != transparentColor)
        {
            fadeTime += speed * Time.deltaTime;
            fadeScreen.GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(Color.black, transparentColor, fadeTime);
            yield return null;
        }
    }
}
