using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iwashi : MonoBehaviour
{
    public IwashiManager myManager;
    public GameObject iwashi;
    public float speed = 1.0f;
    float time = 10.0f;
    float intTime;

    GameObject[] gos;
    // Use this for initialization
    void Start()
    {
        intTime = Random.Range(2.0f, 4.0f);

        gos = myManager.allFish;
        //    speed = Random.Range(myManager.minSpeed,
        //                         myManager.maxSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;       // まっすぐ進む

        time -= 0.1f;
        if (time < 0.0f)
        {
            iwashi.transform.rotation = Quaternion.Euler(new Vector3(iwashi.transform.rotation.x + Random.Range(-0.5f, 0.5f),
                iwashi.transform.rotation.y + Random.Range(-15.5f, 15.5f),
                iwashi.transform.rotation.z + Random.Range(-0.5f, 0.5f)));
            time = 5.0f;
        }

        foreach (GameObject go in gos)
        {
            go.transform.rotation = iwashi.transform.rotation;
        }

        if(transform.position.y > 1.5f)
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
        }

        //    transform.Translate(Time.deltaTime * speed, Time.deltaTime * speed, Time.deltaTime * speed);
        // ApplyRules();

        /*
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

                        if (nDistance < 10.0f)
                        {
                            vavoid = vavoid + (this.transform.position - go.transform.position);
                        }

                        Iwashi anotherFlock = go.GetComponent<Iwashi>();
                        gSpeed = gSpeed + anotherFlock.speed;
                    }
                }
            }

            if (groupSize < 0)
            {
                vcentre = vcentre / groupSize;
                speed = gSpeed / groupSize;

                Vector3 direction = (vcentre + vavoid) - transform.position;
                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                                         Quaternion.LookRotation(direction),
                                         myManager.rotationSpeed * Time.deltaTime);

            }
        }
         */
    }
}