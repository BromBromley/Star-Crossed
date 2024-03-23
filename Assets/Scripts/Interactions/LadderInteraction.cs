using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderInteraction : MonoBehaviour
{
    // this script sends the player up and down the ladder
    // attached to the ladder collider

    public bool goingUp;

    public void UsingLadder(GameObject player)
    {
        StartCoroutine(TeleportDelay(player));
    }

    // delays the teleport of the player to match the fade to black
    private IEnumerator TeleportDelay(GameObject player)
    {
        yield return new WaitForSeconds(0.3f);
        if (goingUp)
        {
            player.transform.position += new Vector3(0, 100f, 0);
        }
        else
        {
            player.transform.position += new Vector3(0, -100f, 0);
        }
    }
}
