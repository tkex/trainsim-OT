using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// Class for defining an event for the cleaning interior tasks; calling the OnDestroyed event when the dirt is destroyed.
public class CleaningInteriorOnDestroyEvent : MonoBehaviour { 

    // Define an event that gets triggered when the GameObject is destroyed
    public event Action OnDestroyed = delegate { };

    // Trigger OnDestroyed event (when the GameObject is destroyed)
    private void OnDestroy()
    {
        // Invoke (trigger) the OnDestroyed event
        OnDestroyed.Invoke();
    }
}