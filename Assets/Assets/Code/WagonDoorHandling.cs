using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WagonDoorHandling : MonoBehaviour
{
    [Header("Wagon Door Settings")]

    [Tooltip("Left door of a wagon.")]
    [SerializeField] private GameObject leftDoor;

    [Tooltip("Right door of a wagon.")]
    [SerializeField] private GameObject rightDoor;

    [SerializeField] private float openingDuration = 4f;
    [SerializeField] private float distance = 0.95f;

    private Vector3 leftDoorOriginalPosition;
    private Vector3 rightDoorOriginalPosition;

    public void OpenDoors()
    {
        // Save the original positions
        leftDoorOriginalPosition = leftDoor.transform.position;
        rightDoorOriginalPosition = rightDoor.transform.position;

        // Opening left door
        leftDoor.transform.DOMove(new Vector3(leftDoorOriginalPosition.x, leftDoorOriginalPosition.y, leftDoorOriginalPosition.z + distance), openingDuration);

        // Opening right door
        rightDoor.transform.DOMove(new Vector3(rightDoorOriginalPosition.x, rightDoorOriginalPosition.y, rightDoorOriginalPosition.z - distance), openingDuration);
    }

    public void CloseDoors()
    {
        // Closing left door
        leftDoor.transform.DOMove(leftDoorOriginalPosition, openingDuration);

        // Closing right door
        rightDoor.transform.DOMove(rightDoorOriginalPosition, openingDuration);
    }
}