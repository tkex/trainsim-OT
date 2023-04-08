using System.Collections.Generic;
using UnityEngine;

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
    public Transform startPosition; // Startposition des Zuges
    public Transform endPosition; // Endposition des Zuges
    public float duration; // Dauer der Zugfahrt in Sekunden

    private bool isMoving = false; // Wird true, wenn der Zug sich bewegt
    private float startTime; // Startzeit der Zugfahrt
    private Vector3 startVector; // Startvektor des Zuges
    private Vector3 endVector; // Endvektor des Zuges

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
            startTime = Time.time;
            startVector = startPosition.position;
            endVector = endPosition.position;
        }

        if (isMoving)
        {
            // Zugbewegung durchführen
            float t = (Time.time - startTime) / duration;

            locomotive.transform.position = Vector3.Lerp(startVector, endVector, t);

            if (t >= 1f)
            {
                // Zugbewegung abgeschlossen
                isMoving = false;
                Debug.Log("Gestoppt!");
            }
        }
    }
}