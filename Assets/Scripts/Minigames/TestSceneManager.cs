using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestSceneManager : MonoBehaviour
{
    // this is the game manager of the minigame test scene

    [SerializeField] private GameObject minigameCanvas;
    [SerializeField] private GameObject[] minigames = new GameObject[5];
    [SerializeField] private GameObject[] minigameButtons = new GameObject[5];
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private GameObject finishedScreen;

    public delegate void OnFinishedTask(int index);
    public static OnFinishedTask onFinishedTask;

    [SerializeField] private GameObject fadeScreen;
    private Color transparentColor;
    private float fadeTime;
    private float speed = 3f;


    void Start()
    {
        backToMenuButton.GetComponent<Button>().onClick.AddListener(GoToMenu);
        onFinishedTask += FinishedMinigame;
        finishedScreen.SetActive(false);
    }


    public void GoToMenu()
    {
        foreach (GameObject minigame in minigames)
        {
            minigame.SetActive(false);
        }
        minigameCanvas.SetActive(false);
        finishedScreen.SetActive(false);
    }

    public void OpenMinigame(int index)
    {
        minigameCanvas.SetActive(true);
        foreach (GameObject minigame in minigames)
        {
            minigame.SetActive(false);
        }
        minigames[index].SetActive(true);
        FadeToBlack();
    }


    // called whenever a minigame gets finished
    private void FinishedMinigame(int index)
    {
        minigameButtons[index].GetComponent<Button>().interactable = false;
        StartCoroutine(ScreenDelay());
    }

    private IEnumerator ScreenDelay()
    {
        yield return new WaitForSeconds(2);
        finishedScreen.SetActive(true);
    }


    // called by menu button
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // called by menu button
    public void ExitGame()
    {
        Application.Quit();
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeOutAndIn());
    }

    // fades the screen to black by changing the color of the fade screen
    private IEnumerator FadeOutAndIn()
    {
        fadeScreen.transform.SetAsLastSibling();
        /*fadeTime = 0f;

        while (fadeScreen.GetComponent<Image>().color != Color.black)
        {
            fadeTime += speed * Time.deltaTime;
            fadeScreen.GetComponent<Image>().color = Color.Lerp(transparentColor, Color.black, fadeTime);
            yield return null;
        }*/

        fadeScreen.GetComponent<Image>().color = Color.black;
        fadeTime = 0f;

        while (fadeScreen.GetComponent<Image>().color != transparentColor)
        {
            fadeTime += speed * Time.deltaTime;
            fadeScreen.GetComponent<Image>().color = Color.Lerp(Color.black, transparentColor, fadeTime);
            yield return null;
        }
        fadeScreen.transform.SetAsFirstSibling();
    }
}
