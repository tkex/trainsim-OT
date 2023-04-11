using UnityEngine;
using System.Collections.Generic;
using System;

public enum WagonState
{
    NotMaintained,
    InProgress,
    Maintained
}

public enum TaskType
{
    Cleaning,
    CheckElectronics,
    RepairLamp,
}

[System.Serializable]
public class WagonTask
{
    public TaskType taskType;
    public bool isDone;
}

public class WagonStateController : MonoBehaviour
{
    public WagonState wagonState;
    public List<WagonTask> tasks = new List<WagonTask>();

    private bool isTasksInitialized = false;

    private TrainController trainController;


    private void Start()
    {
        trainController = transform.parent.GetComponent<TrainController>();
        wagonState =  WagonState.NotMaintained;
    }

}