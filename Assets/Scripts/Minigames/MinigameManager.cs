using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    // this manages the minigames 

    public bool isTestScene;

    [SerializeField] private GameObject[] minigames = new GameObject[4];


    void Start()
    {
        TaskManager.onStartingTask += OpenMinigame;
    }
    
    private void OpenMinigame(int index)
    {
        minigames[index].SetActive(true);
        // Reihenfolge anders, stattdessen check, welches aktiviert werden muss
    }

}
