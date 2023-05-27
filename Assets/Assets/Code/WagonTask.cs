
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public abstract class WagonTask : ScriptableObject
{
    public TaskType TaskType;
    public bool IsDone;
    public event Action<WagonTask> TaskCompleted;

    public void CompleteTask()
    {
        IsDone = true;
        TaskCompleted?.Invoke(this);
    }

    public abstract void SpawnTaskObject(GameObject go, Transform parentTransform);
    public abstract void HandleTask();


    // References for individual task completion
    public GameObject associatedWagon;
    public WagonTaskHandling wagonTaskHandling;
}
