using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainController : MonoBehaviour
{
    #region Prefabs
    [Header("Prefab-Einstellungen")]
    public GameObject locomotivePrefab;
    public GameObject wagonPrefab;
    #endregion

    #region Configurator-Variables
    [Header("Konfigurator-Einstellungen")]
    [Tooltip("Number of spawning wagons")]
    public int numWagons = 5;

    [Tooltip("Spacing value for wagon spawning")]
    public float wagonSpacing = 1.0f;

    [Tooltip("Spawn position of the train.")]
    public Vector3 trainSpawnPosition = new Vector3(0.0f, 0.0f, 0.0f);


    // Lerp (Bewegung) Variables
    //public Transform startPosition; // Startposition des Zuges
    [Header("LERP/Bewegung-Einstellungen")]    
    [Tooltip("End position of train, dependend on empty GO position.")]
    public Transform endPosition; // Endposition des Zuges

    [Tooltip("Duration value for drive in time of the train.")]
    [Range(0.0f, 10f)]
    public float duration; // Dauer der Zugfahrt in Sekunden
    public float decoupleDuration; // Dauer der Dekoppelieurng in Sekunden

    private bool isMoving = false; // Wird true, wenn der Zug sich bewegt
    private Sequence movementSequence; // DOTween Bewegungssequenz

    private GameObject locomotive;
    private GameObject[] wagons;  // Ein Array, um alle erzeugten Wagons zu speichern
    #endregion

    void Start()
    {
        // Spawn Locomotive
        locomotive = Instantiate(locomotivePrefab, transform.position, transform.rotation);

        // Spawne Wagons
        SpawnWagons();

        // Posiitoniere den gesamten Zug
        PositionTrain(locomotive);
    }

    void SpawnWagons()
    {
        wagons = new GameObject[numWagons];  // Initialisiere das Wagon-Array mit der Anzahl der zu spawnenden Wagons

        // Spawne die Wagons nacheinander mit einem Abstand von wagonSpacing
        for (int i = 0; i < numWagons; i++)
        {
            Vector3 wagonPosition = transform.position + (i + 1) * -wagonSpacing * transform.forward;  // Berechne die Position des Wagons basierend auf der Position und Rotation der Locomotive
            Quaternion wagonRotation = locomotive.transform.rotation;  // Der Wagon hat die gleiche Rotation wie die Locomotive
            wagons[i] = Instantiate(wagonPrefab, wagonPosition, wagonRotation);  // Erzeuge den Wagon

            wagons[i].transform.parent = locomotive.transform;  // Setze das Wagon-Objekt als Kind des Locomotive-Objekts 
            // dh. alle Transformationsänderungen, die am Locomotive-Objekt vorgenommen werden, wirken sich auch auf das Wagon-Objekt aus
        }
    }

    void PositionTrain(GameObject locomotiveGO)
    {
        // Verschiebe den gesamten Zug auf der Position trainSpawnPosition
        locomotive.transform.position = trainSpawnPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            // Zugbewegung starten
            isMoving = true;
            movementSequence = DOTween.Sequence();
            movementSequence.Append(locomotive.transform.DOMove(endPosition.position, duration).SetEase(Ease.OutCubic))
                .OnComplete(() => {
                    isMoving = false;
                    Debug.Log("Gestoppt!");

                   
                    // Dekoppeln des Zuges
                    for (int i = 0; i < numWagons; i++)
                    {
                        Vector3 wagonPosition = locomotive.transform.position + (i + 1) * -2 * locomotive.transform.forward; // Berechne die neue Position des Wagens
                        wagons[i].transform.DOMove(wagonPosition, decoupleDuration).SetEase(Ease.OutCubic); // Animiere die Position des Wagens

                        // Ggf. Zeitpause damit jeder Wagon nacheinander dekoppelt
                    }
                });
        }
    }   
}