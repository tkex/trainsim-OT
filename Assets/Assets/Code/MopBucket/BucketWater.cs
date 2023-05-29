using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketWater : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger
        if (other.gameObject.CompareTag("Mop"))
        {
            // Get the MopScript from the broom.
            MopScript mopScript = other.gameObject.GetComponent<MopScript>();

            // If the broom has the MopScript, set the isClean boolean
            if (mopScript != null)
            {
                mopScript.isClean = true;
            }
        }
    }
}