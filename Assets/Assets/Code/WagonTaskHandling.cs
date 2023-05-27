using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WagonTaskHandling : MonoBehaviour
{
    private WagonTaskAssigner wagonTaskAssigner;

    // Tasks that get spawned on a wagon
    public GameObject refuelPrefab;
    public GameObject cleaningPrefab;

    public GameObject fireExtinguisherAnchorPrefab;

    public GameObject currentPlayerWagon;




    private void Start()
    {
        // Get the WagonTaskAssigner component from the wagon
        wagonTaskAssigner = GetComponent<WagonTaskAssigner>();
        //wagonTaskAssigner = currentPlayerWagon.GetComponent<WagonTaskAssigner>();

        // Show what tasks each wagon has (shows only when random is selected)
        LogWagonTasks();

        // Spawn tasks at the start with initial train spawning
        SpawnTasks();
    }


    // ***

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Aktualisiere die Referenz auf den aktuellen Wagon
            currentPlayerWagon = gameObject;
            //wagonTaskAssigner = currentPlayerWagon.GetComponent<WagonTaskAssigner>();
            Debug.Log("Entered " + currentPlayerWagon);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Setze die Referenz zurück, wenn der Spieler den Wagon verlässt
            currentPlayerWagon = null;
            //wagonTaskAssigner = null;
            Debug.Log("Exited " + currentPlayerWagon);
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject == currentPlayerWagon)
        {
            HandleTasks();
            Debug.Log("Inside");
        }
    }
    */

    // ***


    // Looks good!
    public void SpawnTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            // Assignment of the associated wagon
            task.associatedWagon = this.gameObject;
            task.wagonTaskHandling = this;

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
                case TaskType.FireExtinguisher:
                    task.SpawnTaskObject(fireExtinguisherAnchorPrefab, transform);
                    Debug.Log(gameObject.name + " has a fire extinguisher task!");
                    break;
                // Add more cases for other task types
                default:
                    Debug.LogWarning("Unknown task type: " + task.TaskType);
                    break;
            }
        }
    }

    /*
    public void HandleTasks()
    {
        // Überprüfung, ob der Wagon der currentPlayerWagon ist
        if (currentPlayerWagon != null) return;


        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            if (!task.IsDone)
            {
                switch (task.TaskType)
                {
                    case TaskType.Cleaning:
                        //task.HandleTask();
                        break;
                    case TaskType.RefuelEngine:
                        //task.HandleTask();
                        break;
                    case TaskType.FireExtinguisher:
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
    */

    private void LogWagonTasks()
    {
        foreach (WagonTask task in wagonTaskAssigner.tasks)
        {
            Debug.Log(gameObject.name + " has task(s): " + task.TaskType + ", isDone: " + task.IsDone);
        }
    }
}
