using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // this script tells the interaction manager script (which doesn't exist yet) what object the player is interacting with
    // attached to every interactable object and name set manually in the editor

    public string interactableName;

    public bool thisInteractable;
}
