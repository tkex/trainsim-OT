using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WagonTaskHandling : MonoBehaviour
{
    private WagonTaskAssigner wagonTaskAssigner;

    // Tasks that get spawned on a  wagon
    public GameObject refuelPrefab;
    public GameObject cleaningPrefab;

    private void Start()
    {
        // Get the WagonTaskAssigner component from the wagon
        wagonTaskAssigner = GetComponent<WagonTaskAssigner>();

        // Show what tasks each wagon has (shows only when random is selected)
        LogWagonTasks();

        // Spawn tasks at the start with initial train spawning
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
    public void SpawnTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            switch (task.TaskType)
            {
                case TaskType.Cleaning:
                    task.SpawnTaskObject(cleaningPrefab, transform);
                    Debug.Log(gameObject.name + " has a cleaning task!");
                    break;
                case TaskType.RefuelEngine:
                    task.SpawnTaskObject(refuelPrefab, transform);
                    Debug.Log(gameObject.name + " has a refuel task!");
                    break;
                // add more cases for other task types
                default:
                    Debug.LogWarning("Unknown task type: " + task.TaskType);
                    break;
            }
        }
    }

    public void HandleTasks()
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
