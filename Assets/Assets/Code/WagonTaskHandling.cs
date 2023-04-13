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
    }

    private void LogWagonTasks()
    {
        // Iterate through the tasks of the wagon and log them.
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            Debug.Log(gameObject.name + " has task(s): " + task.taskType.ToString() + ", isDone: " + task.isDone.ToString());
        }
    }
}