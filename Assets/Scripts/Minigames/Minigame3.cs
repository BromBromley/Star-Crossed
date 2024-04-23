using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame3 : MonoBehaviour
{
    // this script manages the minigame "checking the ship's system"

    private int correctCounter;
    private static List<GameObject> greenButtons = new List<GameObject>();

    // variables for the first part
    [SerializeField] private GameObject curve;
    private float stretchFactor = 0.1f;
    [SerializeField] private Button squashButton;
    [SerializeField] private Button stretchButton;
    [SerializeField] private GameObject curve_ref;

    // variables for the second part
    [SerializeField] private GameObject[] squares = new GameObject[5];
    private int amount;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private GameObject[] squares_ref = new GameObject[5];
    private int amount_ref;

    // variables for the third part
    [SerializeField] private Slider[] slider = new Slider[4];
    [SerializeField] private GameObject[] buttons = new GameObject[4];
    public bool moveBar;
    private float speed = 3f;
    private float sliderTime;
    private int sliderIndex;
    private bool showingStop;
    [SerializeField] private Slider[] slider_ref = new Slider[4];


    void Start()
    {
        TurnOffSquares();
        // add all the green images overlaying the buttons
    }

    void Update()
    {
        if (moveBar)
        {
            slider[sliderIndex].value = Mathf.PingPong(sliderTime, 5);
            sliderTime += Time.deltaTime * speed;
            // Time.time continues, which causes the skipping

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


    // first part 
    public void SquashCurve()
    {
        if (curve.transform.localScale.x > 0.3)
        {
            curve.transform.localScale = new Vector3((curve.transform.localScale.x - stretchFactor), curve.transform.localScale.y, curve.transform.localScale.z);
        }
    }
    public void StretchCurve()
    {
        if (curve.transform.localScale.x < 1.8)
        {
            curve.transform.localScale = new Vector3((curve.transform.localScale.x + stretchFactor), curve.transform.localScale.y, curve.transform.localScale.z);
        }
    }

    public void CheckIfCurveCorrect(GameObject button)
    {
        if (Mathf.Abs(curve.transform.localScale.x - curve_ref.transform.localScale.x) < 0.2)
        {
            button.transform.GetChild(0).gameObject.SetActive(true);
            greenButtons.Add(button.transform.GetChild(0).gameObject);
            button.GetComponent<Button>().interactable = false;
            SwitchInteractability(squashButton);
            SwitchInteractability(stretchButton);

            correctCounter++;
            CheckIfTaskComplete();
        }
    }


    // second part
    public void ChooseButtons(bool turnOn)
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
            CheckIfSlidersCorrect(i);
        }
        moveBar = !moveBar;
        sliderIndex = i;
    }

    // changes the sprite from play to stop and back when the bar moves
    private void ChangeSprite()
    {
        if (buttons[sliderIndex].transform.GetChild(0).gameObject.activeSelf)
        {
            buttons[sliderIndex].transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            buttons[sliderIndex].transform.GetChild(0).gameObject.SetActive(true);
        }
        if (buttons[sliderIndex].transform.GetChild(1).gameObject.activeSelf)
        {
            buttons[sliderIndex].transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            buttons[sliderIndex].transform.GetChild(1).gameObject.SetActive(true);
        }
        showingStop = !showingStop;
    }

    // automatically checks if the bar levels are close enough
    public void CheckIfSlidersCorrect(int i)
    {
        if (Mathf.Abs(slider[i].value - slider_ref[i].value) < 0.5)
        {
            buttons[i].transform.GetChild(2).gameObject.SetActive(true);
            greenButtons.Add(buttons[i].transform.GetChild(2).gameObject);
            buttons[i].GetComponent<Button>().interactable = false;
            correctCounter++;
            CheckIfTaskComplete();
        }
    }


    //this checks if the player has finished the task
    private void CheckIfTaskComplete()
    {
        if (correctCounter == 6)
        {
            Debug.Log("You did it!");
            correctCounter = 0;
        } 
    }

    // randomize the values for each part
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

        ResetButtons();
    }

    // resets all the buttons and turns off the green covers
    private void ResetButtons()
    {
        foreach (GameObject button in greenButtons)
        {
            button.SetActive(false);
        }
        greenButtons.Clear();

        SwitchInteractability(squashButton);
        SwitchInteractability(stretchButton);
        SwitchInteractability(minusButton);
        SwitchInteractability(plusButton);

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }


    private void SwitchInteractability(Button button)
    {
        button.interactable = !button.interactable;
    }
}
