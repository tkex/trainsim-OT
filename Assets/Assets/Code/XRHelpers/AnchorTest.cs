using System.Collections;
using System.Collections.Generic;
using UltimateXR.Manipulation;
using UnityEngine;

public class AnchorTest : MonoBehaviour
{
    private bool isAnchorOccupied = false;

    private void UxrGrabManager_ObjectPlaced(object sender, UxrManipulationEventArgs e)
    {
        isAnchorOccupied = true;

        Debug.Log($"Object {e.GrabbableObject.name} was placed on anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");
    }

    private void UxrGrabManager_ObjectRemoved(object sender, UxrManipulationEventArgs e)
    {
        isAnchorOccupied = false;

        Debug.Log($"Object {e.GrabbableObject.name} was removed from anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");
    }

    void Update()
    {
        if (isAnchorOccupied)
        {
            Debug.Log("Occupied");
        }
        else
        {
            Debug.Log("Not occupied");
        }
    }
}

/*
 * public class AnchorTest : MonoBehaviour
{
    private bool isAnchorOccupied = false;
    private string fireExtinguisherName = "FireExtinguisher"; // Ersetzen Sie dies durch den tatsächlichen Namen des Feuerlöschers in Ihrer Szene

    private void OnEnable()
    {
        UxrGrabManager.Instance.ObjectPlaced += UxrGrabManager_ObjectPlaced;
        UxrGrabManager.Instance.ObjectRemoved += UxrGrabManager_ObjectRemoved;
    }

    private void OnDisable()
    {
        UxrGrabManager.Instance.ObjectPlaced -= UxrGrabManager_ObjectPlaced;
        UxrGrabManager.Instance.ObjectRemoved -= UxrGrabManager_ObjectRemoved;
    }

    private void UxrGrabManager_ObjectPlaced(object sender, UxrManipulationEventArgs e)
    {
        if (e.GrabbableObject.name == fireExtinguisherName)
        {
            isAnchorOccupied = true;
            Debug.Log($"Fire Extinguisher was placed on anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");
        }
    }

    private void UxrGrabManager_ObjectRemoved(object sender, UxrManipulationEventArgs e)
    {
        if (e.GrabbableObject.name == fireExtinguisherName)
        {
            isAnchorOccupied = false;
            Debug.Log($"Fire Extinguisher was removed from anchor {e.GrabbableAnchor.name} by {e.Grabber.Avatar.name}");
        }
    }

    void Update()
    {
        if (isAnchorOccupied)
        {
            Debug.Log("Feuerlöscher belegt den Anker");
        }
        else
        {
            Debug.Log("Feuerlöscher belegt den Anker nicht");
        }
    }
}
*/