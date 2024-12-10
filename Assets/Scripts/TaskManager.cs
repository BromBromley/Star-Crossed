using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    // this script manages the different tasks and their availability

    [SerializeField] private GameObject taskList;
    private List<GameObject> tasks;

    private Hashtable taskAirlock = new Hashtable();
    private Hashtable taskSpacesuit = new Hashtable();
    private Hashtable taskHelm = new Hashtable();
    private Hashtable taskConsole = new Hashtable();
    private Hashtable taskKitchen = new Hashtable();
    private Hashtable taskPlant = new Hashtable();
    Hashtable[] sortedTaskList = new Hashtable[6];
    private int taskIndex;

    public delegate void OnStartingTask(int task);
    public static event OnStartingTask onStartingTask;

    public delegate void OnCompletedTask();
    public static OnCompletedTask onCompletingTask;

    [SerializeField] private bool showLogs;



    void Start()
    {
        PlayerInteractions.onInteraction += CheckWhichTask;
        GameManager.onNewDay += SetUpTasks;
        onCompletingTask += CompletedTask;

        tasks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Task"));

        InitializeTasksOnStart();
    }



    // this assigns the hashtable for each task with all the necessary information
    private void InitializeTasksOnStart()
    {
        foreach (GameObject task in tasks)
        {
            if (task.name == "airlock")
            {
                taskAirlock.Add("object", task);
                taskAirlock.Add("UI", taskList.transform.GetChild(0).gameObject);
                //taskAirlock.Add("isNeglected", false);
                ((GameObject)taskAirlock["UI"]).GetComponentInChildren<TextMeshProUGUI>().text = "Put on your spacesuit and repair the outside hull";
                taskAirlock.Add("day 1", 2);
            }
            if (task.name == "spacesuit")
            {
                taskSpacesuit.Add("object", task);
                taskSpacesuit.Add("UI", taskList.transform.GetChild(1).gameObject);
                //taskSpacesuit.Add("isNeglected", false);
                ((GameObject)taskSpacesuit["UI"]).GetComponentInChildren<TextMeshProUGUI>().text = "Check if your spacesuit is operational";
                taskSpacesuit.Add("day 1", 0);
                taskSpacesuit.Add("day 2", 5);
            }
            if (task.name == "helm")
            {
                taskHelm.Add("object", task);
                taskHelm.Add("UI", taskList.transform.GetChild(2).gameObject);
                //taskHelm.Add("isNeglected", false);
                ((GameObject)taskHelm["UI"]).GetComponentInChildren<TextMeshProUGUI>().text = "Check the ship's course at the helm";
            }
            if (task.name == "console")
            {
                taskConsole.Add("object", task);
                taskConsole.Add("UI", taskList.transform.GetChild(3).gameObject);
                //taskConsole.Add("isNeglected", false);
                ((GameObject)taskConsole["UI"]).GetComponentInChildren<TextMeshProUGUI>().text = "Do a routine check of the ship systems";
                taskConsole.Add("day 1", 1);
                taskConsole.Add("day 2", 3);
                taskConsole.Add("day 3", 5);
                taskConsole.Add("day 4", 7);
            }
            if (task.name == "kitchen")
            {
                taskKitchen.Add("object", task);
                taskKitchen.Add("UI", taskList.transform.GetChild(4).gameObject);
                //taskKitchen.Add("isNeglected", false);
                ((GameObject)taskKitchen["UI"]).GetComponentInChildren<TextMeshProUGUI>().text = "Get a food ration from the kitchen";
            }
            if (task.name == "plant")
            {
                taskPlant.Add("object", task);
                taskPlant.Add("UI", taskList.transform.GetChild(5).gameObject);
                //taskPlant.Add("isNeglected", false);
                ((GameObject)taskPlant["UI"]).GetComponentInChildren<TextMeshProUGUI>().text = "Water the plants in the observation deck";
            }
        }
        sortedTaskList[0] = taskAirlock;
        sortedTaskList[1] = taskSpacesuit;
        sortedTaskList[2] = taskHelm;
        sortedTaskList[3] = taskConsole;
        sortedTaskList[4] = taskKitchen;
        sortedTaskList[5] = taskPlant;
    }



    private void ResetTasks()
    {
        foreach (GameObject task in tasks)
        {
            task.SetActive(false);
        }
        foreach (Transform child in taskList.transform)
        {
            child.gameObject.SetActive(false);
        }
    }



    // this activates all the tasks that need to be done that day and their description in the UI
    private void SetUpTasks(int day)
    {
        ResetTasks();

        ((GameObject)taskHelm["object"]).SetActive(true);
        ((GameObject)taskHelm["UI"]).SetActive(true);

        ((GameObject)taskKitchen["object"]).SetActive(true);
        ((GameObject)taskKitchen["UI"]).SetActive(true);

        ((GameObject)taskPlant["object"]).SetActive(true);
        ((GameObject)taskPlant["UI"]).SetActive(true);

        foreach (Hashtable task in sortedTaskList)
        {
            if (task.ContainsValue(day))
            {
                ((GameObject)task["object"]).SetActive(true);
                ((GameObject)task["UI"]).SetActive(true);
            }
        }
        // additional check if something has been neglected and needs to be activated too
    }



    private void CheckWhichTask(bool isTask)
    {
        if (isTask)
        {
            foreach (GameObject task in tasks)
            {
                if (task.GetComponent<Interactable>().thisInteractable)
                {
                    task.GetComponent<Interactable>().thisInteractable = false;
                    taskIndex = task.GetComponent<Interactable>().taskNumber;
                }
            }
            onStartingTask?.Invoke(taskIndex);
        }
    }



    private void CompletedTask()
    {
        ((GameObject)sortedTaskList[taskIndex]["object"]).SetActive(false);
        ((GameObject)sortedTaskList[taskIndex]["UI"]).SetActive(false);
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
