using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonTaskHandling : MonoBehaviour
{
    private WagonTaskAssigner wagonTaskAssigner;
    private GameObject cleaningObject;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        wagonTaskAssigner = GetComponent<WagonTaskAssigner>();
        LogWagonTasks();
        SpawnTasks();
    }

    private void LogWagonTasks()
    {
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
        cleaningObject = new GameObject("CleaningObject");
        cleaningObject.transform.position = transform.position;
        cleaningObject.transform.SetParent(transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandleCleaningTask();
        }
    }

    private void HandleCleaningTask()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (WagonTask task in wagonTaskAssigner.tasks)
            {
                if (task.taskType == TaskType.Cleaning && !task.isDone)
                {
                    task.isDone = true;
                    Debug.Log("Cleaning task is now done.");
                    Destroy(cleaningObject);
                    break;
                }
            }
        }
    }
}
