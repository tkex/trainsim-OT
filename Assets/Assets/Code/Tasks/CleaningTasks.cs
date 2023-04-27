using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningTask : IWagonTask
{
    // The type of task that this class implements
    public TaskType TaskType { get; } = TaskType.Cleaning;

    // Boolean flag that check whether the task has been completed
    public bool IsDone { get; set; }

    // Method to complete the task
    public void CompleteTask()
    {
        Debug.Log("Cleaning task is now done.");
    }

    // Method to handle the cleaning task for a given wagon game object
    public void HandleTask(GameObject wagonGameObject)
    {
        Debug.Log("Handling cleaning task for wagon " + wagonGameObject.name);
        IsDone = true;
        CompleteTask();
    }

    // Method to spawn a cleaning object for a given wagon game object
    public void SpawnTaskObject(GameObject wagonGameObject)
    {
        GameObject cleaningObject = new GameObject("CleaningObject");
        cleaningObject.transform.position = wagonGameObject.transform.position;
        cleaningObject.transform.SetParent(wagonGameObject.transform);
    }
}