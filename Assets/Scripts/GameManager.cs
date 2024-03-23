using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public delegate void OnPausingGame();
    //public static event OnPausingGame onPausingGame;

    public delegate void OnContinuingGame();
    public static event OnContinuingGame onContinuingGame;

    public void UnpauseGame()
    {
        onContinuingGame?.Invoke();
        Debug.Log("unpausing game");
    }
}
