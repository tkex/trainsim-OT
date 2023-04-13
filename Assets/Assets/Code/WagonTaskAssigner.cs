using UnityEngine;
using System.Collections.Generic;
using System;


[System.Serializable]
public class WagonTask
{
    public TaskType taskType;
    public bool isDone;
    public event Action<WagonTask> TaskCompleted;

    public void CompleteTask()
    {
        isDone = true;
        TaskCompleted?.Invoke(this);
    }
}

public class WagonTaskAssigner : MonoBehaviour
{

    [Header("WagonTaskAssigner Settings")]

    [Tooltip("The maintenance state of the wagon.")]
    public WagonStates wagonState;
    [Tooltip("List of tasks and their status.")]
    public List<WagonTask> tasks = new List<WagonTask>();
    [Tooltip("The max. possible task a wagon can have.")]
    public int maxNumberOfPossibleTask = 6;

    private bool isTasksInitialized = false;

    private void Start()
    {
        // Set default maintenance state of wagon as not maintained.
        wagonState =  WagonStates.NotMaintained;

        // Assign random tasks to the wagon.
        AssignTasksToWagon();
    }

    private void Update()
    {
        // Check if all tasks are completed and update the wagon state accordingly.
        bool allTasksCompleted = true;
        foreach (WagonTask task in tasks)
        {
            if (!task.isDone)
            {
                allTasksCompleted = false;
                break;
            }
        }
        if (allTasksCompleted)
        {
            switch (wagonState)
            {
                case WagonStates.NotMaintained:
                    wagonState = WagonStates.Maintained;
                    break;
                case WagonStates.InProgress:
                    wagonState = WagonStates.Maintained;
                    break;
            }
        }
        else
        {
            switch (wagonState)
            {
       
                case WagonStates.NotMaintained:
                    wagonState = WagonStates.InProgress;
                    break;
            }
        }
    }


    // This method assigns between 1 and 5 tasks to the wagon and sets their isDone field to false.
    public void AssignTasksToWagon()
    {
        // If tasks have already been initialized, exit the method to avoid assigning tasks again.
        if (isTasksInitialized)
        {
            return;
        }

        // Create a list of all possible tasks by iterating through the TaskType enumeration.
        List<TaskType> allTasks = new List<TaskType>();
        foreach (TaskType taskType in Enum.GetValues(typeof(TaskType)))
        {
            allTasks.Add(taskType);
        }

        // Clear the existing tasks list.
        tasks.Clear();

        // Determine how many tasks to assign.
        int numberOfTasks = UnityEngine.Random.Range(1, maxNumberOfPossibleTask + 1);

        // Randomly assign tasks until the desired number is reached.
        for (int i = 0; i < numberOfTasks; i++)
        {
            // Randomly select a task from the list of all possible tasks.
            int randomIndex = UnityEngine.Random.Range(0, allTasks.Count);
            TaskType randomTask = allTasks[randomIndex];
            allTasks.RemoveAt(randomIndex);

            // Add the selected task to the list of assigned tasks, with isDone set to false.
            tasks.Add(new WagonTask
            {
                taskType = randomTask,
                isDone = false
            });
        }

        // Mark tasks as initialized to avoid assigning them again.
        isTasksInitialized = true;

        // Subscribe to the TaskCompleted event for each task.
        foreach (WagonTask task in tasks)
        {
            task.TaskCompleted += OnTaskCompleted;
        }
    }

    private void OnTaskCompleted(WagonTask completedTask)
    {
        // Check if all tasks are completed.
        bool allTasksCompleted = true;
        foreach (WagonTask task in tasks)
        {
            if (!task.isDone)
            {
                allTasksCompleted = false;
                break;
            }
        }

        if (allTasksCompleted)
        {
            wagonState = WagonStates.Maintained;
        }
        else
        {
            wagonState = WagonStates.InProgress;
        }
    }
}