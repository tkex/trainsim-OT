using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UltimateXR.Manipulation;

[CreateAssetMenu(fileName = "CleaningInteriorTask", menuName = "Task/Cleaning Interior Task")]
public class CleaningInteriorTask : WagonTask
{

    private CleaningInteriorOnDestroyEvent cleaningObject;

    [SerializeField]
    public CleaningInteriorTask()
    {
        TaskType = TaskType.CleanInterior;
        IsDone = false;
    }


    // Method that gets called when the cleaning object is destroyed (and thus will mark the task as complete)
    private void OnDestroyed()
    {
        Debug.Log("Cleaning interior object has been destroyed.");

        // Mark task as done
        HandleTask();
    }


    public override void HandleTask()
    {

        // Make sure that wagonTaskHandling is not null and currentPlayerWagon is set
        if (wagonTaskHandling == null || wagonTaskHandling.currentPlayerWagon == null)
            return;

        // If this task is not associated with the current wagon, cancel
        if (associatedWagon != wagonTaskHandling.currentPlayerWagon)
            return;

        Debug.Log("Cleaning Interior Task HandleTask called");

        IsDone = true;
        Debug.Log("Cleaning Task task is now done.");

        //CompleteTask();       
    }

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
        // Offset of the GameObject
        Vector3 offset = new Vector3(0.85f, 1.27f, -3.7f);

        // Offset for rotation
        Vector3 rotationAngles = new Vector3(-90f, 0f, -60f);

        // Instantiate GameObject with parent Transform
        GameObject cleaningInteriorObject = GameObject.Instantiate(go, parentTransform);

        // Adjust position relative to the parent Transform
        cleaningInteriorObject.transform.position = parentTransform.TransformPoint(offset);

        // Adjust rotation relative to the parent Transform
        cleaningInteriorObject.transform.rotation = Quaternion.Euler(rotationAngles);

        // Subscribe to the OnDestroyed event of the CleaningObject component
        cleaningObject = cleaningInteriorObject.GetComponent<CleaningInteriorOnDestroyEvent>();

        if (cleaningObject != null)
        {
            Debug.Log("CleaningObject script found on the spawned interior object");
            cleaningObject.OnDestroyed += OnDestroyed;
        }
        else
        {
            Debug.LogError("CleaningObject script not on the spawned interior object");
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnDestroyed event
        if (cleaningObject != null)
        {
            cleaningObject.OnDestroyed -= OnDestroyed;
        }
    }
}