using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;

[CreateAssetMenu(fileName = "FireExtinguisherTask", menuName = "Task/Fire Extinguisher Task")]
[System.Serializable]
public class FireExtinguisherTask : WagonTask
{

    private bool isAnchorOccupied = false;
    // Name of the fire extinguisher GameObject
    private string fireExtinguisherName = "FireExtinguisher";


    [SerializeField]
    public FireExtinguisherTask()
    {
        TaskType = TaskType.FireExtinguisher;
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
        if (e.GrabbableObject.name == fireExtinguisherName)
        {
            isAnchorOccupied = true;
            Debug.Log($"Fire Extinguisher was placed on anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");

            // Once fire ext. is placed, execute handle function
            HandleTask();
        }
    }

    private void UxrGrabManager_ObjectRemoved(object sender, UxrManipulationEventArgs e)
    {
        if (e.GrabbableObject.name == fireExtinguisherName)
        {
            isAnchorOccupied = false;
            Debug.Log($"Fire Extinguisher was removed from anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");
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

        Debug.Log("Fire Extinguisher Task HandleTask called");


        if (isAnchorOccupied)
        {
            Debug.Log("Handling Fire Extinguisher Task");
            IsDone = true;
            Debug.Log("Fire Extinguisher Task task is now done.");

            CompleteTask();
        }
        else
        {
            Debug.Log("Fire Extinguisher Task can't be handled because the fire extinguisher is not placed yet.");
        }
    }

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
        // Offset of the GameObject
        Vector3 offset = new Vector3(-1f, 1.08f, 7.34f);

        // Instantiate GameObject with parent Transform
        GameObject fireExtinguisherAnchorObject = GameObject.Instantiate(go, parentTransform);

        // Adjust position relative to the parent Transform
        fireExtinguisherAnchorObject.transform.position = parentTransform.TransformPoint(offset);
    }
}