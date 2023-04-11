using UnityEngine;
using System.Collections.Generic;
using System;

public enum WagonState
{
    NotMaintained,
    InProgress,
    Maintained
}

public enum TaskType
{
    Cleaning,
    CheckElectronics,
    RepairLamp,
}

[System.Serializable]
public class WagonTask
{
    public TaskType taskType;
    public bool isDone;
}

public class WagonStateController : MonoBehaviour
{
    public WagonState wagonState = WagonState.NotMaintained;
    public List<WagonTask> tasks = new List<WagonTask>();

    private bool isTasksInitialized = false;
    private TrainController trainController;


    private void Start()
    {
        trainController = transform.parent.GetComponent<TrainController>();
    }

    private void Update()
    {
        if (!isTasksInitialized)
        {
            InitializeTasks();
            isTasksInitialized = true;
        }

        bool isAllTasksDone = true;

        foreach (WagonTask task in tasks)
        {
            if (!task.isDone)
            {
                isAllTasksDone = false;
                break;
            }
        }

        if (isAllTasksDone && wagonState == WagonState.InProgress)
        {
            wagonState = WagonState.Maintained;
            Debug.Log("Wagon " + gameObject.name + " is maintained.");
        }
        else if (!isAllTasksDone && wagonState == WagonState.Maintained)
        {
            wagonState = WagonState.InProgress;
            Debug.Log("Wagon " + gameObject.name + " is in progress.");
        }

        if (trainController != null && trainController.trainState == TrainController.TrainState.Maintained && areAllWagonsMaintained())
        {
            trainController.trainState = TrainController.TrainState.Maintained;
            Debug.Log("Train is maintained.");
        }

        // Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MarkTaskAsDone(TaskType.Cleaning);
        }
    }

    private bool areAllWagonsMaintained()
    {
        foreach (Transform wagon in transform.parent)
        {
            WagonStateController wagonStateController = wagon.GetComponent<WagonStateController>();
            if (wagonStateController != null && wagonStateController.wagonState != WagonState.Maintained)
            {
                return false;
            }
        }

        return true;
    }

    private void InitializeTasks()
    {
        List<TaskType> availableTasks = new List<TaskType>((TaskType[])Enum.GetValues(typeof(TaskType)));
        int numTasks = UnityEngine.Random.Range(1, Mathf.Min(5, availableTasks.Count));

        for (int i = 0; i < numTasks; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableTasks.Count);
            TaskType taskType = availableTasks[randomIndex];
            availableTasks.RemoveAt(randomIndex);
            tasks.Add(new WagonTask { taskType = taskType, isDone = false });
        }
    }
    public void MarkTaskAsDone(TaskType taskType)
    {
        foreach (WagonTask task in tasks)
        {
            if (task.taskType == taskType)
            {
                task.isDone = true;

                switch (taskType)
                {
                    case TaskType.Cleaning:
                        Debug.Log("Wagon " + gameObject.name + ": Task A is done.");
                        break;
                    case TaskType.CheckElectronics:
                        Debug.Log("Wagon " + gameObject.name + ": Task B is done.");
                        break;
                    case TaskType.RepairLamp:
                        Debug.Log("Wagon " + gameObject.name + ": Task C is done.");
                        break;
                }

                break;
            }
        }
    }
}