﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Controller : MonoBehaviour
{
    public Rigidbody carControllerRb;
    public SphereCollider carControllerCollider;
    public BoxCollider carCollider;
    public Gun gun;

    public float smoothVal = 0.2f; // For smooth rotations, when going up ramps

    [Header("Movement Values")]
    // Accelerations
    public float forwardAccel;
    public float reverseAccel, accelMultiplier;
    public float gravityForce, gravityMultiplier, dragOnGround, dragInAir; // Controls the added gravity to the car
    public float turnStrength; // Maximum angle the car can turn

    // Inputs
    public float speedInput, turnInput;

    private bool isGrounded; // Is car grounded?
    [Header("Raycast System")]
    public LayerMask groundMask;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        // Unparent the car controller that will be followed
        carControllerRb.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (gun.shooting || !gun.shooting)
            PlayerInputs();
    }

    private void PlayerInputs()
    {
        // Speed is 0.0f, since there is no inputs
        speedInput = 0.0f;

        // The car controller's sphere collider ignores the  car's box collider
        Physics.IgnoreCollision(carControllerCollider, carCollider, true);

        // Going forward
        if (Input.GetAxis("Fire1") > 0)
        {
            speedInput = Input.GetAxis("Fire1") * forwardAccel * accelMultiplier;
        }
        // Reversing
        if (Input.GetAxis("Fire3") > 0)
        {
            speedInput = Input.GetAxis("Fire3") * -reverseAccel * accelMultiplier;
        }

        // Turning left and right
        turnInput = Input.GetAxis("Mouse X");

        // Can only turn if player is grounded and not moving
        if (isGrounded && speedInput != 0.0f)
        {
            if (Input.GetAxis("Fire1") > 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f,
                                                      turnInput * turnStrength * Input.GetAxis("Fire1") * Time.deltaTime,
                                                      0.0f));
            }
            if (Input.GetAxis("Fire3") > 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f,
                                      turnInput * turnStrength * Input.GetAxis("Fire3") * Time.deltaTime,
                                      0.0f));
            }
        }
        
        // Car follows the carController thats moving
        transform.position = carControllerRb.transform.position;
    }


    private void FixedUpdate()
    {
        isGrounded = false;
        RaycastHit hit;

        // Check if the ray is colliding with any object with the Ground layermask
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, groundMask))
        {
            isGrounded = true;

            // Rotate the car when flying up
            Quaternion targetRotation;

            // Get target rotation
            targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // Smooth out rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothVal);
        }

        // Check to see if car is grounded
        if (isGrounded)
        {
            carControllerRb.drag = dragOnGround;

            // Checks to see if there is input received
            if (Mathf.Abs(speedInput) > 0.0f)
            {
                carControllerRb.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            // Car not grounded
            carControllerRb.drag = dragInAir;

            // Increases the gravity applied to the car
            carControllerRb.AddForce(Vector3.up * -gravityForce * gravityMultiplier);
        }
    }
}