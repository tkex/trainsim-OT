using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    public delegate void IsCheckedEventHandler(object sender, System.EventArgs e);

    public event IsCheckedEventHandler IsCheckedChanged;

    private bool _isChecked = false;
    public bool IsChecked
    {
        get { return _isChecked; }
        private set
        {
            _isChecked = value;
            if (_isChecked)
            {
                IsCheckedChanged?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }

    private float timeEntered = 0;
    public float maxTimeInSeconds = 3;

    public Color checkedColor = Color.green;
    private new Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckStick"))
        {
            timeEntered = Time.time;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CheckStick"))
        {
            if (Time.time - timeEntered >= maxTimeInSeconds)
            {
                IsChecked = true;
                Debug.Log("Trigger checkbox is set!");

                // Change the color
                if (renderer != null)
                {
                    renderer.material.color = checkedColor;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckStick"))
        {
            timeEntered = 0;
        }
    }
}