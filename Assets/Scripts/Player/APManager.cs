using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APManager : MonoBehaviour
{
    // this manages the player's action points 

    public int AP = 4;
    [SerializeField] private Slider apSlider;

    [SerializeField] private bool showLogs;

    void Start()
    {
        TaskManager.onCompletingTask += SubtractAP;
        GameManager.onNewDay += RefillAP;

        apSlider.value = AP;
    }

    private void SubtractAP()
    {
        AP--;
        apSlider.value = AP;
        Log(AP.ToString());
    }

    private void RefillAP(int i)
    {
        AP = 4;
        apSlider.value = AP;
        Log(AP.ToString());
    }

    // checks if logs should be send to the console
    private void Log(string message)
    {
        if (showLogs)
        {
            Debug.Log(message);
        }
    }
}
