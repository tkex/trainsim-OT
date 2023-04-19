using UnityEngine;
using TMPro;

public class TaskDashboardController : MonoBehaviour
{
    public TrainController trainController;
    public TextMeshProUGUI dashboardText;

    void Update()
    {
        // Clear the dashboard text
        dashboardText.text = "";

        // Loop through all wagons in the TrainController
        foreach (GameObject wagon in trainController.wagons)
        {
            // Display the wagon name
            dashboardText.text += wagon.gameObject.name + "\n";

            // Loop through all tasks in the wagon
            WagonTaskAssigner taskAssigner = wagon.GetComponent<WagonTaskAssigner>();
            foreach (WagonTask task in taskAssigner.tasks)
            {
                // Display the task name and status
                dashboardText.text += "- " + task.taskType.ToString();
                if (task.isDone)
                {
                    dashboardText.text += " [X]";
                }
                dashboardText.text += "\n";
            }

            // Add a separator between wagons
            dashboardText.text += "\n";
        }
    }
}