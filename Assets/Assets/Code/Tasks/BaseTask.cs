using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTask : MonoBehaviour, ITask
{
    protected bool isDone = false;

    public abstract void StartTask();

    public virtual bool IsTaskDone()
    {
        return isDone;
    }
}