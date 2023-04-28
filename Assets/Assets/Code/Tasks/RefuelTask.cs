using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RefuelTask : WagonTask
{
    public RefuelTask()
    {
        TaskType = TaskType.RefuelEngine;
    }

    public override void HandleTask()
    {
        Debug.Log("Handling refuel engine task");
        IsDone = true;
        Debug.Log("Refuel Engine task is now done.");

        CompleteTask();
    }

    public override void SpawnTaskObject(GameObject go)
    {
        GameObject cleaningObject = GameObject.Instantiate(go, go.transform.position, go.transform.rotation);
    }
}