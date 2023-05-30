using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;

[CreateAssetMenu(fileName = "MedkitTask", menuName = "Task/Medkit Task")]
[System.Serializable]
public class MedkitTask : WagonTask
{

    private bool isAnchorOccupied = false;
    // Name of the fire extinguisher GameObject
    private string medkitName = "Medkit";


    [SerializeField]
    public MedkitTask()
    {
        TaskType = TaskType.Medkit;
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
        if (e.GrabbableObject.name == medkitName)
        {
            isAnchorOccupied = true;
            Debug.Log($"Medkit was placed on anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");

            // Once the medkit is placed, execute handle function
            HandleTask();
        }
    }

    private void UxrGrabManager_ObjectRemoved(object sender, UxrManipulationEventArgs e)
    {
        if (e.GrabbableObject.name == medkitName)
        {
            isAnchorOccupied = false;
            Debug.Log($"Medkit was removed from anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");
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

        Debug.Log("Medkit Task HandleTask called");


        if (isAnchorOccupied)
        {
            Debug.Log("Handling Medkit Task");
            IsDone = true;
            Debug.Log("Medkit Task task is now done.");

            CompleteTask();
        }
        else
        {
            Debug.Log("Medkit Task can't be handled because the fire extinguisher is not placed yet.");
        }
    }

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
        // Offset of the GameObject
        Vector3 offset = new Vector3(-0.98f, 1.215f, -2.65f);

        // Offset for rotation
        Vector3 rotationAngles = new Vector3(90f, 0f, -90f);

        // Instantiate GameObject with parent Transform
        GameObject medkitAnchorObject = GameObject.Instantiate(go, parentTransform);

        // Adjust position relative to the parent Transform
        medkitAnchorObject.transform.position = parentTransform.TransformPoint(offset);

        // Adjust rotation relative to the parent Transform
        medkitAnchorObject.transform.rotation = Quaternion.Euler(rotationAngles);
    }
}

