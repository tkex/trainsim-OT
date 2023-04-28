using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public abstract class WagonTask
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

}