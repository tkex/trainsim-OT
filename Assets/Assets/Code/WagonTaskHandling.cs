using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WagonTaskHandling : MonoBehaviour
{
    private WagonTaskAssigner wagonTaskAssigner;

    private void Start()
    {
        wagonTaskAssigner = GetComponent<WagonTaskAssigner>();

        LogWagonTasks();
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
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            if (!task.IsDone)
            {
                switch (task.TaskType)
                {
                    case TaskType.Cleaning:
                        HandleCleaningTask(task);
                        break;
                    case TaskType.RefuelEngine:
                        HandleRefuelTask(task);
                        break;
                    // add more cases for other task types
                    default:
                        Debug.LogWarning("Unknown task type: " + task.TaskType);
                        break;
                }
            }
        }
    }

    private void HandleCleaningTask(WagonTask task)
    {
        Debug.Log("Handling cleaning task for wagon " + gameObject.name);
        task.IsDone = true;
        Debug.Log("Cleaning task is now done.");
        task.CompleteTask();
    }

    private void HandleRefuelTask(WagonTask task)
    {
        Debug.Log("Handling refuel task for wagon " + gameObject.name);
        task.IsDone = true;
        Debug.Log("Refuel task is now done.");
        task.CompleteTask();
    }

    private void SpawnTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            switch (task.TaskType)
            {
                case TaskType.Cleaning:
                    SpawnCleaningObject();
                    Debug.Log(gameObject.name + " has a cleaning task!");
                    break;
                case TaskType.RefuelEngine:
                    Debug.Log(gameObject.name + " has a refuel task!");
                    break;
                // add more cases for other task types
                default:
                    Debug.LogWarning("Unknown task type: " + task.TaskType);
                    break;
            }
        }
    }

    private void SpawnCleaningObject()
    {
        GameObject cleaningObject = new GameObject("CleaningObject");
        cleaningObject.transform.position = transform.position;
        cleaningObject.transform.SetParent(transform);
    }

    private void LogWagonTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            Debug.Log(gameObject.name + " has task(s): " + task.TaskType + ", isDone: " + task.IsDone);
        }
    }
}
