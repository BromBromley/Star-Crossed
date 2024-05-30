using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame3 : MonoBehaviour
{
    // this script manages the minigame "checking the ship's system"

    private int levelCounter;
    private int correctCounter;
    private static List<GameObject> greenButtons = new List<GameObject>();
    [SerializeField] private GameObject blackReferenceScreen;
    private bool isCheckNormal = true;
    private bool computerReacts = true;
    [SerializeField] private GameObject nextLevelButton;

    // variables for the first part
    [SerializeField] private GameObject curve;
    private float stretchFactor = 0.1f;
    [SerializeField] private Button squashButton;
    [SerializeField] private Button stretchButton;
    [SerializeField] private GameObject curve_ref;
    [SerializeField] private Button checkButton01;

    // variables for the second part
    [SerializeField] private GameObject[] squares = new GameObject[5];
    private int amount = 2;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private GameObject[] squares_ref = new GameObject[5];
    private int amount_ref;
    [SerializeField] private Button checkButton02;

    // variables for the third part
    [SerializeField] private Slider[] slider = new Slider[4];
    [SerializeField] private GameObject[] sliderButtons = new GameObject[4];
    public bool moveBar;
    private float speed = 4f;
    private float sliderTime;
    private int sliderIndex;
    private bool showingStop;
    [SerializeField] private Slider[] slider_ref = new Slider[4];

    // variables for resetting the console
    private float prevStretchFactor;
    private int prevAmount;
    private float[] prevSliderValues = new float[4];


    void Start()
    {
        blackReferenceScreen.SetActive(false);
        nextLevelButton.SetActive(false);
        //TurnOffSquares();
    }

    void Update()
    {
        if (moveBar)
        {
            slider[sliderIndex].value = Mathf.PingPong(sliderTime, 5);
            sliderTime += Time.deltaTime * speed;

            if (!showingStop)
            {
                ChangeSprite();
            }
        }
        else if (!moveBar && showingStop)
        {
            ChangeSprite();
        }
    }

    // this is for testing the different levels 
    // should get replaced by day counter
    public void LevelTest()
    {
        levelCounter++;
        SetUpDay();
    }

    // this sets up the values and triggers the behaviors for each day
    private void SetUpDay()
    {
        nextLevelButton.SetActive(false);
        SaveValues();
        RandomizeValues();

        // level 0 is the one at Start
        if (levelCounter == 1)
        {
            isCheckNormal = false;
            StartCoroutine(BlackScreen());
        }
        if (levelCounter == 2)
        {
            isCheckNormal = true;
            StartCoroutine(TimerReset());
        }
        if (levelCounter == 3)
        {
            StopAllCoroutines();
            computerReacts = true;
            StartCoroutine(BlackScreen());
        }
    }


    // first part 
    public void SquashCurve()
    {
        ComputerReaction();
        if (curve.transform.localScale.x > 0.3 && computerReacts)
        {
            curve.transform.localScale = new Vector3((curve.transform.localScale.x - stretchFactor), curve.transform.localScale.y, curve.transform.localScale.z);
        }
    }
    public void StretchCurve()
    {
        ComputerReaction();
        if (curve.transform.localScale.x < 1.8 && computerReacts)
        {
            curve.transform.localScale = new Vector3((curve.transform.localScale.x + stretchFactor), curve.transform.localScale.y, curve.transform.localScale.z);
        }
    }

    public void CheckIfCurveCorrect(GameObject button)
    {
        ComputerReaction();
        if (computerReacts)
        {
            if (Mathf.Abs(curve.transform.localScale.x - curve_ref.transform.localScale.x) < 0.2)
            {
                button.transform.GetChild(0).gameObject.SetActive(true);
                greenButtons.Add(button.transform.GetChild(0).gameObject);
                button.GetComponent<Button>().interactable = false; // needs to be reset too
                SwitchInteractability(squashButton);
                SwitchInteractability(stretchButton);
                correctCounter++;
                CheckIfTaskComplete();
            }
        }
    }


    // second part
    public void ChooseButtons(bool turnOn)
    {
        ComputerReaction();
        if (computerReacts)
        {
            if (turnOn && amount < 5)
            {
                squares[amount].SetActive(true);
                amount++;
            }
            else if (!turnOn && amount > 0)
            {
                amount--;
                squares[amount].SetActive(false);
            }
        }
    }

    private void TurnOffSquares()
    {
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].SetActive(false);
        }
    }

    public void CheckIfSquaresCorrect(GameObject button)
    {
        amount_ref = 0;
        foreach (GameObject square in squares_ref)
        {
            if (square.gameObject.activeSelf)
            {
                amount_ref++;
            }
        }

        if (amount_ref == amount)
        {
            button.transform.GetChild(0).gameObject.SetActive(true);
            greenButtons.Add(button.transform.GetChild(0).gameObject);
            button.GetComponent<Button>().interactable = false;
            SwitchInteractability(minusButton);
            SwitchInteractability(plusButton);
            correctCounter++;
            CheckIfTaskComplete();
        }
    }


    // third part
    public void CheckWhichSlider(int i)
    {
        if (moveBar)
        {
            CheckIfSliderIsCorrect(i);
        }
        moveBar = !moveBar;
        sliderIndex = i;
        sliderTime = slider[sliderIndex].value;
    }

    // changes the sprite from play to stop and back when the bar moves
    private void ChangeSprite()
    {
        if (sliderButtons[sliderIndex].transform.GetChild(0).gameObject.activeSelf)
        {
            sliderButtons[sliderIndex].transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            sliderButtons[sliderIndex].transform.GetChild(0).gameObject.SetActive(true);
        }
        if (sliderButtons[sliderIndex].transform.GetChild(1).gameObject.activeSelf)
        {
            sliderButtons[sliderIndex].transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            sliderButtons[sliderIndex].transform.GetChild(1).gameObject.SetActive(true);
        }
        showingStop = !showingStop;
    }

    // automatically checks if the bar levels are close enough
    public void CheckIfSliderIsCorrect(int i)
    {
        if (Mathf.Abs(slider[i].value - slider_ref[i].value) < 0.5 && computerReacts)
        {
            sliderButtons[i].transform.GetChild(2).gameObject.SetActive(true);
            greenButtons.Add(sliderButtons[i].transform.GetChild(2).gameObject);
            sliderButtons[i].GetComponent<Button>().interactable = false;
            correctCounter++;
            CheckIfTaskComplete();
        }
    }


    // this randomizes if the computer responds on certain days
    private void ComputerReaction()
    {
        if (!isCheckNormal)
        {
            int i = Random.Range(0, 2);
            if (i > 1)
            {
                computerReacts = true;
            }
        }
    }


    //this checks if the player has finished the task
    private void CheckIfTaskComplete()
    {
        if (correctCounter == 6)
        {
            if (levelCounter < 3)
            {
                Debug.Log("Onto the next one!");
            }
            else
            {
                Debug.Log("You did it!");
                TestSceneManager.onFinishedTask?.Invoke(2);
            }
            correctCounter = 0;
            nextLevelButton.SetActive(true);
            StopCoroutine(TimerReset());
        }
    }


    // randomize the values of the reference for each part
    public void RandomizeValues()
    {
        // first part
        float referenceScale = Random.Range(0.3f, 1.8f);
        curve_ref.transform.localScale = new Vector3(referenceScale, curve.transform.localScale.y, curve.transform.localScale.z);

        // second part
        int referenceAmount = Random.Range(0, 6);
        foreach (GameObject square in squares_ref)
        {
            square.SetActive(false);
        }
        for (int i = 0; i <= referenceAmount; i++)
        {
            if (i != 0)
            {
                squares_ref[(i - 1)].SetActive(true);
            }
        }

        // third part
        foreach (Slider slider in slider_ref)
        {
            float referenceValue = Random.Range(0f, 5f);
            slider.value = referenceValue;
        }

        correctCounter = 0;
        ResetButtons();
    }

    // resets all the sliderButtons and turns off the green covers
    private void ResetButtons()
    {
        foreach (GameObject button in greenButtons)
        {
            button.SetActive(false);
        }
        greenButtons.Clear();

        squashButton.interactable = true;
        stretchButton.interactable = true;
        checkButton01.interactable = true;
        minusButton.interactable = true;
        plusButton.interactable = true;
        checkButton02.interactable = true;

        foreach (GameObject button in sliderButtons)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }

    // these are needed for resetting the values on the console
    private void SaveValues()
    {
        prevStretchFactor = curve.transform.localScale.x;
        prevAmount = amount;
        for (int i = 0; i < 4; i++)
        {
            prevSliderValues[i] = slider[i].value;
        }
    }
    private void ResetConsole()
    {
        curve.transform.localScale = new Vector3(prevStretchFactor, curve.transform.localScale.y, curve.transform.localScale.z);

        foreach (GameObject square in squares)
        {
            square.SetActive(false);
        }
        for (int i = 0; i <= prevAmount; i++)
        {
            if (i != 0)
            {
                squares[(i - 1)].SetActive(true);
            }
        }
        amount = prevAmount;

        for (int i = 0; i < 4; i++)
        {
            slider[i].value = prevSliderValues[i];
        }
    }


    private IEnumerator BlackScreen()
    {
        int i = Random.Range(5, 10);
        yield return new WaitForSeconds(i);
        TestAudioManager.onGlitch?.Invoke(true);
        blackReferenceScreen.SetActive(true);
        ResetConsole();
        if (levelCounter == 1)
        {
            yield return new WaitForSeconds(1);
            blackReferenceScreen.SetActive(false);
            isCheckNormal = true;
        }
        RandomizeValues();
    }

    private IEnumerator TimerReset()
    {
        yield return new WaitForSeconds(25);
        TestAudioManager.onGlitch?.Invoke(true);
        ResetConsole();
        RandomizeValues();
        if (levelCounter == 2)
        {
            StartCoroutine(TimerReset());
        }
    }


    private void SwitchInteractability(Button button)
    {
        button.interactable = !button.interactable;
    }

}
