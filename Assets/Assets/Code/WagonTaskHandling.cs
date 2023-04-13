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

        // Spawn tasks
        SpawnTasks();
    }

    private void LogWagonTasks()
    {
        // Iterate through the tasks of the wagon and log them.
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            Debug.Log(gameObject.name + " has task(s): " + task.taskType.ToString() + ", isDone: " + task.isDone.ToString());
        }
    }


    private void SpawnTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            switch (task.taskType)
            {
                case TaskType.Cleaning:
                    SpawnCleaningObject();
                    Debug.Log(gameObject.name + " has a cleaning task!");
                    break;
            }
        }
    }

    private void SpawnCleaningObject()
    {
        // Spawn an empty game object at the wagon's position.
        GameObject cleaningObject = new GameObject("CleaningObject");
        cleaningObject.transform.position = transform.position;

        cleaningObject.transform.SetParent(transform);
    }
}