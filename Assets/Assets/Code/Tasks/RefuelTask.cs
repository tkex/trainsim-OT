using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class RefuelTask : WagonTask
{
    [SerializeField]
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

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
        GameObject refuelObject = GameObject.Instantiate(go, parentTransform);
    }
}