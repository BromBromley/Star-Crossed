using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    // this script manages all the interactables for the different tasks

    private List<GameObject> interactables;
    private string taskName;

    void Start()
    {
        interactables = new List<GameObject>(GameObject.FindGameObjectsWithTag("Interactable"));
        PlayerInteractions.onInteraction += CheckWhichInteractable;
    }

    // this goes through the list of interactables to check which one should be triggered
    // immediately sets the interaction bool to false once it has found the right one
    private void CheckWhichInteractable()
    {
        foreach (GameObject interaction in interactables)
        {
            if (interaction.GetComponent<Interactable>().thisInteractable)
            {
                taskName = interaction.GetComponent<Interactable>().interactableName;
                interaction.GetComponent<Interactable>().thisInteractable = false;
            }
        }
        Debug.Log("You're interacting with " + taskName);
    }
}
