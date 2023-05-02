using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class WagonTaskAssigner : MonoBehaviour
{

    [Header("WagonTaskAssigner Settings")]

    [Tooltip("List of tasks and their status.")]
    public List<WagonTask> tasks = new List<WagonTask>();
    [Tooltip("The max. possible task a wagon can have.")]
    public int maxNumberOfPossibleTask = 5;

    private bool isTasksInitialized = false;

    private void Start()
    {
        // Assign random tasks to the wagon
        //AssignTasksToWagon();
    }

    // Function to assign a single specific task to a wagon
    // Useage (from other class): 
    // WagonTask taskToAssign = new MyCustomTask(); (like Cleaning)
    // wagonTaskAssigner.AssignSpecificTaskToWagon(taskToAssign);
    public void AssignSpecificTaskToWagon(WagonTask task)
    {
        if (tasks.Count < maxNumberOfPossibleTask)
        {
            tasks.Add(task);
        }
        else
        {
            Debug.LogWarning("Cannot assign task: maximum number of possible tasks for this wagon has been reached.");
        }
    }

    // Function to assign various random tasks to a wagon
    public void AssignRandomTasksToWagon()
    {

        // If tasks have already been initialized, exit the method to avoid assigning tasks again
        if (isTasksInitialized)
        {
            return;
        }

        // Create a list of all possible task types by finding all non-abstract subclasses of WagonTask
        List<Type> taskTypes = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(WagonTask)) && !type.IsAbstract)
                {
                    taskTypes.Add(type);
                }
            }
        }

        // Clear the existing tasks list
        tasks.Clear();

        // Determine how many tasks to assign
        int numberOfTasks = UnityEngine.Random.Range(1, maxNumberOfPossibleTask + 1);

        // Randomly assign tasks until the desired number is reached
        for (int i = 0; i < numberOfTasks; i++)
        {
            // Randomly select a task type from the list of all possible task types
            if (taskTypes.Count == 0)
            {
                // If there are no more task types to choose from, exit the loop
                break;
            }
            int randomIndex = UnityEngine.Random.Range(0, taskTypes.Count);
            Type randomTaskType = taskTypes[randomIndex];
            taskTypes.RemoveAt(randomIndex);

            // Create an instance of the selected task type using Activator.CreateInstance()
            WagonTask randomTask = (WagonTask)Activator.CreateInstance(randomTaskType);

            // Add the selected task to the list of assigned tasks, with isDone set to false
            tasks.Add(randomTask);
        }

        // Mark tasks as initialized to avoid assigning them again
        isTasksInitialized = true;
    }

    // Function for clearing all tasks at once
    public void ClearAllTasks()
    {
        tasks.Clear();
    }

    // Function to show all tasks
    public void ShowAllTasks()
    {
        Debug.Log("Tasks assigned to this wagon:");

        foreach (WagonTask task in tasks)
        {
            Debug.Log("- " + task.name);
        }
    }
}

/*
 * 
 * ALTERNATIVE FOR SPECIFIC SUBCLASSES FOR ABSTRACT CLASS WAGONTASK INSTEAD ON ALL LIKE ABOVE
 * 
 * // Create a list of all possible task types
    List<Type> taskTypes = new List<Type>()
    {
    typeof(CleaningTask),
    typeof(RepairTask),
    typeof(DeliveryTask)
    };
 * 
 * 
 * // Determine how many tasks to assign
    int numberOfTasks = UnityEngine.Random.Range(1, maxNumberOfPossibleTask + 1);

    // Randomly assign tasks until the desired number is reached.
    for (int i = 0; i < numberOfTasks; i++)
    {
        // Randomly select a task type from the list of all possible task types.
        int randomIndex = UnityEngine.Random.Range(0, taskTypes.Count);
        Type randomTaskType = taskTypes[randomIndex];
        taskTypes.RemoveAt(randomIndex);

        // Create an instance of the selected task type using Activator.CreateInstance().
        WagonTask randomTask = (WagonTask)Activator.CreateInstance(randomTaskType);

        // Add the selected task to the list of assigned tasks, with isDone set to false.
        tasks.Add(randomTask);
    }
*/

// # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # 

// Old function before refactor (before using abstract class)
/*
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
        tasks.Add(new 
        {
            TaskType = randomTask,
            IsDone = false
        });
    }

    // Mark tasks as initialized to avoid assigning them again.
    isTasksInitialized = true;
}
*/
