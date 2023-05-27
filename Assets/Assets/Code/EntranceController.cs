using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EntranceController : MonoBehaviour
{

    [Header("Door Settings")]
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private DoorType doorType;
    [SerializeField] private float delayForDoorClose = 1f;
    [SerializeField] private float duration = 3f;
    [SerializeField] private float distance = 3f;

    private Vector3 leftDoorOriginalPosition;
    private Vector3 rightDoorOriginalPosition;
    private Vector3 leftDoorTargetPosition;
    private Vector3 rightDoorTargetPosition;
    private bool openDoor = false;

    private void Start()
    {
        leftDoorOriginalPosition = leftDoor.transform.position;
        rightDoorOriginalPosition = rightDoor.transform.position;

        leftDoorTargetPosition = leftDoorOriginalPosition + Vector3.left * distance;
        rightDoorTargetPosition = rightDoorOriginalPosition + Vector3.right * distance;
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
                TrainController.OnTrainDeparting += CloseDoor;
                break;
            case DoorType.Exit:
                TrainController.OnTrainDeparting += OpenDoor;
                TrainController.OnTrainArriving += CloseDoor;
                break;
        }
    }

    private void OnDisable()
    {
        switch (doorType)
        {
            case DoorType.Entrance:
                TrainController.OnTrainArriving -= OpenDoor;
                TrainController.OnTrainDeparting -= CloseDoor;
                break;
            case DoorType.Exit:
                TrainController.OnTrainDeparting -= OpenDoor;
                TrainController.OnTrainArriving -= CloseDoor;
                break;
        }
    }

    public void OpenDoor()
    {
        leftDoor.transform.DOMove(leftDoorTargetPosition, duration).SetEase(Ease.OutQuad);
        rightDoor.transform.DOMove(rightDoorTargetPosition, duration).SetEase(Ease.OutQuad);
        openDoor = true;

        // Close the door after delay
        StartCoroutine(CloseDoorWithDelay(delayForDoorClose));
    }

    public void CloseDoor()
    {
        leftDoor.transform.DOMove(leftDoorOriginalPosition, duration).SetEase(Ease.OutQuad);
        rightDoor.transform.DOMove(rightDoorOriginalPosition, duration).SetEase(Ease.OutQuad);
        openDoor = false;
    }

    private IEnumerator CloseDoorWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        CloseDoor();
    }
}

/*
 *  Old code for one door that goes up vertically.
 * 
 * 
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
            TrainController.OnTrainDeparting += CloseDoor;
            break;
        case DoorType.Exit:
            TrainController.OnTrainDeparting += OpenDoor;
            TrainController.OnTrainArriving += CloseDoor;
            break;
    }
}

private void OnDisable()
{
    switch (doorType)
    {
        case DoorType.Entrance:
            TrainController.OnTrainArriving -= OpenDoor;
            TrainController.OnTrainDeparting -= CloseDoor;
            break;
        case DoorType.Exit:
            TrainController.OnTrainDeparting -= OpenDoor;
            TrainController.OnTrainArriving -= CloseDoor;
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
*/


