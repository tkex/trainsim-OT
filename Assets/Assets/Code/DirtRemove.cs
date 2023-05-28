using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DirtRemove : MonoBehaviour
{
    private int hitCount;
    public int maxMopHits = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mop")
        {
            hitCount++;

            if (hitCount >= maxMopHits)
            {
                Destroy(gameObject);
            }
        }
    }
}