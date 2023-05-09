using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntranceController : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private GameObject movingDoor;
    [SerializeField] private DoorType doorType;
    [SerializeField] private float delayForDoorClose = 1f;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float distance = 3f;

    private Vector3 doorOriginalPosition;
    private Vector3 doorTargetPosition;
    private bool openDoor = false;

    private void Start()
    {
        doorOriginalPosition = movingDoor.transform.position;
        doorTargetPosition = doorOriginalPosition + Vector3.up * distance;
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
                TrainController.OnTrainArriving += OpenDoor;
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
                TrainController.OnTrainArriving -= OpenDoor;
                break;
            case DoorType.Exit:
                TrainController.OnTrainDeparting -= OpenDoor;
                break;
        }
    }

    public void OpenDoor()
    {
        movingDoor.transform.DOMove(doorTargetPosition, duration).SetEase(Ease.OutQuad);
        openDoor = true;

        // Close the door after delay
        StartCoroutine(CloseDoorWithDelay(delayForDoorClose));
    }

    public void CloseDoor()
    {
        movingDoor.transform.DOMove(doorOriginalPosition, duration).SetEase(Ease.OutQuad);
        openDoor = false;
    }

    private IEnumerator CloseDoorWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        CloseDoor();
    }
}