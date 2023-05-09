using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntranceController : MonoBehaviour
{
    public enum DoorType
    {
        Entrance,
        Exit
    }

    [Header("Door Settings")]
    [SerializeField] private GameObject entranceDoor;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private DoorType doorType;
    [SerializeField] private float delayForDoorClose = 1f;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float distance = 3f;

    private Vector3 entranceOriginalPosition;
    private Vector3 entranceTargetPosition;
    private Vector3 exitOriginalPosition;
    private Vector3 exitTargetPosition;
    private bool openDoor = false;

    private void Start()
    {
        entranceOriginalPosition = entranceDoor.transform.position;
        entranceTargetPosition = entranceOriginalPosition + Vector3.up * distance;
        exitOriginalPosition = exitDoor.transform.position;
        exitTargetPosition = exitOriginalPosition + Vector3.up * distance;
    }

    private void Update()
    {
        if (openDoor)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OnEnable()
    {
        switch (doorType)
        {
            case DoorType.Entrance:
                TrainController.OnTrainArrived += OpenDoor;
                break;
            case DoorType.Exit:
                TrainController.OnTrainDeparting += OpenDoor;
                break;
        }
    }

    private void OnDisable()
    {
        switch (doorType)
        {
            case DoorType.Entrance:
                TrainController.OnTrainArrived -= OpenDoor;
                break;
            case DoorType.Exit:
                TrainController.OnTrainDeparting -= OpenDoor;
                break;
        }
    }

    public void OpenDoor()
    {
        entranceDoor.transform.DOMove(entranceTargetPosition, duration).SetEase(Ease.OutQuad);
        exitDoor.transform.DOMove(exitTargetPosition, duration).SetEase(Ease.OutQuad);
        openDoor = true;

        // Close the door after delay
        StartCoroutine(CloseDoorWithDelay(delayForDoorClose));
    }

    public void CloseDoor()
    {
        entranceDoor.transform.DOMove(entranceOriginalPosition, duration).SetEase(Ease.OutQuad);
        exitDoor.transform.DOMove(exitOriginalPosition, duration).SetEase(Ease.OutQuad);
        openDoor = false;
    }

    private IEnumerator CloseDoorWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        CloseDoor();
    }
}