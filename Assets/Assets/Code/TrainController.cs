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

    void Start()
    {
        // Spawn Locomotive
        locomotive = Instantiate(locomotivePrefab, transform.position, transform.rotation);

        // Spawn Wagons
        SpawnWagons();

        // Position the entire train
        PositionTrain(locomotive);
    }

    void SpawnWagons()
    {
        wagons = new GameObject[numWagons];  // Initialize the wagon array with the number of wagons to spawn

        // Spawn the wagons one after the other with a spacing of wagonSpacing
        for (int i = 0; i < numWagons; i++)
        {
            Vector3 wagonPosition = transform.position + (i + 1) * -wagonSpacing * transform.forward;  // Calculate the position of the wagon based on the position and rotation of the locomotive
            Quaternion wagonRotation = locomotive.transform.rotation;  // The wagon has the same rotation as the locomotive
            wagons[i] = Instantiate(wagonPrefab, wagonPosition, wagonRotation);  // Create the wagon

            wagons[i].transform.parent = locomotive.transform;  // Set the wagon object as a child of the locomotive object
                                                                // i.e. all transformation changes made to the locomotive object will also affect the wagon object

            // Set wagon states randomly from the enum list
            WagonState randomWagonState = (WagonState)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WagonState)).Length);
            Debug.Log("Wagon " + i + " State: " + randomWagonState);

            // Set wagon state to not maintained yet as default
            MaintenanceState maintenanceState = MaintenanceState.NotMaintainedYet;
            Debug.Log("Wagon " + i + " Maintenance state: " + maintenanceState);
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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            // Start train movement
            isMoving = true;
            movementSequence = DOTween.Sequence();
            movementSequence.Append(locomotive.transform.DOMove(endPosition.position, trainMoveInDuration).SetEase(Ease.OutCubic))
                .OnComplete(() => {
                    isMoving = false;
                    Debug.Log("Gestoppt!");

                    // Decoupling of train
                    StartCoroutine(ExecuteDecoupleAfterTime(decoupleInterval));
                });
        }
    }   
}