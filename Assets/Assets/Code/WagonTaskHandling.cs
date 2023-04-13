using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WagonTaskHandling : MonoBehaviour
{
    private WagonTaskAssigner wagonTaskAssigner;

    private void Start()
    {
        // Get the WagonTaskAssigner component from the wagon.
        wagonTaskAssigner = GetComponent<WagonTaskAssigner>();

        // Log the tasks of the wagon.
        LogWagonTasks();

        // Spawn tasks.
        SpawnTasks();
    }

    private void Update()
    {
        // Check for space input.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Iterate through the tasks of the wagon.
            foreach (WagonTask task in wagonTaskAssigner.tasks)
            {
                // Check if the task is a cleaning task and has not been done yet.
                if (task.taskType == TaskType.Cleaning && !task.isDone)
                {
                    // Set the isDone flag of the task to true.
                    task.isDone = true;
                    Debug.Log("Cleaning task is now done.");
                }
            }
        }
    }

    private void SpawnTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            switch (task.taskType)
            {
                // Cases and what happens when a specific task case has been found on the wagon.
                case TaskType.Cleaning:
                    SpawnCleaningObject();
                    Debug.Log(gameObject.name + " has a cleaning task!");
                    break;
            }
        }
    }

    // Helper function to see what tasks a wagon has.
    private void LogWagonTasks()
    {
        // Iterate through the tasks of the wagon and log them.
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            Debug.Log(gameObject.name + " has task(s): " + task.taskType.ToString() + ", isDone: " + task.isDone.ToString());
        }
    }

    private void SpawnCleaningObject()
    {
        // Spawn an empty game object at the wagon's position.
        GameObject cleaningObject = new GameObject("CleaningObject");
        cleaningObject.transform.position = transform.position;

        // Set spawned empty game object as children of the current wagon.
        cleaningObject.transform.SetParent(transform);
    }
}