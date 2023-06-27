using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RemoveBottle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bottle"))
        {
            Destroy(other.gameObject);
        }
    }
}
