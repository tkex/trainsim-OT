using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelTask : IWagonTask
{
    public TaskType TaskType { get; } = TaskType.RefuelEngine;
    public bool IsDone { get; set; }

    public void CompleteTask()
    {
        Debug.Log("Refuel task is now done.");
    }

    public void HandleTask(GameObject wagonGameObject)
    {
        Debug.Log("Handling refuel task for wagon " + wagonGameObject.name);
        IsDone = true;
        CompleteTask();
    }

    public void SpawnTaskObject(GameObject wagonGameObject)
    {
        Debug.Log(wagonGameObject.name + " has a refuel task!");
    }
}