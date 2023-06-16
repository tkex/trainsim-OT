using System.Collections;
using UnityEngine;


public class DirtRemove : MonoBehaviour
{
    private bool isMopping = false;

    // Time how long the mob needs to be on the dirt until its destroyed
    public float requiredMopTime = 3f;

    private IEnumerator MopTimer(MopScript mopScript)
    {
        yield return new WaitForSeconds(requiredMopTime);

        if (mopScript.isClean)
        {
            Destroy(gameObject);

            // Mop is now dirty
            mopScript.isClean = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mop")
        {
            // Get the MopScript
            MopScript mopScript = other.gameObject.GetComponent<MopScript>();

            // If not null, not mopping and the mop clean, cleaning is possible
            if (mopScript != null && mopScript.isClean && !isMopping)
            {
                isMopping = true;
                StartCoroutine(MopTimer(mopScript));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mop")
        {
            isMopping = false;
            StopAllCoroutines();
        }
    }
}



