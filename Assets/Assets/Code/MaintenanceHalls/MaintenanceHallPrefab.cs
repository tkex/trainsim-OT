using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaintenanceHallPrefab : MonoBehaviour
{
    // Type of maintenance needed for the hall
    public TaskType maintenanceType;

    // Flag to indicate if the maintenance hall is currently occupied or not with a wagon
    public bool isOccupied = false;

    // Timer for maintenance
    private float timer = 0f;

    // Length of the timer in seconds
    [SerializeField] private float maintenanceTimeLength = 10f;

    public void UpdateTimer()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                isOccupied = false;
            }
        }
    }

    public void StartMaintenance()
    {
        timer = maintenanceTimeLength;
        isOccupied = true;
    }

    public float GetRemainingTime()
    {
        return timer;
    }
}
