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

    private void Start()
    {
        SpawnTrain(numWagons, useRandomStates);
    }

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


    public void MoveTrainInsideHall()
    {
        isMoving = true;

        // Move the locomotive to the maintenanceTargetPosition
        movementSequence = DOTween.Sequence();
        movementSequence.Append(locomotive.transform.DOMove(endPosition.position, trainMoveInDuration).SetEase(Ease.OutCubic))
            .OnComplete(() =>
            {
                // Set the flag that the train is not moving anymore
                isMoving = false;
            });

    }

    public void MoveTrainOutOfHall()
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

    }

    // Encouple function once train moves in
    public IEnumerator ExecuteDecoupleAfterTime(float time)
    {

        // Save position of each wagon in array before decoupling it for easier encoupling
        SaveInitialWagonPositions();

        // Starting here, decouple logic
        for (int i = wagons.Length - 1; i >= 0; i--)
        {
            Vector3 wagonPosition = locomotive.transform.position + (i + 1) * -decoupleDistance * locomotive.transform.forward; // Calculate new position of each wagon
            wagons[i].transform.DOMove(wagonPosition, decoupleDuration).SetEase(Ease.OutCubic); // Animate and move position of each wagon

            yield return new WaitForSeconds(time);
        }
    }

    // Encouple train again once maintenance is over
    public IEnumerator ExecuteEncoupleAfterTime(float time)
    {
        // Check if control flag is enabled (Flag in inspector and maintenance is over)
        if (encoupleWhenMaintained && !isExecutingMaintenance)
        {
            // Encouple logic
            for (int i = 0; i < wagons.Length; i++)
            {
                Vector3 wagonPosition = initialWagonPositions[i]; // Get initial position of wagon
                wagons[i].transform.DOMove(wagonPosition, decoupleDuration).SetEase(Ease.OutCubic); // Animate and move position of each wagon

                yield return new WaitForSeconds(time);
            }
        }
    }

    /*
     * 
     *  HELPER FUNCTIONS
     *  UNDER
     * 
     */

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


