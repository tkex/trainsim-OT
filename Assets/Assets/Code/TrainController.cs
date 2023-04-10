using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class TrainController : MonoBehaviour
{
    #region States
    // Enum for wagon states
    public enum WagonState
    {
        CleaningTodo,
        CheckOfElectronics,
        ReplacementOfParts,
        SecurityCheck
    }

    // Enum for maintenance state
    public enum MaintenanceState
    {
        NotMaintainedYet,
        InProgress,
        Maintained
    }
    #endregion

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

    [Tooltip("Duration value for drive in time of the train.")]
    [Range(0.0f, 10f)]
    public float trainMoveInDuration; // Duration of the train ride in seconds
    [Tooltip("Duration how long the decouple process takes.")]
    [Range(0.0f, 10f)]
    public float decoupleDuration; // Duration of the decoupling process in seconds

    private bool isMoving = false; // Becomes true when the train is moving
    private Sequence movementSequence; // DOTween movement sequence

    private GameObject locomotive;
    private GameObject[] wagons;  // An array to store all created wagons

    [Tooltip("The individual decouple distance for each wagon.")]
    public float decoupleDistance = 2f;
    [Tooltip("Value how long it takes for each wagon to separate.")]
    public float decoupleInterval = 1.0f;
    #endregion

    Dictionary<GameObject, HashSet<WagonState>> wagonStatesDict = new Dictionary<GameObject, HashSet<WagonState>>();

    void Start()
    {
        // Spawn Locomotive
        locomotive = Instantiate(locomotivePrefab, transform.position, transform.rotation);

        // Spawn Wagons
        SpawnWagons();

        // Position the entire train
        PositionTrain(locomotive);
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
    }

    void PositionTrain(GameObject locomotiveGO)
    {
        // Move the entire train to the trainSpawnPosition
        locomotive.transform.position = trainSpawnPosition;
    }

    IEnumerator ExecuteDecoupleAfterTime(float time)
    {
        // Decouple logic
        for (int i = wagons.Length - 1; i >= 0; i--)
        {
            Vector3 wagonPosition = locomotive.transform.position + (i + 1) * -decoupleDistance * locomotive.transform.forward; // Calculate new position of each wagon
            wagons[i].transform.DOMove(wagonPosition, decoupleDuration).SetEase(Ease.OutCubic); // Animate and move position of each wagon

            yield return new WaitForSeconds(time);
        }
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

            MaintenanceState maintenanceState = MaintenanceState.NotMaintainedYet;
            Debug.Log("Wagon " + i + " Maintenance state: " + maintenanceState);

            // Use HashSet to store unique wagon states for each wagon
            HashSet<WagonState> wagonStates = new HashSet<WagonState>();
            SetRandomWagonStates(wagons[i], UnityEngine.Random.Range(1, 4), wagonStates);
            wagonStatesDict.Add(wagons[i], wagonStates); // Add the wagon and its states to the dictionary
        }
    }

    // Add a method to set a random number of WagonStates for each wagon
    // while making sure each WagonState is unique.
    void SetRandomWagonStates(GameObject wagonGO, int maxNumStates, HashSet<WagonState> existingStates)
    {
        int numStates = UnityEngine.Random.Range(1, maxNumStates + 1); // Get a random number of states for the wagon between 1 and maxNumStates    
        HashSet<WagonState> wagonStates = new HashSet<WagonState>(); // Create a HashSet to store the wagon states

        // Loop through and randomly select unique states for the wagon
        for (int i = 0; i < numStates; i++)
        {
            // Get a random WagonState that is not already in the wagonStates HashSet
            WagonState randomWagonState = GetRandomUniqueWagonState(wagonStates);

            // Add the WagonState to the wagonStates HashSet
            wagonStates.Add(randomWagonState);

            Debug.Log("Wagon " + wagonGO.name + " State " + i + ": " + randomWagonState);
        }
    }


    // Helper method to get a random WagonState that is not already in the given list
    WagonState GetRandomUniqueWagonState(HashSet<WagonState> existingStates)
    {
        HashSet<WagonState> existingStateSet = new HashSet<WagonState>(existingStates); // Create a HashSet from the existing states list

        WagonState randomState = (WagonState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WagonState)).Length); // Get a random WagonState

        // Loop until the randomState is not already in the existingStates list
        while (existingStateSet.Contains(randomState))
        {
            randomState = (WagonState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WagonState)).Length); // Get a new random WagonState
        }

        return randomState;
    }
}