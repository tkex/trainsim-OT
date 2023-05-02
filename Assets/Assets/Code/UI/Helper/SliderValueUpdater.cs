using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValueUpdater : MonoBehaviour
{
    public TextMeshProUGUI sliderValueText;
    public Slider slider;

    void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        sliderValueText.text = value.ToString();
    }
}