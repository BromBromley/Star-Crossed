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

    void Start()
    {
        interactIcon.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        interactIcon.SetActive(true);
        //interactIcon.transform.position += new Vector3(this.transform.position.x, 0, 0); <- good idea, but doesn't work as hoped

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
    }

    private void OnTriggerExit(Collider other)
    {
        interactIcon.SetActive(false);
    }

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
