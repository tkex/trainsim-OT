using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class TrainController : MonoBehaviour
{

    public enum TrainState
    {
        NotMaintained,
        InProgress,
        Maintained
    }

    public TrainState trainState = TrainState.NotMaintained;


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

    [Tooltip("Value how many states a wagon can have.")]
    public int maxNumStatesPerWagon = 3;
    #endregion

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

            // Assign a name to the wagon based on its index
            wagons[i].name = "Wagon " + (i + 1);
        }
    }
}