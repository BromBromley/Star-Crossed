using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // this script manages the main menu elements and their functionality

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject aboutScreen;

    void Start()
    {
        mainScreen.SetActive(true);
        optionsScreen.SetActive(false);
        aboutScreen.SetActive(false);
    }

    public void ShowScreen(GameObject screen)
    {
        mainScreen.SetActive(false);
        screen.SetActive(true);
    }

    public void ShowMainMenu(GameObject screen)
    {
        screen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
