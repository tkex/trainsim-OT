using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RefuelTask", menuName = "Task/Refuel Task")]
[System.Serializable]
public class FireExtinguisherTask : WagonTask
{
    [SerializeField]
    public FireExtinguisherTask()
    {
        TaskType = TaskType.FireExtinguisher;
        IsDone = false;
    }

    public override void HandleTask()
    {
        Debug.Log("Handling Fire Extinguisher Task");
        IsDone = true;
        Debug.Log("Fire Extinguisher Task task is now done.");

        CompleteTask();
    }

    public override void SpawnTaskObject(GameObject go, Transform parentTransform)
    {
        // Offset of the GameObject
        Vector3 offset = new Vector3(-1f, 1.08f, 7.34f);

        // Instantiate GameObject with parent Transform
        GameObject fireExtinguisherAnchorObject = GameObject.Instantiate(go, parentTransform);

        // Adjust position relative to the parent Transform
        fireExtinguisherAnchorObject.transform.position = parentTransform.TransformPoint(offset);
    }
}
