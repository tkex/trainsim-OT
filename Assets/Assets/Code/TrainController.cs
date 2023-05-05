using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class TrainController : MonoBehaviour
{
    #region Prefabs
    [Header("Prefab Settings")]
    public GameObject locomotivePrefab;
    public GameObject wagonPrefab;

    #endregion

    #region Configurator-Variables
    [Header("Configurator Settings")]
    [Tooltip("Number of spawning wagons")]
    public int numWagons = 5;

    [Tooltip("Spacing value for wagon spawning")]
    public float wagonSpacing = 1.0f;

    [Tooltip("Spawn position of the train.")]
    public Vector3 trainSpawnPosition = new Vector3(0.0f, 0.0f, 0.0f);


    // Lerp (Movement) Variables
    [Header("LERP/Movement Settings")]
    [Tooltip("Delay before the train moves into the platform.")]
    public float trainMoveInDelay = 2f; // Delay before the train moves in (when space is pressed)

    [Tooltip("End position of train, dependent on empty GO position.")]
    public Transform endPosition; // End position of the train

    [Tooltip("End position of train after its maintenance.")]
    public Transform maintenanceTargetPosition; // End position of the train when its maintained and drives away

    [Tooltip("Duration value for drive in time of the train.")]
    [Range(0.0f, 10f)]
    public float trainMoveInDuration; // Duration of the train ride in seconds
    [Tooltip("Duration how long the decouple process takes.")]
    [Range(0.0f, 10f)]
    public float decoupleDuration; // Duration of the decoupling process in seconds

    private bool isMoving = false; // Becomes true when the train is moving
    private Sequence movementSequence; // DOTween movement sequence


    Vector3[] initialWagonPositions; // An array to store all prior positions of created wagons before decoupling

    private bool isExecutingMaintenance = false; // Control flag to ensure that maintenance sequence is executed only once
    private bool hasEncoupled = false; // Becomes true when train encouples again

    [Header("En- and Decoupling Settings")]
    [Tooltip("The individual decouple distance for each wagon.")]
    public float decoupleDistance = 2f;
    [Tooltip("Value how long it takes for each wagon to separate or encouple.")]
    public float decoupleInterval = 1.0f;
    [Tooltip("Individual time in seconds (delay) for the encoupling and drive out.")]
    [SerializeField] private float delayBetweenEncoupleAndDriveOut = 3f;

    [Header("Control-Flags Settings")]
    [Tooltip("Flag to determine whether should encouple when moving into hall.")]
    public bool decoupleWhenInHall = true;
    [Tooltip("Flag to determine whether should encouple again after maintenance.")]
    public bool encoupleWhenMaintained = true;
    [Tooltip("Flag to determine whether train drives out after maintenance nor not.")]
    public bool startTrainMovementAfterMaintenance = true;

    [Header("State Settings")]
    [Tooltip("Flag to determine whether wagons get random states or defined ones.")]
    public bool useRandomStates;

    [Header("Dynamic - No touch here")]
    public GameObject emptyTrainGameObject; // Empty train parent
    private GameObject locomotive; // Locomotive GO
    public GameObject[] wagons;  // An array to store all created wagons

    #endregion

    void Update()
    {
        // Check all the time for the train maintenance states
        CheckTrainState();        
    }


    void CheckTrainState()
    {
        // Get the TrainStateMachine component and read out the current state
        TrainState currentState = gameObject.GetComponent<TrainStateMachine>().trainState;

        // Debug log based on the current state.
        if (currentState == TrainState.Maintained)
        {
            // Debug.Log("Train is maintained, tut tut!");

            // Set flag that maintenance is done
            isExecutingMaintenance = false;

            // Check if control flags are enabled
            if (!hasEncoupled && !isMoving && !isExecutingMaintenance)
            {
                // Set flag so no endless loop for positioning for wagons happen
                hasEncoupled = true; 

                // Start encoupling process and maintenance movement (need to uncomment)
                //StartCoroutine(MoveTrainAfterMaintenanceWithDelay());
            }
        }
        else if (currentState == TrainState.InProgress)
        {
            // Debug.Log("Train is in progress!");
        }
        else
        {
            // Debug.Log("Train is not maintained yet.");
        }
    }
    private IEnumerator ExecuteDecoupleAfterTime(float delayTime)
    {
        // Delay for a moment, then execute decoupling
        yield return new WaitForSeconds(delayTime);
        ExecuteDecoupling();
    }

    private void ExecuteDecoupling()
    {
        // Store all initial positions of wagons
        initialWagonPositions = new Vector3[wagons.Length];

        for (int i = 0; i < wagons.Length; i++)
        {
            initialWagonPositions[i] = wagons[i].transform.position;
        }

        // Decouple wagons one by one
        for (int i = 0; i < wagons.Length; i++)
        {
            Vector3 decoupleTargetPosition = initialWagonPositions[i] - (Vector3.right * decoupleDistance);

            wagons[i].transform.DOMove(decoupleTargetPosition, decoupleDuration).SetEase(Ease.OutCubic);
        }

        // Reset the encoupling flag
        hasEncoupled = false;
    }

    private void EncoupleTrain()
    {
        // Encouple wagons one by one
        for (int i = 0; i < wagons.Length; i++)
        {
            wagons[i].transform.DOMove(initialWagonPositions[i], decoupleDuration).SetEase(Ease.OutCubic);
        }

        // Reset the encoupling flag
        hasEncoupled = true;

        // Drive out of hall
        if (startTrainMovementAfterMaintenance)
        {
            StartCoroutine(DelayedStartOfTrainMovement(delayBetweenEncoupleAndDriveOut));
        }
    }

    IEnumerator DelayedStartOfTrainMovement(float delay)
    {
        // Delay for a moment, then execute start of train movement
        yield return new WaitForSeconds(delay);

        MoveTrainOutOfHall();
    }

    private void MoveTrainOutOfHall()
    {
        isMoving = true;

        // Move the locomotive to the maintenanceTargetPosition
        movementSequence = DOTween.Sequence();
        movementSequence.Append(locomotive.transform.DOMove(maintenanceTargetPosition.position, trainMoveInDuration).SetEase(Ease.OutCubic))
            .OnComplete(() =>
            {
            // Set the flag that the train is not moving anymore
            isMoving = false;
            });

        // Encouple train again
        if (encoupleWhenMaintained)
        {
            ExecuteEncoupling();
        }
    }

    private void ExecuteEncoupling()
    {
        // Delay the encoupling process
        StartCoroutine(ExecuteEncouplingAfterTime(decoupleInterval * wagons.Length));

        IEnumerator ExecuteEncouplingAfterTime(float delayTime)
        {
            // Delay for a moment, then execute encoupling
            yield return new WaitForSeconds(delayTime);
            EncoupleTrain();
        }
    }

    private void PositionTrain(GameObject locomotiveGO)
    {
        // Move the entire train to the trainSpawnPosition
        locomotive.transform.position = trainSpawnPosition;
    }

    private void SaveInitialWagonPositions()
    {
        initialWagonPositions = new Vector3[wagons.Length];

        for (int i = 0; i < wagons.Length; i++)
        {
            initialWagonPositions[i] = wagons[i].transform.position;
        }
    }

    private void ReturnWagonsToInitialPositions()
    {
        for (int i = 0; i < wagons.Length; i++)
        {
            wagons[i].transform.position = initialWagonPositions[i];
        }
    }


    private IEnumerator ExecuteEncouplingAfterTime(float interval)
    {
        yield return new WaitForSeconds(interval);

        // Check if train needs to be moved out of the maintenance hall
        if (startTrainMovementAfterMaintenance)
        {
            isMoving = true;
            movementSequence = DOTween.Sequence();
            movementSequence.Append(emptyTrainGameObject.transform.DOMove(maintenanceTargetPosition.position, trainMoveInDuration).SetEase(Ease.OutCubic))
                .SetDelay(delayBetweenEncoupleAndDriveOut)
                .OnComplete(() =>
                {
                    isMoving = false;

                // Set flag to know encoupling was done
                hasEncoupled = true;
                });
        }
        else
        {
            // If train does not move, directly set flag to know encoupling was done
            hasEncoupled = true;
        }

        // Return wagons to their initial positions
        ReturnWagonsToInitialPositions();

        // Restore the parent-child relation of wagons
        for (int i = 0; i < wagons.Length; i++)
        {
            wagons[i].transform.SetParent(emptyTrainGameObject.transform);
        }
    }


    List<GameObject> SpawnWagons(int numWagons, bool useRandomStates)
    {
        List<GameObject> spawnedWagons = new List<GameObject>();

        {
            wagons = new GameObject[numWagons];

            for (int i = 0; i < numWagons; i++)
            {
                Vector3 wagonPosition = transform.position + (i + 1) * -wagonSpacing * transform.forward;
                Quaternion wagonRotation = locomotive.transform.rotation;
                wagons[i] = Instantiate(wagonPrefab, wagonPosition, wagonRotation);
                wagons[i].transform.parent = locomotive.transform;

                // Assign a name to the wagon based on its index
                wagons[i].name = "Wagon " + (i + 1);

                // Check if wagons are going to use random tasks or specific single tasks that are added in runtime
                if (useRandomStates)
                {
                    // Activitate the random state script on the wagon
                    wagons[i].GetComponent<WagonTaskAssigner>().AssignRandomTasksToWagon();
                }
                else
                {
                    //Debug.Log("Manual tasks can be set up here!");

                    /*
                    // Set up manual tasks (in this case a new Cleaning Task)
                    WagonTaskAssigner wagonTaskAssigner = wagons[i].GetComponent<WagonTaskAssigner>();
                    CleaningTask cleaningTask = new CleaningTask();

                    if (wagonTaskAssigner != null)
                    {
                        wagonTaskAssigner.AssignSpecificTaskToWagon(cleaningTask);
                    }
                    */
                }
            }
        }

        return spawnedWagons;
    }

    public void AssignTaskToWagon(int wagonIndex, WagonTask task)
    {
        if (wagonIndex < 0 || wagonIndex >= wagons.Length)
        {
            Debug.LogError("Invalid wagon index");
            return;
        }

        WagonTaskAssigner wagonTaskAssigner = wagons[wagonIndex].GetComponent<WagonTaskAssigner>();
        if (wagonTaskAssigner != null)
        {
            wagonTaskAssigner.AssignSpecificTaskToWagon(task);
        }
    }


    public void SpawnTrain(int numWagons, bool useRandomStates)
    {
        // Spawn Empty Tain GameObject
        emptyTrainGameObject = new GameObject("Train");

        // Spawn Locomotive
        locomotive = Instantiate(locomotivePrefab, transform.position, transform.rotation);

        // Spawn Empty Tain GameObject
        locomotive.name = "Locomotive";

        // Set parent of locomotive
        locomotive.transform.SetParent(emptyTrainGameObject.transform);

        // Spawn the wagons
        SpawnWagons(numWagons, useRandomStates);

        // Position the entire train
        PositionTrain(emptyTrainGameObject);
    }
}


