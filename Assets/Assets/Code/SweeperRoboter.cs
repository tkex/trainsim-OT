using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SweeperRoboter : MonoBehaviour
{


    // Boolean flag to check if the robot is turned on
    public bool isTurnedOn = false;

    [Header("Prefab Settings")]

    // The eyes of the robot
    public GameObject eyes;

    // The left brush of the robot
    public GameObject leftBrush;

    // The right brush of the robot
    public GameObject rightBrush;

    [Header("Speed And Direction Settings")]

    // The speed of the robot
    public float roboterSpeed = 5f;

    // The active duration of the robot
    public float activeDuration = 30f;

    // The rotation speed of the brushes
    public float brushRotationSpeed = 200f;

    // The interval between blinks
    public float blinkInterval = 1f;

    // Boolean flag to check if the blinking coroutine is running
    private bool isBlinking = false;

    // The Rigidbody component of the robot
    private Rigidbody rb;

    // The current movement direction of the robot
    private Vector3 direction;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        direction = transform.forward;
    }
    
    void Update()
    {
        if (isTurnedOn)
        {
            // Move the robot in the current direction
            rb.velocity = direction * roboterSpeed;

            // Rotate the brushes in opposite directions
            leftBrush.transform.DORotate(new Vector3(0, 0, brushRotationSpeed), 1, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Incremental);
            rightBrush.transform.DORotate(new Vector3(0, 0, -brushRotationSpeed), 1, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Incremental);

            // Start the timer and turn off the robot after the specified duration
            StartCoroutine(TurnOffAfterDuration(activeDuration));

            // Start blinking the eyes
            if (!isBlinking)
            {
                StartCoroutine(BlinkEyes(blinkInterval));
            }
        }
        else
        {
            // Stop movement
            rb.velocity = Vector3.zero;

            // Stop rotating the brushes
            leftBrush.transform.DOKill();
            rightBrush.transform.DOKill();

            // Stop blinking the eyes
            if (isBlinking)
            {
                StopCoroutine(BlinkEyes(blinkInterval));
                isBlinking = false;
            }

            // Make sure eyes are turned off when the robot is turned off
            eyes.SetActive(false);
        }
    }

    IEnumerator TurnOffAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        isTurnedOn = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Change the direction when a collision occurs
        //direction = Vector3.Reflect(direction, collision.contacts[0].normal);

        // Calculate reflection direction based on the angle of incidence and the normal at the point of collision
        Vector3 reflectDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);

        // Ensure the reflection direction stays on the ground
        reflectDirection.y = 0;

        // Change the rotation of the robot to face the reflection direction
        transform.rotation = Quaternion.LookRotation(reflectDirection);

        // Update the movement direction of the robot
        direction = transform.forward;
    }

    IEnumerator BlinkEyes(float interval)
    {
        isBlinking = true;

        while (true)
        {
            eyes.SetActive(!eyes.activeSelf);
            yield return new WaitForSeconds(interval);
        }
    }
}
