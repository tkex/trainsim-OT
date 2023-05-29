using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    public bool isChecked = false;
    private float timeEntered = 0;
    public float maxTimeInSeconds = 3;

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