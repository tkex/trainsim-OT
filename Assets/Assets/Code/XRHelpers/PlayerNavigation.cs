using System.Collections;
using UltimateXR.Guides;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    // The trigger gameobject that have the navigationtriggerzone script on them
    public GameObject zoneA;
    public GameObject zoneB;

    // The gameobject the compass directs to
    public Transform targetObjectA;
    public Transform targetObjectB;

    // Delay parameters for Compass
    public float navigateToADelay = 3f;
    public float navigateToBDelayWhenButtonIsClicked = 10f;

    // Control flags for handling logic
    private bool navigateToA = false;
    private bool navigateToB = false;
    private bool reachedBothZones = false;

    void Start()
    {
        StartCoroutine(ActivateCompassToAAfterDelay(navigateToADelay));
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
            UxrCompass.Instance.SetTarget(null);
            zoneB.SetActive(true);
        }
    }

    public void ReachedZoneB()
    {
        navigateToB = false;
        UxrCompass.Instance.SetTarget(null);
        reachedBothZones = true;
        zoneB.SetActive(false);
    }

    private IEnumerator ActivateCompassToAAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        navigateToA = true;
        UxrCompass.Instance.SetTarget(targetObjectA.transform, UxrCompassDisplayMode.Use);
    }

    // When button is clicked in TrainControllerUI, call this function to start the Coroutine for new Compass direction
    public void ButtonIsClicked()
    {
        if (!reachedBothZones)
        {
            StartCoroutine(ActivateCompassToBAfterDelay(navigateToBDelayWhenButtonIsClicked));
        }
    }

    private IEnumerator ActivateCompassToBAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        navigateToA = false;
        navigateToB = true;
        UxrCompass.Instance.SetTarget(targetObjectB.transform, UxrCompassDisplayMode.Use);
    }
}