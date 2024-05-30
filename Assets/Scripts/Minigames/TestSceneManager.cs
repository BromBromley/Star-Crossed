using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneManager : MonoBehaviour
{
    // this is the game manager of the minigame test scene

    // TODO invoke finished event 
    // TODO add volume option

    [SerializeField] private GameObject minigameCanvas;
    [SerializeField] private GameObject[] minigames = new GameObject[4];
    [SerializeField] private GameObject[] minigameButtons = new GameObject[4];
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private GameObject finishedScreen;

    public delegate void OnFinishedTask(int index);
    public static OnFinishedTask onFinishedTask;

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
    }

    public void OpenMinigame(int index)
    {
        minigameCanvas.SetActive(true);
        foreach (GameObject minigame in minigames)
        {
            minigame.SetActive(false);
        }
        minigames[index].SetActive(true);
    }

    private void FinishedMinigame(int index)
    {
        minigameButtons[index].GetComponent<Button>().interactable = false;
        finishedScreen.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
