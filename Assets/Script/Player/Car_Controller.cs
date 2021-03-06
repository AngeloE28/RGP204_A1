using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Controller : MonoBehaviour
{
    public Rigidbody carControllerRb;
    public SphereCollider carControllerCollider;
    public BoxCollider carCollider;
    public Gun gun;

    public float maxPlayerHealth;
    private float currentPlayerhealth;
    public float smoothCarRotationVal = 15.0f; // For smooth rotations, when going up ramps
    public float carFlipRotationVal = 5.0f; // For smooth rotations, when resetting the z & x-axis
    private Vector3 originalPos;

    [Header("Movement Values")]
    // Accelerations
    public float forwardAccel;
    public float reverseAccel, accelMultiplier;
    public float gravityForce, gravityMultiplier, dragOnGround, dragInAir; // Controls the added gravity to the car
    public float turnStrength; // Maximum angle the car can turn

    // Inputs
    public float speedInput, turnInput, flipInput;

    private bool isGrounded; // Is car grounded?
    private float notGroundedTimer; // Timer to flip the car

    [Header("Raycast System")]
    public LayerMask groundMask;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    [Header("Wheels")]
    public float maxWheelTurnAngle; // Max angle, the wheel will turn
    public Transform frontLeftWheel, frontRightWheel; // Gets the front wheels

    [Header("Particle System")]
    public float maxEmission; // Max particle being emitted
    private float emissionRate; // Controls how many particles to emit
    public ParticleSystem[] trailEffects; // Gets all the particle systems

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        currentPlayerhealth = maxPlayerHealth;
        // Hide the cursor during play
        Cursor.visible = false;

        // Unparent the car controller that will be followed
        carControllerRb.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
    }

    public void PlayerTakeDamage(float damage)
    {
        currentPlayerhealth -= damage;

        if (currentPlayerhealth <= 0)
            Invoke("PlayerDeath", 0.01f);
    }

    private void PlayerDeath()
    {
        // Disable the game object for a set amount of time to replicate respawning
        this.gameObject.SetActive(false);
        Invoke("Respawn", 0.25f);
    }

    private void Respawn()
    {
        // Enable the game object replicate respawning
        // Reset all original values
        // Re-activate at original position
        this.gameObject.SetActive(true);

        currentPlayerhealth = maxPlayerHealth;
        carControllerRb.position = originalPos;
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

        // Turn both wheels in the direction the car is turning
        if (speedInput >= 0) // For going forward
        {
            frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x,
                                                            (turnInput * maxWheelTurnAngle) - 90, // Takeaway 90 degrees to compensate the wheels rotation
                                                            frontLeftWheel.localRotation.eulerAngles.z);

            frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x,
                                                            turnInput * maxWheelTurnAngle + 90, // Add 90 degrees to compensate the wheels rotation
                                                            frontRightWheel.localRotation.eulerAngles.z);
        }
        else // Reversing
        {
            frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x,
                                                            -((turnInput * maxWheelTurnAngle) + 90), // Add 90 degrees to compensate the wheels rotation
                                                            frontLeftWheel.localRotation.eulerAngles.z);

            frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x,
                                                            -(turnInput * maxWheelTurnAngle - 90), // Takeaway 90 degrees to compensate the wheels rotation
                                                            frontRightWheel.localRotation.eulerAngles.z);
        }

        // Allow car to flip
        flipInput = Input.GetAxis("Mouse Y");

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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothCarRotationVal);
        }

        // Don't emit anything when not moving
        emissionRate = 0.0f;

        // Check to see if car is grounded
        if (isGrounded)
        {
            // Reset the timer
            notGroundedTimer = 5f;

            carControllerRb.drag = dragOnGround;

            // Checks to see if there is input received
            if (Mathf.Abs(speedInput) > 0.0f)
            {
                carControllerRb.AddForce(transform.forward * speedInput);

                emissionRate = maxEmission; // Start playing particles
            }
        }
        else
        {
            // Car not grounded
            carControllerRb.drag = dragInAir;

            // Increases the gravity applied to the car
            carControllerRb.AddForce(Vector3.up * -gravityForce * gravityMultiplier);

            // Manual reset
            if (flipInput > 0)
            {
                CarFlip();
            }

            // Automatic reset
            // Resets the car's x & z-rotation when the car is not grounded for a set amount of time
            if (notGroundedTimer > 0.0f)
            {
                notGroundedTimer -= Time.deltaTime;
            }
            else
            {
                // Flip the car
                CarFlip();
            }

        }

        // Controls the emission of the trailEffects
        foreach(ParticleSystem trail in trailEffects)
        {
            var emissionModule = trail.emission;
            emissionModule.rateOverTime = emissionRate;
        }
    }

    private void CarFlip()
    {
        // Rotate the car to reset the rotation
        Quaternion targetRotations;
        // Get the euler angles
        Vector3 eulerRotation = transform.rotation.eulerAngles;

        // Get target rotation
        targetRotations = Quaternion.Euler(0.0f, eulerRotation.y, 0.0f);

        // Rotate the car
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotations, Time.deltaTime * carFlipRotationVal);
    }

}
