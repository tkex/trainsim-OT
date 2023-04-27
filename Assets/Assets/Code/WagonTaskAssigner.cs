using UnityEngine;
using System.Collections.Generic;
using System;


[System.Serializable]
public class WagonTask : IWagonTask
{
    [SerializeField] private TaskType taskType;
    [SerializeField] private bool isDone;

    public event Action<WagonTask> TaskCompleted;

    public TaskType TaskType
    {
        get { return taskType; }
        set { taskType = value; }
    }

    public bool IsDone
    {
        get { return isDone; }
        set { isDone = value; }
    }

    public void CompleteTask()
    {
        IsDone = true;
        TaskCompleted?.Invoke(this);
    }

    public void HandleTask(GameObject wagonGameObject)
    {
        Debug.Log($"Handling {TaskType} task for wagon {wagonGameObject.name}");
    }

    public void SpawnTaskObject(GameObject wagonGameObject)
    {
        GameObject taskObject = new GameObject("TaskObject");
        taskObject.transform.position = wagonGameObject.transform.position;
        taskObject.transform.SetParent(wagonGameObject.transform);
    }
}

public class WagonTaskAssigner : MonoBehaviour
{

    [Header("WagonTaskAssigner Settings")]
    
    [Tooltip("List of tasks and their status.")]
    public List<WagonTask> tasks = new List<WagonTask>();
    [Tooltip("The max. possible task a wagon can have.")]
    public int maxNumberOfPossibleTask = 6;

    private bool isTasksInitialized = false;

    private void Start()
    {
        // Assign random tasks to the wagon.
        AssignTasksToWagon();
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
                TaskType = randomTask,
                IsDone = false
            });
        }

        // Mark tasks as initialized to avoid assigning them again.
        isTasksInitialized = true;    
    }
}