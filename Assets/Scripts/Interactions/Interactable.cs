using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // this script tells the interaction manager script (which doesn't exist yet) what object the player is interacting with
    // attached to every interactable object and name set manually in the editor

    public string interactableName;
    public int taskNumber;

    public bool thisInteractable;

    // this changes the name of the INTERACTABLE gameObject for easier use in other scripts
    void Awake()
    {
        this.gameObject.name = interactableName;
    }
}
