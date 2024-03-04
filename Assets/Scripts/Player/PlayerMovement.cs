using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // this script manages the input for the player movement

    private UnityEngine.Vector3 movementDirection;
    private float movement;
    private UnityEngine.Vector3 lastMovement;
    private float speed = 0;
    private float maxSpeed = 14f;
    private float acceleration = 14f;
    private float stoppingForce = 18f;

    // public bool playerIsFloating;

    private void Update()
    {
        movement = Input.GetAxis("Horizontal");
        Debug.Log(speed);
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
    }

    // this gives the movement a fade in
    private void Accelerate()
    {
        //Debug.Log("moving");
        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }
        else if (speed > maxSpeed)
        {
            speed -= stoppingForce * Time.deltaTime;
        }

        speed = Mathf.Clamp(speed, 0, maxSpeed);

        movementDirection = new UnityEngine.Vector3(movement, 0, 0);
        lastMovement = movementDirection;

        transform.position += new UnityEngine.Vector3(movement, 0, 0) * speed * Time.deltaTime;
    }

    // this gives the movement a fade out
    private void Decelerate()
    {
        speed -= stoppingForce * Time.deltaTime;

        transform.position += lastMovement * speed * Time.deltaTime;

        if (speed <= 0.0f)
        {
            speed = 0.0f;
            lastMovement = UnityEngine.Vector3.zero;
        }
    }
}