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
    Vector3 rand;
    // Use this for initialization
    void Start()
    {

        rand = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-90f, 90.0f), Random.Range(-0.5f, 0.5f));
        intTime = Random.Range(2.0f, 4.0f);

        gos = myManager.allFish;

        //    speed = Random.Range(myManager.minSpeed,
        //                         myManager.maxSpeed);
    }
    // Update is called once per frame
    void Update()
    {

        if (time < 0.0f)
        {
            rand = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-3, 3f), Random.Range(-0.5f, 0.5f));
            time = 5.0f;

            iwashi.transform.Rotate(iwashi.transform.rotation.x + rand.x,
                iwashi.transform.rotation.y + rand.y,
                iwashi.transform.rotation.z + rand.z);
            foreach (GameObject go in gos)
            {
                go.transform.Rotate(go.transform.rotation.x + rand.x,
                go.transform.rotation.y + rand.y,
                go.transform.rotation.z + rand.z);
            }
        }
        time -= 0.1f;
       

        foreach (GameObject go in gos)
        {
            //   go.transform.rotation = iwashi.transform.rotation;


            go.transform.position += go.transform.forward * Time.deltaTime * speed;       // まっすぐ進む

            
            
            if (go.transform.position.x > 80f)
            {
                go.transform.position = new Vector3(go.transform.position.x - 10, go.transform.position.y, go.transform.position.z);
            }

            if(go.transform.position.x < -80f)
            {
                go.transform.position = new Vector3(go.transform.position.x + 10, go.transform.position.y, go.transform.position.z);

            }

            if (go.transform.position.y > 2.5f)
            {
                go.transform.position = new Vector3(go.transform.position.x, 2.5f, go.transform.position.z);
            }

            if (go.transform.position.y < -1f)
            {
                go.transform.position = new Vector3(go.transform.position.x, -1f , go.transform.position.z);
            }

            if (go.transform.position.z > 100)
            {
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z - 10.0f);
            }

            if (go.transform.position.z < 5)
            {
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 5.0f);
            }

            if (go.transform.rotation.x > 5)
            {
                go.transform.Rotate(5, go.transform.rotation.y, go.transform.rotation.z);
            }

            if (go.transform.rotation.x < -5)
            {
                go.transform.Rotate(-5, go.transform.rotation.y, go.transform.rotation.z);
            }

            if (go.transform.rotation.z > 5)
            {
                go.transform.Rotate(go.transform.rotation.x, go.transform.rotation.y, 5f);
            }

            if (go.transform.rotation.z < -5)
            {
                go.transform.Rotate(go.transform.rotation.x, go.transform.rotation.y, -5f);
            }
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