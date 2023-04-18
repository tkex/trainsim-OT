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
    public Transform maintenanceTargetPosition; // End position of the train when its maintained and drives away.

    [Tooltip("Duration value for drive in time of the train.")]
    [Range(0.0f, 10f)]
    public float trainMoveInDuration; // Duration of the train ride in seconds
    [Tooltip("Duration how long the decouple process takes.")]
    [Range(0.0f, 10f)]
    public float decoupleDuration; // Duration of the decoupling process in seconds

    private bool isMoving = false; // Becomes true when the train is moving
    private Sequence movementSequence; // DOTween movement sequence

    private GameObject locomotive;
    public  GameObject[] wagons;  // An array to store all created wagons

    Vector3[] initialWagonPositions; // An array to store all prior positions of created wagons before decoupling

    [Tooltip("The individual decouple distance for each wagon.")]
    public float decoupleDistance = 2f;
    [Tooltip("Value how long it takes for each wagon to separate.")]
    public float decoupleInterval = 1.0f;

    [Tooltip("Value how many states a wagon can have.")]
    public int maxNumStatesPerWagon = 3;
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
        GameObject emptyTrainGameObject = new GameObject("EmptyGameObject");

        // Spawn Locomotive
        locomotive = Instantiate(locomotivePrefab, transform.position, transform.rotation);

        // Set parent of locomotive
        locomotive.transform.SetParent(emptyTrainGameObject.transform);

        // Assign a name to the emptyTrainGameObject
        emptyTrainGameObject.name = "Train";

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
                });
        }


        // Based on the StateMachine, do some actions.
        // Get the TrainStateMachine component and read out the current state
        TrainState currentState = gameObject.GetComponent<TrainStateMachine>().trainState;

        // Debug log based on the current state.
        if (currentState == TrainState.Maintained)
        {
            Debug.Log("Train is maintained, tut tut!");
            MoveTrainAfterMaintenance();
          
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


    public void MoveTrainAfterMaintenance()
    {
        StartCoroutine(ExecuteEncoupleAfterTime(2f));
    }


    IEnumerator ExecuteEncoupleAfterTime(float time)
    {
        // Encouple logic
        for (int i = 0; i < wagons.Length; i++)
        {
            Vector3 wagonPosition = initialWagonPositions[i]; // Get initial position of wagon
            wagons[i].transform.DOMove(wagonPosition, decoupleDuration).SetEase(Ease.OutCubic); // Animate and move position of each wagon

            yield return new WaitForSeconds(time);
        }
    }


    IEnumerator ExecuteDecoupleAfterTime(float time)
    {

        // Save initial positions of wagons (so encoupling is easier)
        initialWagonPositions = new Vector3[wagons.Length];

        for (int i = 0; i < wagons.Length; i++)
        {
            initialWagonPositions[i] = wagons[i].transform.position;
        }

        // Starting here, decouple logic
        for (int i = wagons.Length - 1; i >= 0; i--)
        {
            Vector3 wagonPosition = locomotive.transform.position + (i + 1) * -decoupleDistance * locomotive.transform.forward; // Calculate new position of each wagon
            wagons[i].transform.DOMove(wagonPosition, decoupleDuration).SetEase(Ease.OutCubic); // Animate and move position of each wagon

            yield return new WaitForSeconds(time);
        }
    }

    void PositionTrain(GameObject locomotiveGO)
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
        }
    }
}