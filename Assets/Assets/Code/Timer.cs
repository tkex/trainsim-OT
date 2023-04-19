using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [Tooltip("Drag in the Timer text.")]
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component that will display the timer
    [Tooltip("Value of the number that gets counted down or up from.")]
    public float startTime = 60f; // The time that the timer will start at (default is 60 seconds)
    [Tooltip("If false, the countdown counts up and if true the timer counts down from the value of start time.")]
    public bool countDown = false; // Whether the timer will count down instead of up

    private float currentTime; // The current time on the timer
    private bool isRunning; // Whether the timer is currently running or not

    void Start()
    {
        // Set time inits
        currentTime = startTime;

        // Disable later and start when game start is init.
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            // Update the current time based on whether the timer is counting up or down
            if (countDown)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime += Time.deltaTime;
            }

            // Clamp the current time to zero if it goes below zero
            currentTime = Mathf.Max(currentTime, 0f);

            // Calculate the minutes and seconds from the current time
            int minutes = (int)(currentTime / 60f);
            int seconds = (int)(currentTime % 60f);

            // Format the minutes and seconds as strings
            string minutesStr = minutes.ToString().PadLeft(2, '0'); // Adds a leading zero if minutes < 10
            string secondsStr = seconds.ToString().PadLeft(2, '0'); // Adds a leading zero if seconds < 10

            // Set the TextMeshProUGUI component to display the time in the format "mm:ss"
            timerText.SetText(minutesStr + ":" + secondsStr);

            // Check if the timer has run out
            if (currentTime == 0f)
            {
                StopTimer();

                Debug.Log("Time's up!"); // Do something when the timer runs out
            }
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {        
        isRunning = false;
        string minutesStr = ((int)(currentTime / 60f)).ToString().PadLeft(2, '0');
        string secondsStr = ((int)(currentTime % 60f)).ToString().PadLeft(2, '0');
        timerText.SetText(minutesStr + ":" + secondsStr);
    }
}