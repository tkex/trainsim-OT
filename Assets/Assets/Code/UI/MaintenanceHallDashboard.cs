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
            maintenanceHallDashboardText.text += $"<b><color=#FFD700>{hall.maintenanceType} Wartungshalle</color></b>\n";
            maintenanceHallDashboardText.text += $"Beschäftigt: {hall.isOccupied}\n";
            maintenanceHallDashboardText.text += $"Wagon drinne: {hall.hasWagon}\n";
            maintenanceHallDashboardText.text += $"Dauer der Wartung: {hall.maintenanceTimeLength} Sekunden\n";

            // Display remaining maintenance time if both occupied and has wagon are set
            if (hall.isOccupied && hall.hasWagon)
            {
                float remainingTime = hall.maintenanceTimeLength - hall.timer;
                maintenanceHallDashboardText.text += $"Restzeit: {remainingTime:F1} Sekunden\n";
            }

            maintenanceHallDashboardText.text += "\n";
        }
    }
}