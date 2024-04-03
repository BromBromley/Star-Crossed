using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // this script manages the UI elements in the main scene of the game

    [SerializeField] private GameObject fadeScreen;
    private Color transparentColor;
    private float fadeTime;
    private float speed = 3f;
    //private string taskDescription;
    string[] taskDescription = new string[6];

    [SerializeField] private GameObject textbox;
    private InteractableManager _interactableManager;

    void Start()
    {
        transparentColor = fadeScreen.GetComponent<Image>().color;
        PlayerInteractions.onUsingDoor += FadeToBlack;

        textbox.SetActive(false);
        PlayerInteractions.onInteraction += ShowTextbox;

        _interactableManager = FindObjectOfType<InteractableManager>();

        GameManager.onContinuingGame += HideTextbox;

        TaskManager.onStartingTask += ShowTaskText;

        AssignDescriptions();
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

    private void ShowTextbox(bool isTask)
    {
        textbox.SetActive(true);
        if (!isTask)
        {
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
        Debug.Log("hiding textbox");
    }

    private void ShowTaskText(int task)
    {
        textbox.GetComponentInChildren<TextMeshProUGUI>().text = "You're interacting with " + taskDescription[task] + ". Come back later to play the minigame.";
        textbox.GetComponentInChildren<Button>().onClick.AddListener(EndTask);
        textbox.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "Complete task";
    }

    private void EndTask()
    {
        TaskManager.onCompletingTask?.Invoke();
        textbox.GetComponentInChildren<Button>().onClick.RemoveListener(EndTask);
        textbox.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "Yay!";
    }
}
