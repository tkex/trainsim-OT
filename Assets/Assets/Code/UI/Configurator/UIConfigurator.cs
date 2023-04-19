using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIConfigurator : MonoBehaviour
{
    public Timer timer;
    public TrainController trainController;

    public Slider startTimeSlider;
    public Toggle countDownToggle;
    public Slider numWagonsSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial values of the UI elements based on the variables
        startTimeSlider.value = timer.startTime;
        countDownToggle.isOn = timer.countDown;
        numWagonsSlider.value = trainController.numWagons;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the variables based on the UI elements
        timer.startTime = startTimeSlider.value;
        timer.countDown = countDownToggle.isOn;
        trainController.numWagons = Mathf.RoundToInt(numWagonsSlider.value);
    }

}
