using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MopScript : MonoBehaviour
{
    public bool isClean = true;

    private void OnTriggerEnter(Collider other)
    {
        // If in CleanWater, mop is clean again
        if (other.gameObject.tag == "CleanWater")
        {
            isClean = true;
        }
    }
}