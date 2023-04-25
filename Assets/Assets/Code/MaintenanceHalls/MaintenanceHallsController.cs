using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MaintenanceHall
{
    // Type of maintenance needed for the hall
    public TaskType maintenanceType;

    // Length of the timer in seconds
    [SerializeField] private float maintenanceTimeLength = 10f;
}

public class MaintenanceHallsController : MonoBehaviour
{
    // Prefab of the maintenance hall
    public GameObject maintenanceHallPrefab;

    // List of maintenance halls to spawn
    [SerializeField] private List<MaintenanceHall> maintenanceHalls;

    // Distance between each maintenance hall
    [SerializeField] private float distanceBetweenHalls = 10f;

    private void Start()
    {
        // Spawn maintenance halls
        SpawnMaintenanceHalls();
    }

    private void SpawnMaintenanceHalls()
    {
        // Starting position of the first maintenance hall
        Vector3 spawnPosition = transform.position;

        foreach (MaintenanceHall hall in maintenanceHalls)
        {
            // Instantiate maintenance hall prefab
            GameObject hallObject = Instantiate(maintenanceHallPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
