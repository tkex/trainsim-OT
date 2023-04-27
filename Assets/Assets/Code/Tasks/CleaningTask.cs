using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningTask : BaseTask
{
    private GameObject cleaningObject;

    public override void StartTask()
    {
        // Spawn the cleaning object.
        SpawnCleaningObject();
    }

    private void SpawnCleaningObject()
    {
        // Spawn an empty game object at the wagon's position.
        cleaningObject = new GameObject("CleaningObject");
        cleaningObject.transform.position = transform.position;

        // Set spawned empty game object as children of the current wagon.
        cleaningObject.transform.SetParent(transform);
    }

    public override bool IsTaskDone()
    {
        // Check if the cleaning object has been destroyed.
        return cleaningObject == null;
    }
}