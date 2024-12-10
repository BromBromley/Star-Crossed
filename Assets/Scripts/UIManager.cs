using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // this script manages the UI elements in the main scene of the game

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject fadeScreen;
    private Color transparentColor;
    private float fadeTime;
    private float speed = 3f;
    string[] taskDescription = new string[6];

    [SerializeField] private GameObject textbox;
    [SerializeField] private GameObject dayCounter;
    [SerializeField] private GameObject taskButton;
    private InteractableManager _interactableManager;
    private APManager _apManager;



    void Start()
    {
        transparentColor = fadeScreen.GetComponent<Image>().color;
        PlayerInteractions.onUsingDoor += FadeToBlack;

        textbox.SetActive(false);
        PlayerInteractions.onInteraction += ShowTextbox;

        taskButton.GetComponentInChildren<Button>().onClick.AddListener(EndTask);
        taskButton.SetActive(false);

        _interactableManager = FindObjectOfType<InteractableManager>();
        _apManager = FindObjectOfType<APManager>();

        GameManager.onPausingGame += ShowPauseScreen;
        GameManager.onContinuingGame += HidePauseScreen;
        GameManager.onContinuingGame += HideTextbox;
        GameManager.onNewDay += UpdateDay;

        TaskManager.onStartingTask += ShowTaskText;

        AssignDescriptions();
    }


    void OnDisable()
    {
        GameManager.onPausingGame -= ShowPauseScreen;
        GameManager.onContinuingGame -= HidePauseScreen;
        GameManager.onContinuingGame -= HideTextbox;
        GameManager.onNewDay -= UpdateDay;

        TaskManager.onStartingTask -= ShowTaskText;
    }



    private void AssignDescriptions()
    {
        taskDescription[0] = "airlock";
        taskDescription[1] = "spacesuit";
        taskDescription[2] = "helm";
        taskDescription[3] = "console";
        taskDescription[4] = "kitchen";
        taskDescription[5] = "plant";
    }



    private void FadeToBlack()
    {
        StartCoroutine(FadeOutAndIn());
    }


    // fades the screen to black by changing the color of the fade screen
    private IEnumerator FadeOutAndIn()
    {
        fadeScreen.transform.SetAsLastSibling();
        fadeTime = 0f;

        while (fadeScreen.GetComponent<Image>().color != Color.black)
        {
            fadeTime += speed * Time.deltaTime;
            fadeScreen.GetComponent<Image>().color = Color.Lerp(transparentColor, Color.black, fadeTime);
            yield return null;
        }

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



    private void ShowPauseScreen()
    {
        pauseScreen.SetActive(true);
    }


    private void HidePauseScreen()
    {
        pauseScreen.SetActive(false);
    }



    private void ShowTextbox(bool isTask)
    {
        textbox.SetActive(true);
        if (!isTask)
        {
            taskButton.SetActive(false);
            textbox.GetComponentInChildren<TextMeshProUGUI>().text = "You're interacting with " + _interactableManager.taskName;
        }
        else
        {
            // confirmation button to start the task
        }
    }


    private void HideTextbox()
    {
        textbox.SetActive(false);
    }


    private void ShowTaskText(int task)
    {
        textbox.GetComponentInChildren<TextMeshProUGUI>().text = "You're interacting with " + taskDescription[task] + ". Come back later to play the minigame.";
        taskButton.SetActive(true);
    }


    private void EndTask()
    {
        taskButton.SetActive(false);
        if (_apManager.AP > 0)
        {
            TaskManager.onCompletingTask?.Invoke();
            textbox.GetComponentInChildren<TextMeshProUGUI>().text = "You've completed the task. Yippie!";
        }
        else
        {
            textbox.GetComponentInChildren<TextMeshProUGUI>().text = "You don't have enough AP for that right now. Go to bed.";
        }
    }



    // this updates the HUD's day counter
    private void UpdateDay(int day)
    {
        dayCounter.GetComponent<TextMeshProUGUI>().text = "Day 0" + (day + 1);
    }
}
