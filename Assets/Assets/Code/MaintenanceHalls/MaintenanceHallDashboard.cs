using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaintenanceHallDashboard : MonoBehaviour
{
    public MaintenanceHallsController maintenanceHalls;
    public TextMeshProUGUI maintenanceHallDashboardText;

    private void Update()
    {
        // Clear the dashboard text
        maintenanceHallDashboardText.text = "";

        // Loop through all maintenance halls
        foreach (MaintenanceHall hall in maintenanceHalls.maintenanceHalls)
        {
            // Display the maintenance hall name and status information
            maintenanceHallDashboardText.text += $"<b><color=#FFD700>{hall.maintenanceType} Maintenance Hall</color></b>\n";
            maintenanceHallDashboardText.text += $"Occupied: {hall.isOccupied}\n";
            maintenanceHallDashboardText.text += $"Wagon inside: {hall.hasWagon}\n";
            maintenanceHallDashboardText.text += $"Maintenance duration: {hall.maintenanceTimeLength} seconds\n";

            // Display remaining maintenance time if both occupied and has wagon are set
            if (hall.isOccupied && hall.hasWagon)
            {
                float remainingTime = hall.maintenanceTimeLength - hall.timer;
                maintenanceHallDashboardText.text += $"Time left: {remainingTime:F1} seconds\n";
            }

            maintenanceHallDashboardText.text += "\n";
        }
    }
}