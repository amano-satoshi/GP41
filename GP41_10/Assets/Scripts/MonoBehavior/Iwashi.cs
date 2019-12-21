using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iwashi : MonoBehaviour
{
    public IwashiManager myManager;
    float speed;

    // Use this for initialization
    void Start()
    {
        speed = Random.Range(myManager.minSpeed,
                             myManager.maxSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
        ApplyRules();

    }
    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance <= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Iwashi anotherFlock = go.GetComponent<Iwashi>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }

            if(go.transform.position.y > 2.5f)
            {
                go.transform.position = new Vector3(go.transform.position.x,
                    2.5f,
                    go.transform.position.z);
            }
        }


        if (groupSize < 8)
        {
            vcentre = vcentre / groupSize;
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                     Quaternion.LookRotation(direction),
                                     myManager.rotationSpeed * Time.deltaTime);

        }

        if(transform.position.x > 10 || transform.position.x < -10
            || transform.position.z > 30 || transform.position.z < 0)
        {
            Vector3 direction = new Vector3(0f, 0f, 0f) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                     Quaternion.LookRotation(direction),
                                     myManager.rotationSpeed * Time.deltaTime);

        }
    }
}