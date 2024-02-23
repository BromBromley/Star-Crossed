using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    // this script sends the player to the room they entered
    // attached to every doorway collider

    public bool goingLeft;

    public void EnteringDoor(GameObject player)
    {
        Debug.Log("entering room");
        if (goingLeft)
        {
            player.transform.position += new Vector3(-60f, 0, 0);
        }
        else
        {
            player.transform.position += new Vector3(60f, 0, 0);
        }
    }
}
