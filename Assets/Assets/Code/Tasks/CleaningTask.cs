using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UltimateXR.Manipulation;

// CreateAssetMenu to enable creating manually ScriptableObjects (only needed for that) so can be dragged into the inspector for manual task assignment
// Project windows -> Create -> Task -> CleaningTask
[CreateAssetMenu(fileName = "CleaningTask", menuName = "Task/Cleaning Task")]
public class CleaningTask : WagonTask
{

    private bool isAnchorOccupied = true;

    // Name of the bottle GameObject
    private string cleaningName = "ClubMateBottleFixed";


    [SerializeField]
    public CleaningTask()
    {
        TaskType = TaskType.Cleaning;
        IsDone = false;
    }

    private void OnEnable()
    {
        UxrGrabManager.Instance.ObjectPlaced += UxrGrabManager_ObjectPlaced;
        UxrGrabManager.Instance.ObjectRemoved += UxrGrabManager_ObjectRemoved;
    }

    private void OnDisable()
    {
        UxrGrabManager.Instance.ObjectPlaced -= UxrGrabManager_ObjectPlaced;
        UxrGrabManager.Instance.ObjectRemoved -= UxrGrabManager_ObjectRemoved;
    }

    private void UxrGrabManager_ObjectPlaced(object sender, UxrManipulationEventArgs e)
    {
        if (e.GrabbableObject.name == cleaningName)
        {
            isAnchorOccupied = true;
            Debug.Log($"Bottle was placed on anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");
        }
    }

    private void UxrGrabManager_ObjectRemoved(object sender, UxrManipulationEventArgs e)
    {
        if (e.GrabbableObject.name == cleaningName)
        {
            isAnchorOccupied = false;
            Debug.Log($"Bottle was removed from anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");

            // Once the cleaning object (Club Mate bottle) is placed, execute handle function
            HandleTask();
        }
    }

    public override void HandleTask()
    {

        // Make sure that wagonTaskHandling is not null and currentPlayerWagon is set
        if (wagonTaskHandling == null || wagonTaskHandling.currentPlayerWagon == null)
            return;

        // If this task is not associated with the current wagon, cancel
        if (associatedWagon != wagonTaskHandling.currentPlayerWagon)
            return;

        Debug.Log("Cleaning Task HandleTask called");


        if (!isAnchorOccupied)
        {
            Debug.Log("Handling Cleaning Task");
            IsDone = true;
            Debug.Log("Cleaning Task task is now done.");

            CompleteTask();
        }
        else
        {
            Debug.Log("Cleaning Task can't be handled because the bottle is not placed yet.");
        }
    }

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
        // Offset of the GameObject
        Vector3 offset = new Vector3(-1.111f, 1.83f, 3.1f);

        // Offset for rotation
        Vector3 rotationAngles = new Vector3(0f, 180f, 0f);

        // Instantiate GameObject with parent Transform
        GameObject cleaningBottleAnchorObject = GameObject.Instantiate(go, parentTransform);

        // Adjust position relative to the parent Transform
        cleaningBottleAnchorObject.transform.position = parentTransform.TransformPoint(offset);

        // Adjust rotation relative to the parent Transform
        cleaningBottleAnchorObject.transform.rotation = Quaternion.Euler(rotationAngles);
    }
}