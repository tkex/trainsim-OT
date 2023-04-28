using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public override void SpawnTaskObject(GameObject go)
    {
        GameObject cleaningObject = GameObject.Instantiate(go, go.transform.position, go.transform.rotation);
    }
}