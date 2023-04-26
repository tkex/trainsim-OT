using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntranceController : MonoBehaviour
{

    [Header("Door Settings")]
    // The game object representing the door
    [SerializeField] private GameObject entranceGo;

    // Flag to progress if the door should be opened
    [SerializeField] private bool openDoor = false;

    [Tooltip("Delay before the start of the vertical door movement")]
    // Delay before the door movement starts

    [SerializeField] private float delay = 1f;
    [Tooltip("Duration of the vertical door movement")]

    // Duration of the door movement
    [SerializeField] private float duration = 3f;
    [Tooltip("Distance of the vertical door movement")]

    // Distance the door will move
    [SerializeField] private float distance = 3f;

    // Original position of the door
    private Vector3 originalPosition;

    // Target position of the door
    private Vector3 targetPosition;


    private void Start()
    {
        // Store the original position of the door
        originalPosition = entranceGo.transform.position; 

        // Calculate the target position of the door
        targetPosition = originalPosition + Vector3.up * distance;
    }

    private void Update()
    {
        if (openDoor)
        {
            // Door movement to open the door
            OpenDoor();
           
        } else
        {
            CloseDoor();
        }
    }

    public void SetOpenDoor(bool value)
    {
        // Set the value of the openDoor flag
        openDoor = value;
    }

    public void OpenDoor()
    {
        entranceGo.transform.DOMove(targetPosition, duration)
               .SetEase(Ease.OutQuad);
    }

    public void CloseDoor()
    {
        // Door movement to close the door
        entranceGo.transform.DOMove(originalPosition, duration)
            .SetEase(Ease.OutQuad);
    }
}