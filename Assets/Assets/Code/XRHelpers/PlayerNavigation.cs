using UltimateXR.Guides;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    public GameObject zoneA; 
    public GameObject zoneB;

    public Transform targetObjectA;
    public Transform targetObjectB;

    private bool navigateToA = false;
    private bool navigateToB = false;
    private bool reachedBothZones = false;

    void Start()
    {
        UxrCompass.Instance.SetTarget(targetObjectA.transform, UxrCompassDisplayMode.Use);
        navigateToA = true;
    }

    void Update()
    {
        if (navigateToA)
        {
            UxrCompass.Instance.SetTarget(targetObjectA.transform, UxrCompassDisplayMode.Use);
        }
        else if (navigateToB)
        {
            UxrCompass.Instance.SetTarget(targetObjectB.transform, UxrCompassDisplayMode.Use);
        }
        else
        {
            UxrCompass.Instance.SetTarget(null);
        }
    }

    public void ReachedZoneA()
    {
        if (!reachedBothZones)
        {
            navigateToA = false;
            UxrCompass.Instance.SetTarget(targetObjectB.transform, UxrCompassDisplayMode.Use);
            // navigateToB = true;
        }
    }

    public void ReachedZoneB()
    {
        navigateToB = false;
        UxrCompass.Instance.SetTarget(null);
        reachedBothZones = true;
    }

    // When button is clicked in TrainControllerUI, call this one
    public void ButtonIsClicked()
    {
        if (!reachedBothZones)
        {
            navigateToA = false;
            navigateToB = true;
            UxrCompass.Instance.SetTarget(targetObjectB.transform, UxrCompassDisplayMode.Use);
        }
    }
}