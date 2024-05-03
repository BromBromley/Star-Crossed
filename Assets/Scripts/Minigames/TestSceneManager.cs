using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneManager : MonoBehaviour
{
    // this is the game manager of the minigame test scene

    [SerializeField] private GameObject minigameCanvas;

    [SerializeField] GameObject[] minigames = new GameObject[5];
    [SerializeField] private GameObject backToMenuButton;

    void Start()
    {
        backToMenuButton.GetComponent<Button>().onClick.AddListener(GoToMenu);
    }

    private void GoToMenu()
    {
        foreach (GameObject minigame in minigames)
        {
            minigame.SetActive(false);
        }
        minigameCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
