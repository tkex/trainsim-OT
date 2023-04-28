using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WagonTaskHandling : MonoBehaviour
{
    private WagonTaskAssigner wagonTaskAssigner;

    public GameObject cleaningPrefab;

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

    // Looks good!
    private void SpawnTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            switch (task.TaskType)
            {
                case TaskType.Cleaning:
                    task.SpawnTaskObject(cleaningPrefab);
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

    private void HandleTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            if (!task.IsDone)
            {
                switch (task.TaskType)
                {
                    case TaskType.Cleaning:
                        //HandleCleaningTask(task);
                        task.HandleTask();
                        break;
                    case TaskType.RefuelEngine:
                        task.HandleTask();
                        //HandleRefuelTask(task);
                        break;
                    // add more cases for other task types
                    default:
                        Debug.LogWarning("Unknown task type: " + task.TaskType);
                        break;
                }
            }
        }
    }



    // Looks good!
    private void LogWagonTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            Debug.Log(gameObject.name + " has task(s): " + task.TaskType + ", isDone: " + task.IsDone);
        }
    }
}
