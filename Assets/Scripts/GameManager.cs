using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public delegate void OnPausingGame();
    //public static event OnPausingGame onPausingGame;

    public delegate void OnContinuingGame();
    public static event OnContinuingGame onContinuingGame;

    public delegate void OnNewDay(int day);
    public static event OnNewDay onNewDay;

    private int testDay;

    public void UnpauseGame()
    {
        onContinuingGame?.Invoke();
        Debug.Log("unpausing game");
    }

    void Update()
    {
        // this is for testing the day system
        if (Input.GetKeyDown(KeyCode.N))
        {
            onNewDay?.Invoke(testDay);
            testDay++;
        }
    }
}
