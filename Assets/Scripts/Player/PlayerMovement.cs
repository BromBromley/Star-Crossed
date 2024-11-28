using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // this script manages the input for the player movement

    private bool playerCanMove = true;
    private Rigidbody rb;
    private float movement;
    private UnityEngine.Vector3 lastMovement;
    private float speed = 0;
    private float maxSpeed = 8f;
    private float acceleration = 16f;
    private float stoppingForce = 30f;

    // public bool playerIsFloating;
    private PlayerInputActions playerControls;
    private InputAction moveAction;

    private Animator animator;
    [SerializeField] private GameObject playerSprite;
    private bool facingLeft = false;



    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }


    private void OnEnable()
    {
        moveAction = playerControls.Player.Move;
        moveAction.Enable();
    }


    private void OnDisable()
    {
        moveAction.Disable();
    }



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        PlayerInteractions.onInteraction += SwitchMovementBool;
        PlayerInteractions.onUsingDoor += PausePlayerMovement;
        GameManager.onContinuingGame += EnableMovement;
    }


    private void Update()
    {
        if (playerCanMove)
        {
            movement = moveAction.ReadValue<UnityEngine.Vector2>().x;
        }
        else
        {
            movement = 0f;
        }
    }


    private void FixedUpdate()
    {
        if (movement > 0.0f || movement < 0.0f)
        {
            Accelerate();
        }
        else
        {
            Decelerate();
        }

        // this checks the player's direction and flips the sprite accordingly
        if (movement < 0 && !facingLeft)
        {
            FlipSprite();
        }
        else if (movement > 0 && facingLeft)
        {
            FlipSprite();
        }

        animator.SetFloat("speed", Mathf.Abs(movement));
    }



    // this gives the movement a fade in
    private void Accelerate()
    {
        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }
        else if (speed > maxSpeed)
        {
            speed -= stoppingForce * Time.deltaTime;
        }

        speed = Mathf.Clamp(speed, 0, maxSpeed);

        lastMovement = new UnityEngine.Vector3(movement, 0, 0);

        rb.velocity = new UnityEngine.Vector3(movement * speed, 0, 0);
    }


    // this gives the movement a fade out
    private void Decelerate()
    {
        //speed -= stoppingForce * Time.deltaTime;

        //rb.velocity = lastMovement * speed;
        rb.velocity -= 0.2f * rb.velocity;

        /*if (speed <= 0.0f)
        {
            speed = 0.0f;
            lastMovement = UnityEngine.Vector3.zero;
        }*/
    }



    // changes the state of the bool to match the interactions
    private void SwitchMovementBool(bool isTask)
    {
        playerCanMove = !playerCanMove;
    }


    private void EnableMovement()
    {
        playerCanMove = true;
    }



    // this disables the player movement for a short period while going through doors
    private void PausePlayerMovement()
    {
        StartCoroutine(PausingPlayerMovement());
    }


    private IEnumerator PausingPlayerMovement()
    {
        playerCanMove = false;
        yield return new WaitForSeconds(0.5f);
        playerCanMove = true;
    }



    private void FlipSprite()
    {
        UnityEngine.Vector3 currentScale = playerSprite.transform.localScale;
        currentScale.x *= -1;
        playerSprite.transform.localScale = currentScale;

        facingLeft = !facingLeft;
    }

}