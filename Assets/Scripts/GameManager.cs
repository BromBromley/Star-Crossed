using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private InputAction pauseAction;
    private bool isPaused;

    public delegate void OnPausingGame();
    public static event OnPausingGame onPausingGame;
    public static OnPausingGame onContinuingGame;

    public delegate void OnNewDay(int day);
    public static event OnNewDay onNewDay;

    private int testDay;



    void Awake()
    {
        playerControls = new PlayerInputActions();
    }



    void OnEnable()
    {
        pauseAction = playerControls.UI.Pause;
        pauseAction.Enable();
        pauseAction.performed += PressedPause;
    }


    void OnDisable()
    {
        pauseAction.Disable();
    }



    void Update()
    {
        // this is for testing the day system
        if (Input.GetKeyDown(KeyCode.N) && testDay < 8)
        {
            onNewDay?.Invoke(testDay);
            testDay++;
        }
    }



    private void PressedPause(InputAction.CallbackContext callbackContext)
    {
        if (isPaused)
        {
            UnpauseGame();
        }
        else
        {
            onPausingGame?.Invoke();
            PauseGame();
        }
    }



    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }


    private void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        onContinuingGame?.Invoke();
        Debug.Log("unpausing game");
    }

}
