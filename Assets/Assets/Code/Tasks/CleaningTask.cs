using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// CreateAssetMenu to enable creating manually ScriptableObjects (only needed for that) so can be dragged into the inspector for manual task assignment
// Project windows -> Create -> Task -> CleaningTask
[CreateAssetMenu(fileName = "CleaningTask", menuName = "Task/Cleaning Task")]
public class CleaningTask : WagonTask
{
    public CleaningTask()
    {
        TaskType = TaskType.Cleaning;
    }

    public override void HandleTask()
    {
        Debug.Log("Handling cleaning task");
        IsDone = true;
        Debug.Log("Cleaning task is now done.");

        CompleteTask();
    }

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
       GameObject cleaningObject = GameObject.Instantiate(go, parentTransform);
    }
}