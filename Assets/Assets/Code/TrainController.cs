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
    WagonTaskAssigner wagonTaskAssigner;  // Activate or deactivate the script dependend on the useRandomState boolean flag

    [Header("Dynamic - No touch here")]
    private GameObject emptyTrainGameObject; // Empty train parent
    private GameObject locomotive; // Locomotive GO
    public GameObject[] wagons;  // An array to store all created wagons
  
    

    #endregion

    // State machine from train
    private TrainStateMachine trainStateMachine;

    private void Awake()
    {
        trainStateMachine = GetComponent<TrainStateMachine>();
    }

    

    void Start()
    {
        // Spawn Empty Tain GameObject
        emptyTrainGameObject = new GameObject("Train");

        // Spawn Locomotive
        locomotive = Instantiate(locomotivePrefab, transform.position, transform.rotation);

        // Set parent of locomotive
        locomotive.transform.SetParent(emptyTrainGameObject.transform);

        // Assign a name to the emptyTrainGameObject
        //emptyTrainGameObject.name = "Train";

        // Spawn Wagons
        SpawnWagons();

        // Position the entire train
        PositionTrain(emptyTrainGameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            // Start train movement
            isMoving = true;
            movementSequence = DOTween.Sequence();
            movementSequence.Append(locomotive.transform.DOMove(endPosition.position, trainMoveInDuration).SetEase(Ease.OutCubic))
                .SetDelay(trainMoveInDelay)
                .OnComplete(() => {
                    isMoving = false;
                    Debug.Log("Gestoppt!");

                    // Decoupling of train
                    StartCoroutine(ExecuteDecoupleAfterTime(decoupleInterval));

                    // Set flag for showing maintenance has started
                    isExecutingMaintenance = true;
                });
        }

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

                // Start encoupling process and maintenance movement
                StartCoroutine(MoveTrainAfterMaintenanceWithDelay());
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

    IEnumerator MoveTrainAfterMaintenanceWithDelay()
    {
        // Encouple first
        StartCoroutine(ExecuteEncoupleAfterTime(decoupleInterval));

        // Move train
        yield return MoveTrainAfterMaintenance(delayBetweenEncoupleAndDriveOut);
    }

    IEnumerator MoveTrainAfterMaintenance(float delayAmount)
    {
        // Check if control flag is enabled
        if (startTrainMovementAfterMaintenance && !isExecutingMaintenance)
        {
            // The delay between encoupling and maintenance drive out
            yield return new WaitForSeconds(delayAmount);
        
            // Start train movement
            isMoving = true;
            movementSequence = DOTween.Sequence();
            movementSequence.Append(locomotive.transform.DOMove(maintenanceTargetPosition.position, trainMoveInDuration).SetEase(Ease.InQuint))
                .SetDelay(trainMoveInDelay)
                .OnComplete(() => {
                    isMoving = false;
                    Debug.Log("DONE!");
                });
        }
    }

    IEnumerator ExecuteEncoupleAfterTime(float time)
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

    IEnumerator ExecuteDecoupleAfterTime(float time)
    {
        // Check if control flag is enabled
        if (decoupleWhenInHall)
        {
            // Save position of each wagon in array before decoupling it for easier encoupling
            SaveWagonInitialPosiiton();

            // Starting here, decouple logic
            for (int i = wagons.Length - 1; i >= 0; i--)
            {
                Vector3 wagonPosition = locomotive.transform.position + (i + 1) * -decoupleDistance * locomotive.transform.forward; // Calculate new position of each wagon
                wagons[i].transform.DOMove(wagonPosition, decoupleDuration).SetEase(Ease.OutCubic); // Animate and move position of each wagon

                yield return new WaitForSeconds(time);
            }
        }
    }

    private void SaveWagonInitialPosiiton()
    {
        // Save initial positions of wagons (so encoupling is easier)
        initialWagonPositions = new Vector3[wagons.Length];

        for (int i = 0; i < wagons.Length; i++)
        {
            initialWagonPositions[i] = wagons[i].transform.position;
        }
    }

    private void PositionTrain(GameObject locomotiveGO)
    {
        // Move the entire train to the trainSpawnPosition
        locomotive.transform.position = trainSpawnPosition;
    }  

    void SpawnWagons()
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


            // Here later on, if a wagon gets random states or with user input
            // Thus disabling the WagonTaskAssigner on the wagonPrefab or choosing states defined
            // by the user here (in decoupled)

            // Here to go then

            if (useRandomStates)
            {
                // Activitate the random state script on the wagon
                wagons[i].GetComponent<WagonTaskAssigner>().AssignRandomTasksToWagon();
            }
            else
            {
                // Set up manual tasks (in this case a new Cleaning Task)
                WagonTaskAssigner wagonTaskAssigner = wagons[i].GetComponent<WagonTaskAssigner>();
                CleaningTask cleaningTask = new CleaningTask();

                if (wagonTaskAssigner != null)
                {
                    wagonTaskAssigner.AssignSpecificTaskToWagon(cleaningTask);
                }
            }
        }
    }
}