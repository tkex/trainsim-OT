using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script observes the state changes of a WagonTaskStateMachine
public class WagonStateObserver : MonoBehaviour
{
    // A reference to the WagonTaskStateMachine that this observer is observing
    public WagonTaskStateMachine wagonStateMachine;

    // Called when this script is enabled
    private void OnEnable()
    {
        // Register the HandleWagonStateChanged method to be called when the wagon state changes
        wagonStateMachine.OnWagonStateChanged += HandleWagonStateChanged;
    }

    // Called when this script is disabled
    private void OnDisable()
    {
        // Unregister the HandleWagonStateChanged method from being called when the wagon state changes
        wagonStateMachine.OnWagonStateChanged -= HandleWagonStateChanged;
    }

    // Whenever the wagon state changes, this functions will be called and allows to respond to the state change
    private void HandleWagonStateChanged(WagonStates newState)
    {
        // Do something in response to the wagon state changing
        // Like in this case output a debug message
        Debug.Log(gameObject.name + " state changed to: " + newState);
    }
}