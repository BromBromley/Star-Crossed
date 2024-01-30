using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // this script manages the input for the player movement

    private Rigidbody rb;
    private float movement;
    private float lastMovement;
    //private float speed = 0;
    private float maxSpeed = 3f;
    private float acceleration;
    private float stoppingForce;

    public bool playerIsFloating;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        movement = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (movement > 0.0f || movement < 0.0f)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        //Debug.Log("moving");
        rb.velocity = new Vector3(movement * maxSpeed, 0, 0);
    }

    /*
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

        rb.velocity = new Vector3(movement * speed, 0, 0);

        lastMovement = movement;
    }

    // this gives the movement a fade out
    private void Decelerate()
    {
        speed -= stoppingForce * Time.deltaTime;

        rb.velocity = new Vector3(lastMovement * speed, 0, 0);

        if (speed <= 0.0f)
        {
            speed = 0.0f;
            lastMovement = 0;
        }
    }
    */
}
