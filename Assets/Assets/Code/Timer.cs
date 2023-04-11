using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component that will display the timer
    private float startTime; // The time that the timer started
    private bool isRunning; // Whether the timer is currently running or not

    void Start()
    {
        startTime = Time.time; // Save the current time as the start time
        isRunning = true; // Start the timer
    }

    void Update()
    {
        if (isRunning)
        {
            // Calculate the elapsed time since the timer started
            float elapsedTime = Time.time - startTime;

            // Calculate the minutes and seconds from the elapsed time
            int minutes = (int)(elapsedTime / 60f);
            int seconds = (int)(elapsedTime % 60f);

            // Format the minutes and seconds as strings
            string minutesStr = minutes.ToString();
            string secondsStr = seconds.ToString().PadLeft(2, '0'); // adds a leading zero if seconds < 10

            // Set the TextMeshProUGUI component to display the time in the format "mm:ss"
            timerText.SetText(minutesStr + ":" + secondsStr);
        }
    }
}