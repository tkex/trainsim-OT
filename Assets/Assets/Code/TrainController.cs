using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    
    public GameObject locomotivePrefab;
    public GameObject wagonPrefab;

    // Configurator variables
    public int numWagons = 5;
    public float wagonSpacing = 1.0f;
    public Vector3 trainPosition = new Vector3(0.0f, 0.0f, 0.0f);

    void Start()
    {
        // Spawn locomotive
        GameObject locomotive = Instantiate(locomotivePrefab, trainPosition, transform.rotation);

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
}