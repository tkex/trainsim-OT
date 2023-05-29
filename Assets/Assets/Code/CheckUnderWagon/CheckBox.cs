using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    public bool isChecked = false;
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
                isChecked = true;
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