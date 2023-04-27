using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an interface for wagon tasks (handled in WagonTaskHandling)
public interface IWagonTask
{
    // Prop to get the type of the task
    public TaskType TaskType { get; }
    // Prop to check if the task is done
    public bool IsDone { get; set; }
    // Method to complete the task
    void CompleteTask();
    // Method to handle the task
    void HandleTask(GameObject wagonGameObject);
    // Method to spawn a task object
    void SpawnTaskObject(GameObject wagonGameObject);
}