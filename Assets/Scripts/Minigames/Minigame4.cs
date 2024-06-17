using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Minigame4 : MonoBehaviour
{
    // this script manages the minigame "take care of the plants"

    private bool holdingWater;
    private bool holdingNutrients;

    [SerializeField] private GameObject[] plants = new GameObject[3];
    [SerializeField] private GameObject[] plantOutlines = new GameObject[3];
    private int[] waterLevels = new int[3];
    private int[] nutrientLevels = new int[3];

    [SerializeField] private GameObject nextDayButton;

    private int dayCounter;

    // sprites with bitemarks
    [SerializeField] private Sprite[] firstPlantSprites = new Sprite[3];
    [SerializeField] private Sprite[] secondPlantSprites = new Sprite[3];
    [SerializeField] private Sprite[] thirdPlantSprites = new Sprite[3];

    [SerializeField] private GameObject leaf;

    [SerializeField] private GameObject creature;
    private Color transparentColor = new Color32(255, 255, 255, 0);

    [SerializeField] private GameObject[] waterLevelHUD = new GameObject[3];
    [SerializeField] private GameObject[] nutrientLevelHUD = new GameObject[3];

    // line for showing active tool
    [SerializeField] private GameObject line;
    private Vector3 pointerPosition;
    private Vector3 buttonPosition;


    void Start()
    {
        nextDayButton.SetActive(false);
        line.SetActive(false);
        leaf.SetActive(false);

        waterLevels[0] = 2;
        waterLevels[1] = 3;
        waterLevels[2] = 3;
        nutrientLevels[0] = 4;
        nutrientLevels[1] = 3;
        nutrientLevels[2] = 5;

        creature.GetComponent<Image>().color = transparentColor;
        creature.SetActive(false);

        TurnOffPlants();
        UpdateHUD();
    }

    void Update()
    {
        if (holdingWater || holdingNutrients)
        {
            pointerPosition = Input.mousePosition;
            line.transform.position = (buttonPosition + pointerPosition) / 2f;
            Vector3 direction = buttonPosition - pointerPosition;
            line.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            line.transform.localScale = new Vector3(direction.magnitude / 50, 0.15f, 1f);
        }
    }


    // gets replaced by GameManager later
    public void TestDays()
    {
        dayCounter++;
        SetUpDay();
    }


    // this manages the drains and sets up the plants and spooky effects
    private void SetUpDay()
    {
        nextDayButton.SetActive(false);
        if (dayCounter < 3)
        {
            Drain01();
        }
        else
        {
            Drain02();
        }
        if (dayCounter == 4)
        {
            plants[0].GetComponent<Image>().sprite = firstPlantSprites[0];
            plants[1].GetComponent<Image>().sprite = secondPlantSprites[0];
            plants[2].GetComponent<Image>().sprite = thirdPlantSprites[0];
            leaf.SetActive(true);
        }
        if (dayCounter == 6)
        {
            StartCoroutine(FadeInCreature());
            plants[0].GetComponent<Image>().sprite = firstPlantSprites[1];
            plants[1].GetComponent<Image>().sprite = secondPlantSprites[1];
            plants[2].GetComponent<Image>().sprite = thirdPlantSprites[1];
        }
        if (dayCounter == 7)
        {
            creature.SetActive(false);
            plants[0].GetComponent<Image>().sprite = firstPlantSprites[2];
            plants[1].GetComponent<Image>().sprite = secondPlantSprites[2];
            plants[2].GetComponent<Image>().sprite = thirdPlantSprites[2];
        }
        UpdateHUD();
    }


    // this checks which tool has been chosen and triggers the plants
    public void ClickedWater(GameObject button)
    {
        AssignStartingPoint(button);
        holdingWater = !holdingWater;
        holdingNutrients = false;
        if (holdingWater)
        {
            TurnOnPlants();
            line.SetActive(true);
        }
        else
        {
            TurnOffPlants();
            line.SetActive(false);
        }
    }

    public void ClickedNutrients(GameObject button)
    {
        AssignStartingPoint(button);
        holdingNutrients = !holdingNutrients;
        holdingWater = false;
        if (holdingNutrients)
        {
            TurnOnPlants();
            line.SetActive(true);
        }
        else
        {
            TurnOffPlants();
            line.SetActive(false);
        }
    }

    // this assigns the right button as starting point for the line
    private void AssignStartingPoint(GameObject button)
    {
        buttonPosition = button.transform.position;
    }


    // this checks if the levels of the plants are filled already to see if they should be interactable or not
    private void TurnOnPlants()
    {
        for (int i = 0; i < 3; i++)
        {
            if (holdingWater && waterLevels[i] < 5)
            {
                plants[i].GetComponent<Button>().interactable = true;
                plantOutlines[i].SetActive(true);
            }
            if (holdingNutrients && nutrientLevels[i] < 5)
            {
                plants[i].GetComponent<Button>().interactable = true;
                plantOutlines[i].SetActive(true);
            }
        }
    }

    private void TurnOffPlants()
    {
        foreach (GameObject plant in plants)
        {
            plant.GetComponent<Button>().interactable = false;
        }
        for (int i = 0; i < 3; i++)
        {
            plantOutlines[i].SetActive(false);
        }
    }


    // this refills whatever stat and plant has been clicked
    // called by the button components of each plant
    public void GetRefilled(int index)
    {
        line.SetActive(false);
        TurnOffPlants();
        if (holdingWater)
        {
            if (waterLevels[index] < 5)
            {
                waterLevels[index]++;
                holdingWater = false;
            }
        }
        else
        {
            if (nutrientLevels[index] < 5)
            {
                nutrientLevels[index]++;
                holdingNutrients = false;
            }
        }
        UpdateHUD();
        CheckIfDone();
    }

    // updates the values of the bars for water and nutrients
    private void UpdateHUD()
    {
        for (int i = 0; i < 3; i++)
        {
            waterLevelHUD[i].GetComponent<Slider>().value = waterLevels[i];
            nutrientLevelHUD[i].GetComponent<Slider>().value = nutrientLevels[i];
        }
    }


    // this reduces the water and nutrient levels of the plants
    // the only difference is the amount they get reduced
    private void Drain01()
    {
        for (int i = 0; i < waterLevels.Length; i++)
        {
            int j = Random.Range(0, 2);
            waterLevels[i] -= j;
            if (waterLevels[i] < 0)
            {
                waterLevels[i] = 0;
            }
            if (waterLevels[i] > 5)
            {
                waterLevels[i] = 5;
            }
        }
        for (int i = 0; i < nutrientLevels.Length; i++)
        {
            int j = Random.Range(0, 2);
            nutrientLevels[i] -= j;
            if (nutrientLevels[i] < 0)
            {
                nutrientLevels[i] = 0;
            }
            if (nutrientLevels[i] > 5)
            {
                nutrientLevels[i] = 5;
            }
        }
    }

    private void Drain02()
    {
        for (int i = 0; i < waterLevels.Length; i++)
        {
            int j = Random.Range(1, 4);
            waterLevels[i] -= j;
            if (waterLevels[i] < 0)
            {
                waterLevels[i] = 0;
            }
            if (waterLevels[i] > 5)
            {
                waterLevels[i] = 5;
            }
        }
        for (int i = 0; i < nutrientLevels.Length; i++)
        {
            int j = Random.Range(1, 4);
            nutrientLevels[i] -= j;
            if (nutrientLevels[i] < 0)
            {
                nutrientLevels[i] = 0;
            }
            if (nutrientLevels[i] > 5)
            {
                nutrientLevels[i] = 5;
            }
        }
    }

    // this checks the sum of the water and nutrient levels 
    private void CheckIfDone()
    {
        int sumWater = 0;
        foreach (int water in waterLevels)
        {
            sumWater += water;
        }

        int sumNutrients = 0;
        foreach (int nutrients in nutrientLevels)
        {
            sumNutrients += nutrients;
        }

        if (sumWater == 15 && sumNutrients == 15)
        {
            Debug.Log("You did it!");
            if (dayCounter == 7)
            {
                TestSceneManager.onFinishedTask?.Invoke(3);
            }
            else
            {
                nextDayButton.SetActive(true);
            }
        }
    }

    // this fades the creature at the window in and out
    private IEnumerator FadeInCreature()
    {
        yield return new WaitForSeconds(4);
        TestAudioManager.onGlitch?.Invoke(false);
        creature.SetActive(true);
        float fadeTime = 0f;
        float speed = 0.3f;
        while (creature.GetComponent<Image>().color != Color.black)
        {
            fadeTime += speed * Time.deltaTime;
            creature.GetComponent<Image>().color = Color.Lerp(transparentColor, Color.black, fadeTime);
            yield return null;
        }
        yield return new WaitForSeconds(5);
        fadeTime = 0f;
        while (creature.GetComponent<Image>().color != transparentColor)
        {
            fadeTime += speed * Time.deltaTime;
            creature.GetComponent<Image>().color = Color.Lerp(Color.black, transparentColor, fadeTime);
            yield return null;
        }
    }

}
