using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationTriggerZone : MonoBehaviour
{

    public PlayerNavigation playerNavigation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject == playerNavigation.zoneA)
            {
                playerNavigation.ReachedZoneA();
            }
            else if (this.gameObject == playerNavigation.zoneB)
            {
                playerNavigation.ReachedZoneB();
            }
        }
    }
}