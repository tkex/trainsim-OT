using UnityEngine;
using System;


// This script manages the maintenance state of a wagon based on its tasks completion status
public class WagonTaskStateMachine : MonoBehaviour
{
    // The wagon task assigner component attached to the wagon
    private WagonTaskAssigner wagonTaskAssigner;

    // The maintenance state of the wagon
    [Tooltip("The maintenance state of the wagon.")]
    public WagonStates wagonState;

    // Observer pattern
    // Define an event that will be triggered whenever the wagon state changes
    public event Action<WagonStates> OnWagonStateChanged;

    // State colours
    [Header("Maintenance State Colours")]
    public Color notMaintainedColor = Color.red;
    public Color inProgressColor = Color.yellow;
    public Color maintainedColor = Color.green;
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();       
    }

    private void Start()
    {
        // Get the WagonTaskAssigner component from the same GameObject
        wagonTaskAssigner = GetComponent<WagonTaskAssigner>();

        // Set default maintenance state of wagon as not maintained.
        wagonState = WagonStates.NotMaintained;
    }

    private void Update()
    {
        // Check if the state needs to be updated
        if (wagonTaskAssigner.tasks.Count == 0 || !CheckIfAnyTasksCompleted())
        {
            // If no tasks or no task is completed, set wagon state as not maintained
            ChangeState(WagonStates.NotMaintained);

            renderer.material.color = notMaintainedColor;
        }
        else if (CheckIfAllTasksCompleted())
        {
            // If all tasks are completed, set wagon state as maintained
            ChangeState(WagonStates.Maintained);
            renderer.material.color = maintainedColor;
        }
        else if (wagonState != WagonStates.InProgress && CheckIfAnyTasksCompleted())
        {
            // If some tasks are completed, set wagon state as in progress
            ChangeState(WagonStates.InProgress);
            renderer.material.color = inProgressColor;
        }
    }

    private void ChangeState(WagonStates newState)
    {
        // Set wagon state
        wagonState = newState;

        // Trigger the OnWagonStateChanged event, passing the new state as a parameter
        OnWagonStateChanged?.Invoke(newState);

        // Do something with the new state (now in the observer) (to remove)
        //Debug.Log(gameObject.name + " state changed to: " + newState);
    }

    private bool CheckIfAllTasksCompleted()
    {
        // Check if all tasks are completed
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            if (!task.isDone)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckIfAnyTasksCompleted()
    {
        // Check if any task is completed
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            if (task.isDone)
            {
                return true;
            }
        }
        return false;
    }
}