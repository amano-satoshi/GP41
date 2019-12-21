using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IwashiManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject fish;
    public int numFish;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(10, 10, 5);

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    // Use this for initialization
    void Start()
    {
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));

            allFish[i] = (GameObject)Instantiate(fish, pos, fish.transform.rotation);
            allFish[i].GetComponent<Iwashi>().myManager = this;
            allFish[i].transform.parent = fishPrefab.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
