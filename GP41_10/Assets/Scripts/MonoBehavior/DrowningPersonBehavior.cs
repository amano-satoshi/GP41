///////////////////////////////////////////////////////////////
//
// おぼれている人の挙動(DrowningPersonBehavior : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/18
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowningPersonBehavior : MonoBehaviour
{
    private int id;
    [SerializeField, Header("マップ画面に表示する時間")]
    private float DispMaxTime = 0f;
    private float DispTime = 0f;
    public GameObject DispPerson;
    private GameObject DrowingPerson;
    private Vector3 vec = new Vector3(0f, 0f, 0f);
    private float speed = 0f;
    private bool Rescue = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += vec * speed * Time.deltaTime;
        if(DispTime > 0f)
        {
            DispTime -= Time.deltaTime;
            if(DispTime <= 0f && DrowingPerson != null)
            {
                Destroy(DrowingPerson);
                DispTime = 0f;
            }
        }
    }

    public void SetID(int num)
    {
        id = num;
    }

    public int GetID()
    {
        return id;
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag == "SeaArea")
        {
            vec += collider.gameObject.GetComponent<OceanCurrent>().GetOceanCurrentVec();
            vec = vec.normalized;
            speed = collider.gameObject.GetComponent<OceanCurrent>().GetOceanCurrentSpeed();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Rader" && !Rescue)
        {
            DrowingPerson = Instantiate(DispPerson, transform.position, Quaternion.identity);
            DrowingPerson.GetComponent<MeshRenderer>().material.color = Color.red;
            DispTime = DispMaxTime;
        }
    }

    public void Rescued()
    {
        if (DrowingPerson != null)
        {
            Destroy(DrowingPerson);
            DispTime = 0f;
        }
        Rescue = true;
    }

    public bool GetRescued()
    {
        return Rescue;
    }
}
