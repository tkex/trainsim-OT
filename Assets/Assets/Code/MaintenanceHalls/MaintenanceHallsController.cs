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
    // Prefab of the maintenance hall
    public GameObject maintenanceHallPrefab;

    // List of maintenance halls to spawn
    [SerializeField] private List<MaintenanceHall> maintenanceHalls;

    // Starting position of the first maintenance hall
    [SerializeField] private Vector3 startingSpawnPosition;

    // Distance between each maintenance hall in the X direction
    [SerializeField] private float distanceBetweenHallsX = 10f;

    // Distance between each maintenance hall in the Y direction
    [SerializeField] private float distanceBetweenHallsY = 10f;



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
            spawnPosition += new Vector3(distanceBetweenHallsX, distanceBetweenHallsY, 0);
        }
    }
}
