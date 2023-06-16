using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MaintenanceHall
{
    // Type of maintenance needed for the hall
    public TaskType maintenanceType;

    // Length of the timer in seconds
    public float maintenanceTimeLength;

    // Flag to check if the maintenance hall is currently occupied or not with a wagon
    public bool isOccupied = false;

    // Flag to check if the maintenance hall has a wagon inside or not
    public bool hasWagon = false;

    // Timer for maintenance
    public float timer = 0f;
}


public class MaintenanceHallsController : MonoBehaviour
{
    [Header("Prefab Settings")]
    [Tooltip("Drag in the maintenance hall prefab that shall be spawned.")]
    // Prefab of the maintenance hall
    public GameObject maintenanceHallPrefab;

    // List of maintenance halls to spawn
    [SerializeField] public List<MaintenanceHall> maintenanceHalls;

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

    private void Update()
    {
        // Update maintenance timers
        foreach (MaintenanceHall hall in maintenanceHalls)
        {
            if (hall.hasWagon && hall.isOccupied)
            {
                hall.timer += Time.deltaTime;

                if (hall.timer >= hall.maintenanceTimeLength)
                {
                    Debug.Log(string.Format("{0} Maintenance Hall has completed maintenance.", hall.maintenanceType));

                    // TODO Remove wagon function later here
                    hall.isOccupied = false;

                    // Reset timer
                    hall.timer = 0f;
                }
            }
        }
    }

    private void SpawnMaintenanceHalls()
    {
        // Create parent object for maintenance halls
        GameObject maintenanceHallsParent = new GameObject("MaintenanceHalls");

        Vector3 spawnPosition = startingSpawnPosition;

        foreach (MaintenanceHall hall in maintenanceHalls)
        {
            // Instantiate maintenance hall prefab as child of parent object
            GameObject hallObject = Instantiate(maintenanceHallPrefab, spawnPosition, Quaternion.identity, maintenanceHallsParent.transform);

            // Set the name of the maintenance hall based on its maintenance type
            hallObject.name = string.Format("{0} Maintenance Hall", hall.maintenanceType);

            // Update spawn position for next maintenance hall
            spawnPosition += new Vector3(distanceBetweenHallsX, 0, distanceBetweenHallsZ);
        }
    } 
}
