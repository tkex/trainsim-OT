using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainController : MonoBehaviour
{
    // Prefab Definitionen
    public GameObject locomotivePrefab;
    public GameObject wagonPrefab;


    // Konfigurator Variabeln
    public int numWagons = 5;
    public float wagonSpacing = 1.0f;
    public Vector3 trainSpawnPosition = new Vector3(0.0f, 0.0f, 0.0f);


    // Lerp (Bewegung)
    //public Transform startPosition; // Startposition des Zuges
    public Transform endPosition; // Endposition des Zuges
    public float duration; // Dauer der Zugfahrt in Sekunden

    private bool isMoving = false; // Wird true, wenn der Zug sich bewegt
    private Sequence movementSequence; // DOTween Bewegungssequenz

    private GameObject locomotive;

    void Start()
    {
        // Spawn locomotive
        locomotive = Instantiate(locomotivePrefab, trainSpawnPosition, transform.rotation);

        /*
        // Spawn wagons
        List<GameObject> wagons = new List<GameObject>();
        for (int i = 0; i < numWagons; i++)
        {
            Vector3 wagonPos = transform.position + Vector3.forward * (i + 1) * wagonSpacing;
            GameObject wagon = Instantiate(wagonPrefab, wagonPos, transform.rotation);
            wagons.Add(wagon);
        }

        // Attach wagons to locomotive
        Rigidbody locomotiveRB = locomotive.GetComponent<Rigidbody>();
        foreach (GameObject wagon in wagons)
        {
            ConfigurableJoint joint = wagon.AddComponent<ConfigurableJoint>();
            joint.connectedBody = locomotiveRB;
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            joint.anchor = Vector3.zero;
            joint.connectedAnchor = Vector3.back * wagonSpacing;
        }
        */
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
                });
        }
    }
}