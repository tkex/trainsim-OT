using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [Tooltip("Drag in the Timer text.")]
    // Reference to the TextMeshProUGUI component that will display the timer
    public TextMeshProUGUI timerText;

    [Tooltip("Value of the number that gets counted down or up from.")]
    // The time that the timer will start at
    public float startTime = 60f;

    [Tooltip("If false, the countdown counts up and if true the timer counts down from the value of start time.")]
    // If the timer will count down instead of up
    public bool countDown = false;

    private float currentTime;
    private bool isRunning;

    void Start()
    {
        // Set time inits
        currentTime = startTime;

        // Disable later and start when game start is init
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

            // Stop the current time to zero if it goes below zero
            currentTime = Mathf.Max(currentTime, 0f);

            // Calculate the minutes and seconds from the current time
            int minutes = (int)(currentTime / 60f);
            int seconds = (int)(currentTime % 60f);

            // Format the minutes and seconds as strings
            // Adds a zero if minutes < 10
            string minutesStr = minutes.ToString().PadLeft(2, '0');
            // Adds a zero if seconds < 10
            string secondsStr = seconds.ToString().PadLeft(2, '0');

            timerText.SetText(minutesStr + ":" + secondsStr);

            // Check if the timer has run out
            if (currentTime == 0f)
            {
                StopTimer();

                // Do something when the timer runs out
                // Debug.Log("Time's up!");
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