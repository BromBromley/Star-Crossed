using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    // this script manages the interactions with doors and items

    [SerializeField] private GameObject interactIcon;

    private bool isInteracting;
    private bool canEnterDoor = true;
    public delegate void OnUsingDoor();
    public static event OnUsingDoor onUsingDoor;
    // 'event' prevents the delegate from being called by a different script, leave out if necessary

    public delegate void OnInteraction(bool isTask);
    public static event OnInteraction onInteraction;

    private PlayerInputActions playerControls;
    private InputAction interaction;

    [SerializeField] private bool showLogs;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
        interactIcon.SetActive(false);
    }

    private void OnEnable()
    {
        interaction = playerControls.Player.Interact;
        interaction.Enable();
        interaction.performed += Interact;
    }

    private void OnDisable()
    {
        interaction.Disable();
    }

    // checks if the player is standing in front of and interacting with an object
    // maybe change to OnTriggerEnter in order to fix weird bug?
    private void OnTriggerStay(Collider other)
    {
        // shows the interaction button
        interactIcon.SetActive(true);

        if (isInteracting)
        {
            if (other.tag == "Door" && canEnterDoor)
            {
                other.GetComponent<DoorInteraction>().EnteringDoor(this.gameObject);
                onUsingDoor?.Invoke();
                StartCoroutine(DoorCooldown());
                Log("using door");
            }

            if (other.tag == "Ladder" && canEnterDoor)
            {
                other.GetComponent<LadderInteraction>().UsingLadder(this.gameObject);
                onUsingDoor?.Invoke();
                StartCoroutine(DoorCooldown());
                Log("using ladder");
            }

            if (other.tag == "Interactable" || other.tag == "Task")
            {
                other.GetComponent<Interactable>().thisInteractable = true;
                onInteraction?.Invoke(true);
                Log("interacting with task");
            }
            if (other.tag == "Interactable")
            {
                other.GetComponent<Interactable>().thisInteractable = true;
                onInteraction?.Invoke(false);
                Log("interacting");
            }
            isInteracting = false;
        }
    }

    // hides the interaction button
    private void OnTriggerExit(Collider other)
    {
        interactIcon.SetActive(false);
        isInteracting = false;
    }

    // automatically teleports the player if they walk through a door
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Door" && canEnterDoor)
        {
            other.gameObject.GetComponent<DoorInteraction>().EnteringDoor(this.gameObject);
            onUsingDoor?.Invoke();
            StartCoroutine(DoorCooldown());
            Log("using door");
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        isInteracting = true;
    }

    // this prevents accidentally spamming through doors
    private IEnumerator DoorCooldown()
    {
        canEnterDoor = false;
        yield return new WaitForSeconds(1);
        canEnterDoor = true;
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
