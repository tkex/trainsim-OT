using UnityEngine;
using TMPro;

public class TaskDashboardController : MonoBehaviour
{
    public TrainController trainController;
    public TextMeshProUGUI dashboardText;

    private void Update()
    {
        // Clear the dashboard text
        dashboardText.text = "";

        // Loop through all wagons in the TrainController
        foreach (GameObject wagon in trainController.wagons)
        {
            // Display the wagon parent object name in bold color
            dashboardText.text += $"<b><color=#FFD700>{wagon.gameObject.name}</color></b>\n";

            // Loop through all tasks in the WagonTaskAssigner component
            WagonTaskAssigner taskAssigner = wagon.GetComponent<WagonTaskAssigner>();
            foreach (WagonTask task in taskAssigner.tasks)
            {
                // Display the task type with checkbox
                string checkbox = task.isDone ? "[X]" : "[ ]";
                string text = $"{checkbox} {task.taskType}";

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