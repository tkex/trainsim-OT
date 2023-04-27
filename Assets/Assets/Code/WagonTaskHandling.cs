using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonTaskHandling : MonoBehaviour
{
    public WagonTaskAssigner wagonTaskAssigner;
    public List<IWagonTask> tasks = new List<IWagonTask>();

    private void Start()
    {

        // Get the WagonTaskAssigner component from the wagon
        wagonTaskAssigner = GetComponent<WagonTaskAssigner>();

        // Add each WagonTask instance as an IWagonTask to the tasks list
        foreach (WagonTask wagonTask in wagonTaskAssigner.tasks)
        {
            tasks.Add(wagonTask);
        }

        // Log all tasks of the wagon
        LogWagonTasks();

        // Spawn the tasks in wagons
        SpawnTasks();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandleTasks();
        }
    }

    private void HandleTasks()
    {
        foreach (IWagonTask task in tasks)
        {
            if (!task.IsDone)
            {
                task.HandleTask(gameObject);
            }
        }
    }

    private void SpawnTasks()
    {
        foreach (IWagonTask task in tasks)
        {
            task.SpawnTaskObject(gameObject);
        }
    }

    private void LogWagonTasks()
    {
        foreach (IWagonTask task in tasks)
        {
            Debug.Log(gameObject.name + " has task(s): " + task.TaskType + ", isDone: " + task.IsDone);
        }
    }
}