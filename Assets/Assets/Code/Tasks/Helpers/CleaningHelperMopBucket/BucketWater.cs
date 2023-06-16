using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketWater : MonoBehaviour
{
    // Counter var
    private int cleanCounter = 0;

    // Hit amount until mop is clean again
    public int maxHitAmount = 3;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger
        if (other.gameObject.CompareTag("Mop"))
        {
            // Get the MopScript from the mop
            MopScript mopScript = other.gameObject.GetComponent<MopScript>();

            if (mopScript != null)
            {
                cleanCounter++;

                if (cleanCounter >= maxHitAmount)
                {
                    // Set boolean to true when enough hits are counted
                    mopScript.isClean = true;
                    // Reset the cleaner counter
                    cleanCounter = 0;
                }
            }
        }
    }
}