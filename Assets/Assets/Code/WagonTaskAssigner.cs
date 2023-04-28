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
    public int maxNumberOfPossibleTask = 6;

    private bool isTasksInitialized = false;

    // Create a list of all possible task types.
    List<Type> taskTypes = new List<Type>()
    {
        typeof(CleaningTask),
        typeof(RefuelTask),
    };


    private void Start()
    {
        // Assign random tasks to the wagon.
        AssignTasksToWagon();
    }

    public void AssignTasksToWagon()
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

        // Clear the existing tasks list.
        tasks.Clear();

        // Determine how many tasks to assign.
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
