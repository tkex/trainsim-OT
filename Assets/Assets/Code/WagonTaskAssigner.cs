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
    Cleaning,           // Wagon reinigen
    CheckBrakes,        // Bremsen überprüfen
    InspectCouplers,    // Kupplungen inspizieren
    RepairLighting,     // Beleuchtung reparieren
    CleanInterior,      // Innenraum reinigen
    LubricateDoors,     // Türen schmieren
    RefuelEngine,       // Motor betanken
}

[System.Serializable]
public class WagonTask
{
    public TaskType taskType;
    public bool isDone;
}

public class WagonTaskAssigner : MonoBehaviour
{
    public WagonState wagonState;
    public List<WagonTask> tasks = new List<WagonTask>();

    private bool isTasksInitialized = false;

    private TrainController trainController;


    private void Start()
    {
        trainController = transform.parent.GetComponent<TrainController>();
        wagonState =  WagonState.NotMaintained;

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
        int numberOfTasks = UnityEngine.Random.Range(1, 6);

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
    }
}