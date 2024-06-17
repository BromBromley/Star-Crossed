using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Minigame2 : MonoBehaviour
{
    // this script manages the minigame "set the ship's course"

    [SerializeField] private GameObject ship;
    private bool shipFacingRight;
    private bool shipFacingUp;

    [SerializeField] private GameObject bigShip;
    [SerializeField] private GameObject blackShip;
    private Vector3 priorShipPosition;
    private Quaternion priorShipRotation;

    [SerializeField] private GameObject display;
    [SerializeField] private GameObject blackDisplay;

    private float distanceShipCell;
    private float distanceShipClosest;
    private Vector3 closestCell;

    [SerializeField] private GameObject movementCounter;
    private int stepCounter = 2;
    private bool canMove = true;
    private bool cellOccupied;

    [SerializeField] private GameObject moon01;
    [SerializeField] private GameObject moon02;
    [SerializeField] private GameObject anomaly01;
    [SerializeField] private GameObject anomaly02;
    [SerializeField] private GameObject anomaly03;

    private int dayCounter;
    [SerializeField] private GameObject dayButton;

    void Start()
    {
        blackDisplay.SetActive(false);
        dayButton.SetActive(false); // remove later

        DaySetup(0);
    }

    // this is for testing purposes and is called by a button on screen
    // will be replaced by the event system in the game
    public void DaysTest()
    {
        dayCounter++;
        DaySetup(dayCounter);
    }

    // this sets up the different events and objects that change over time
    private void DaySetup(int day)
    {
        stepCounter = 2;
        canMove = true;
        UpdateCounter();

        // days start counting at 0
        if (day == 0)
        {
            moon01.SetActive(false);
            //moon02.transform.position = display.transform.Find("cell_F_09").transform.position; -> buggy
            anomaly01.SetActive(true);
            anomaly02.SetActive(false);
            anomaly03.SetActive(false);

            bigShip.SetActive(false);
            blackShip.SetActive(false);
        }
        if (day == 1)
        {
            moon01.SetActive(true);
            moon01.transform.position = display.transform.Find("cell_C_01").transform.position;
            moon02.transform.position = display.transform.Find("cell_F_08").transform.position;
        }
        if (day == 2)
        {
            moon01.transform.position = display.transform.Find("cell_D_02").transform.position;
            moon02.transform.position = display.transform.Find("cell_F_07").transform.position;
            anomaly01.SetActive(false);
            anomaly02.SetActive(true);

            blackShip.SetActive(true);
            // this to be one step of the ship earlier
            priorShipPosition = ship.transform.position;
            priorShipRotation = ship.transform.rotation;
            blackShip.transform.position = priorShipPosition;
            blackShip.transform.rotation = priorShipRotation;
        }
        if (day == 3)
        {
            moon01.transform.position = display.transform.Find("cell_D_03").transform.position;
            moon02.transform.position = display.transform.Find("cell_G_06").transform.position;
        }
        if (day == 4)
        {
            moon01.transform.position = display.transform.Find("cell_D_04").transform.position;
            moon02.transform.position = display.transform.Find("cell_H_06").transform.position;

            blackShip.SetActive(false);
        }
        if (day == 5)
        {
            moon01.transform.position = display.transform.Find("cell_D_05").transform.position;
            moon02.transform.position = display.transform.Find("cell_I_06").transform.position;

            blackDisplay.SetActive(true);
            TestAudioManager.onGlitch?.Invoke(true);
        }
        if (day == 6)
        {
            moon01.transform.position = display.transform.Find("cell_C_06").transform.position;
            moon02.transform.position = display.transform.Find("cell_J_07").transform.position;
            anomaly02.SetActive(false);
            anomaly03.SetActive(true);

            bigShip.SetActive(true);
            blackDisplay.SetActive(false);
            TestAudioManager.onGlitch?.Invoke(false);
        }
        if (day == 7)
        {
            moon01.transform.position = display.transform.Find("cell_B_06").transform.position;
            moon02.SetActive(false);
            bigShip.SetActive(false);
            TestAudioManager.onGlitch?.Invoke(false);
        }
        if (dayCounter == 8)
        {
            Debug.Log("You have reached your destination! ... or have you?");
            TestSceneManager.onFinishedTask?.Invoke(1);
        }
    }

    // this updates the step counter on screen and disables the ability to move the ship if it hits zero
    private void UpdateCounter()
    {
        movementCounter.GetComponent<TextMeshProUGUI>().text = "0" + stepCounter.ToString();
        if (stepCounter <= 0)
        {
            canMove = false;
            dayButton.SetActive(true); // remove later
        }
    }

    // this moves the ship one cell to whatever direction is chosen
    // it also checks if the ship can move in that direction and updates the step counter
    public void MoveHorizontal(float direction)
    {
        SaveShipPosition();
        if (canMove)
        {
            for (int i = 0; i < display.transform.childCount; i++)
            {
                distanceShipCell = Vector3.Distance(ship.transform.position, display.transform.GetChild(i).transform.position);
                distanceShipClosest = Vector3.Distance(ship.transform.position, closestCell);
                if (direction > 0)
                {
                    if (distanceShipCell < distanceShipClosest && display.transform.GetChild(i).transform.position.x > ship.transform.position.x)
                    {
                        closestCell = display.transform.GetChild(i).transform.position;
                        CheckCell(i);
                        shipFacingRight = true;
                    }
                }
                else
                {
                    if (distanceShipCell < distanceShipClosest && display.transform.GetChild(i).transform.position.x < ship.transform.position.x)
                    {
                        closestCell = display.transform.GetChild(i).transform.position;
                        CheckCell(i);
                        shipFacingRight = false;
                    }
                }
            }
        }

        if (closestCell != Vector3.zero && !cellOccupied)
        {
            ship.transform.position = closestCell;
            stepCounter--;
            UpdateCounter();
            RotateShipHorizontal();
            MoveBlackShip();
            cellOccupied = false;
        }
        closestCell = Vector3.zero;
    }
    public void MoveVertical(float direction)
    {
        SaveShipPosition();
        if (canMove)
        {
            for (int i = 0; i < display.transform.childCount; i++)
            {
                distanceShipCell = Vector3.Distance(ship.transform.position, display.transform.GetChild(i).transform.position);
                distanceShipClosest = Vector3.Distance(ship.transform.position, closestCell);
                if (direction > 0)
                {
                    if (distanceShipCell < distanceShipClosest && display.transform.GetChild(i).transform.position.y > ship.transform.position.y)
                    {
                        closestCell = display.transform.GetChild(i).transform.position;
                        CheckCell(i);
                        shipFacingUp = true;
                    }
                }
                else
                {
                    if (distanceShipCell < distanceShipClosest && display.transform.GetChild(i).transform.position.y < ship.transform.position.y)
                    {
                        closestCell = display.transform.GetChild(i).transform.position;
                        CheckCell(i);
                        shipFacingUp = false;
                    }
                }
            }
        }

        if (closestCell != Vector3.zero && !cellOccupied)
        {
            ship.transform.position = closestCell;
            stepCounter--;
            UpdateCounter();
            RotateShipVertical();
            MoveBlackShip();
            cellOccupied = false; // maybe that helps?
        }
        closestCell = Vector3.zero;
    }

    // this checks if the cell the ship is trying to move to is occupied by another object
    private void CheckCell(int i)
    {
        //Debug.Log("checking cell");
        if (display.transform.GetChild(i).GetComponent<CellScript>().occupied)
        {
            cellOccupied = true;
        }
        else
        {
            cellOccupied = false;
        }
    }

    private void RotateShipHorizontal()
    {
        if (shipFacingRight)
        {
            ship.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        else
        {
            ship.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    private void RotateShipVertical()
    {
        if (shipFacingUp)
        {
            ship.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            ship.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    private void SaveShipPosition()
    {
        if (dayCounter == 2 || dayCounter == 3)
        {
            priorShipPosition = ship.transform.position;
            priorShipRotation = ship.transform.rotation;
        }
    }
    private void MoveBlackShip()
    {
        if (dayCounter == 2 || dayCounter == 3)
        {
            blackShip.transform.position = priorShipPosition;
            blackShip.transform.rotation = priorShipRotation;
        }
    }
}
