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
        { TaskType.Cleaning, "Clean Wagon" },
        { TaskType.CheckBrakes, "Check brakes" },
        { TaskType.InspectCouplers, "Check couplers" },
        { TaskType.CleanInterior, "Clean interior" },
        { TaskType.RefuelEngine, "Refill the motor" },
        { TaskType.FireExtinguisher, "Refill Fire Extinguisher" },
        { TaskType.Medkit, "Refill Medkits" }
        // Add more mappings as needed (see enums)
    };

    private void Update()
    {
        // Clear the dashboard text
        dashboardText.text = "";

        // Loop through all wagons in the TrainController
        foreach (GameObject wagon in trainController.wagons)
        {
            // Get the WagonTaskAssigner component
            WagonTaskAssigner taskAssigner = wagon.GetComponent<WagonTaskAssigner>();

            // Display the wagon parent object name in bold color
            dashboardText.text += $"<b><color=#FFD700>{wagon.gameObject.name}:</color></b>\n";

            if (taskAssigner.tasks != null && taskAssigner.tasks.Count > 0 && taskAssigner.tasks[0] != null)
            {
                // Loop through all tasks in the WagonTaskAssigner component                
                foreach (WagonTask task in taskAssigner.tasks)
                {
                    // Display the task type with checkbox and description
                    string checkbox = task.IsDone ? "<color=#00FF00>[x]</color>" : "[ ]";
                    string description = taskDescriptions[task.TaskType];

                    // Concatened info output of the checkbot and the task type / desc
                    //string text = $"{checkbox} {task.taskType}";
                    string text = $"{checkbox} {description}";

                    // Cross out the task text if it's done
                    if (task.IsDone)
                    {
                        text = $"<s>{text}</s>";
                    }

                    dashboardText.text += text + "\n";
                }
            }
            // Add a separator between wagons
            dashboardText.text += "\n";
        }
    }
}