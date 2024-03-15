using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    // this script manages the interactions with doors and items

    [SerializeField] private GameObject interactIcon;

    private bool canEnterDoor = true;
    public delegate void OnUsingDoor();
    public static event OnUsingDoor onUsingDoor;
    // 'event' prevents the delegate from being called by a different script, leave out if necessary

    public delegate void OnInteraction();
    public static event OnInteraction onInteraction;

    void Start()
    {
        interactIcon.SetActive(false);
    }

    // checks if the player is standing in front of and interacting with an object
    private void OnTriggerStay(Collider other)
    {
        // shows the interaction button
        interactIcon.SetActive(true);
        //interactIcon.transform.position += new Vector3(this.transform.position.x, 0, 0); //< -good idea, but doesn't work as hoped

        if (other.tag == "Door" && canEnterDoor && Input.GetKey(KeyCode.E))
        {
            other.GetComponent<DoorInteraction>().EnteringDoor(this.gameObject);
            onUsingDoor?.Invoke();
            StartCoroutine(DoorCooldown());
        }

        if (other.tag == "Ladder" && canEnterDoor && Input.GetKey(KeyCode.E))
        {
            other.GetComponent<LadderInteraction>().UsingLadder(this.gameObject);
            onUsingDoor?.Invoke();
            StartCoroutine(DoorCooldown());
        }

        if (other.tag == "Interactable" && Input.GetKeyDown(KeyCode.E))
        {
            other.GetComponent<Interactable>().thisInteractable = true;
            onInteraction?.Invoke();
            Debug.Log("interacting");
        }
    }

    // hides the interaction button
    private void OnTriggerExit(Collider other)
    {
        interactIcon.SetActive(false);
    }

    // automatically teleports the player if they walk through a door
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Door" && canEnterDoor)
        {
            other.gameObject.GetComponent<DoorInteraction>().EnteringDoor(this.gameObject);
            onUsingDoor?.Invoke();
            StartCoroutine(DoorCooldown());
        }
    }

    // this prevents accidentally spamming through doors
    private IEnumerator DoorCooldown()
    {
        canEnterDoor = false;
        yield return new WaitForSeconds(1);
        canEnterDoor = true;
    }
}
