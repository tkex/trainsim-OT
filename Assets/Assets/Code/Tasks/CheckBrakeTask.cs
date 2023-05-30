using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Manipulation;

[CreateAssetMenu(fileName = "CheckBrakeTask", menuName = "Task/CheckBrake Task")]
[System.Serializable]
public class CheckBrakeTask : WagonTask
{

    private CheckBox checkBox;

    [SerializeField]
    public CheckBrakeTask()
    {
        TaskType = TaskType.CheckBrakes;
        IsDone = false;
    }

    private void OnDisable()
    {
        // Unsubscribe IsCheckedChanged event
        if (checkBox != null)
        {
            checkBox.IsCheckedChanged -= CheckBox_IsCheckedChanged;
        }
    }

    private void CheckBox_IsCheckedChanged(object sender, System.EventArgs e)
    {
        // Call HandleTask when isChecked is set to true
        HandleTask();
    }

    public override void HandleTask()
    {

        // Make sure that wagonTaskHandling is not null and currentPlayerWagon is set
        if (wagonTaskHandling == null || wagonTaskHandling.currentPlayerWagon == null)
            return;

        // If this task is not associated with the current wagon, cancel
        if (associatedWagon != wagonTaskHandling.currentPlayerWagon)
            return;


        Debug.Log("Handling CheckBrakes Task");
        IsDone = true;

        Debug.Log("CheckBrakes Task is now done.");

        CompleteTask();
    }

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
        // Offset of the GameObject
        Vector3 offset = new Vector3(-0.689f, 0.378f, -5.316f);

        // Offset for rotation
        //Vector3 rotationAngles = new Vector3(90f, 0f, -90f);

        // Instantiate GameObject with parent Transform
        GameObject checkBrakeZoneObject = GameObject.Instantiate(go, parentTransform);

        // Adjust position relative to the parent Transform
        checkBrakeZoneObject.transform.position = parentTransform.TransformPoint(offset);

        // Adjust rotation relative to the parent Transform
        //checkBrakeZoneObject.transform.rotation = Quaternion.Euler(rotationAngles);

        // Subscribe to IsCheckedChanged event
        checkBox = checkBrakeZoneObject.GetComponent<CheckBox>();
        if (checkBox != null)
        {
            checkBox.IsCheckedChanged += CheckBox_IsCheckedChanged;
        }
        else
        {
            Debug.LogError("CheckBox script not found on the spawned object.");
        }
    }
}

