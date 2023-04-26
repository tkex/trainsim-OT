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

    // Control flag to indicate if the maintenance hall is currently occupied or not with a wagon
    public bool isOccupied;
}

public class MaintenanceHallsController : MonoBehaviour
{
    [Header("Prefab Settings")]
    [Tooltip("Drag in the maintenance hall prefab that shall be spawned.")]
    // Prefab of the maintenance hall
    public GameObject maintenanceHallPrefab;

    // List of maintenance halls to spawn
    [SerializeField] private List<MaintenanceHall> maintenanceHalls;

    [Header("Position Settings")]
    [Tooltip("Set the starting spawn position of the first maintenance hall.")]
    // Starting position of the first maintenance hall
    [SerializeField] private Vector3 startingSpawnPosition;

    [Tooltip("The X distance between each maintenance hall.")]
    // Distance between each maintenance hall in the X direction
    [SerializeField] private float distanceBetweenHallsX = 10f;

    [Tooltip("The Z distance between each maintenance hall.")]
    // Distance between each maintenance hall in the Y direction
    [SerializeField] private float distanceBetweenHallsZ = 10f;



    private void Start()
    {
        // Spawn maintenance halls
        SpawnMaintenanceHalls();
    }

    private void SpawnMaintenanceHalls()
    {
        Vector3 spawnPosition = startingSpawnPosition;

        foreach (MaintenanceHall hall in maintenanceHalls)
        {
            // Instantiate maintenance hall prefab
            GameObject hallObject = Instantiate(maintenanceHallPrefab, spawnPosition, Quaternion.identity);

            // Set isOccupied flag to false initially
            hall.isOccupied = false;

            // Update spawn position for next maintenance hall
            spawnPosition += new Vector3(distanceBetweenHallsX, 0, distanceBetweenHallsZ);
        }
    }
}
