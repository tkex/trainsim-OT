using UltimateXR.Guides;
using UnityEngine;
using UnityEngine.XR;

public class NavigationScript : MonoBehaviour
{
    public Transform targetObject;
 
    void Update()
    {
        //UxrCompass.Instance.SetTarget(targetObject.transform, UxrCompassDisplayMode.Look);
        //UxrCompass.Instance.SetTarget(targetObject.transform, UxrCompassDisplayMode.Location);
        //UxrCompass.Instance.SetTarget(targetObject.transform, UxrCompassDisplayMode.Grab);  
        UxrCompass.Instance.SetTarget(targetObject.transform, UxrCompassDisplayMode.Use);  
    }
}