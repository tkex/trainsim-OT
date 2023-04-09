using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntranceController : MonoBehaviour
{
    public GameObject entranceGo; // The game object representing the door

    [Header("Einfahrt-Einstellungen")]
    [Tooltip("Delay before the start of the vertical door movement")]
    public float delay = 1f; // Delay before the door movement starts
    [Tooltip("Duration of the vertical door movement")]
    public float duration = 3f; // Duration of the door movement
    [Tooltip("Distance of the vertical door movement")]
    public float distance = 3f; // Distance by which the door should move

    private Vector3 originalPosition; // Original position of the door
    private Vector3 targetPosition; // Target position of the door

    private void Start()
    {
        originalPosition = entranceGo.transform.position; // store the original position of the door
        targetPosition = originalPosition + Vector3.up * distance; // calculate the target position of the door

        // Start the door movement
        entranceGo.transform.DOMove(targetPosition, duration)
            .SetDelay(delay)
            .SetEase(Ease.OutQuad);
    }

    public void OpenDoor()
    {
        // Door movement to open the door
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
