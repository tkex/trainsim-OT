using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskDashboardController : MonoBehaviour
{
    public TrainController trainController;
    public TextMeshProUGUI dashboardText;

    // Dictionary to map enum values to descriptions (used only for the dashboard)
    private readonly Dictionary<TaskType, string> taskDescriptions = new Dictionary<TaskType, string>()
    {
        { TaskType.Cleaning, "Wagen reinigen" },
        { TaskType.CheckBrakes, "Bremsen prüfen" },
        { TaskType.InspectCouplers, "Kupplung inspizieren" },
        { TaskType.RepairLighting, "Beleuchtung reparieren" },
        { TaskType.CleanInterior, "Innenraum reinigen" },
        { TaskType.LubricateDoors, "Türen schmieren" },
        { TaskType.RefuelEngine, "Motor betanken" },
        // Add more mappings as needed (see enums)
    };

    private void Update()
    {
        // Clear the dashboard text
        dashboardText.text = "";

        // Loop through all wagons in the TrainController
        foreach (GameObject wagon in trainController.wagons)
        {
            // Display the wagon parent object name in bold color
            dashboardText.text += $"<b><color=#FFD700>{wagon.gameObject.name}:</color></b>\n";

            // Loop through all tasks in the WagonTaskAssigner component
            WagonTaskAssigner taskAssigner = wagon.GetComponent<WagonTaskAssigner>();
            foreach (WagonTask task in taskAssigner.tasks)
            {
                // Display the task type with checkbox and description
                string checkbox = task.isDone ? "<color=#00FF00>[x]</color>" : "[ ]";                
                string description = taskDescriptions[task.taskType];

                // Concatened info output of the checkbot and the task type / desc
                //string text = $"{checkbox} {task.taskType}";
                string text = $"{checkbox} {description}";

                // Cross out the task text if it's done
                if (task.isDone)
                {
                    text = $"<s>{text}</s>";
                }

                dashboardText.text += text + "\n";
            }

            // Add a separator between wagons
            dashboardText.text += "\n";
        }
    }
}