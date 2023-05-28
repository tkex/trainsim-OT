using System.Collections;
using UnityEngine;

public class DirtRemove : MonoBehaviour
{
    private bool isMopping = false;
    // Time how long the mob needs to be on the dirt until its destroyed
    public float requiredMopTime = 3f;

    private IEnumerator MopTimer()
    {
        yield return new WaitForSeconds(requiredMopTime);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mop" && !isMopping)
        {
            isMopping = true;
            StartCoroutine(MopTimer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mop")
        {
            isMopping = false;
            StopCoroutine(MopTimer());
        }
    }
}




