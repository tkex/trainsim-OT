using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStateMachine : MonoBehaviour
{
    public enum TrainState
    {
        NotMaintained,
        InProgress,
        Maintained
    }

    // The trainController component attached
    private TrainController trainController;

    // The maintenance state of the train
    [Tooltip("The maintenance state of the train.")]
    public TrainState trainState;

    // Check if all wagons are maintained
    bool allMaintained = true;
    bool inProgress = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the trainController component from the same GameObject
        trainController = GetComponent<TrainController>();

        // Set train state at the start to NotMaintained
        trainState = TrainState.NotMaintained;
    }

    // Update is called once per frame
    void Update()
    {
        allMaintained = true;
        inProgress = false;

        foreach (GameObject wagon in trainController.wagons)
        {
            WagonTaskStateMachine wagonTaskStateMachine = wagon.GetComponent<WagonTaskStateMachine>();
            if (wagonTaskStateMachine.wagonState != WagonStates.Maintained)
            {
                allMaintained = false;
                if (wagonTaskStateMachine.wagonState == WagonStates.InProgress)
                {
                    inProgress = true;
                }
            }
            else
            {
                if (!inProgress)
                {
                    inProgress = true;
                }
            }
        }

        // Set the train state based on the wagon states
        if (!allMaintained && !inProgress)
        {
            trainState = TrainState.NotMaintained;
        }
        else if (inProgress && !allMaintained)
        {
            trainState = TrainState.InProgress;
        }
        else
        {
            trainState = TrainState.Maintained;
        }

        Debug.Log("Train state: " + trainState);
    }
}