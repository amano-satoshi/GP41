﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    [SerializeField, Header("救出成功範囲")]
    private float SuccessRange = 0f;
    private GameObject stageSpawer;
    private List<GameObject> DrowingPersons = new List<GameObject>();
    private List<GameObject> RescuePersons = new List<GameObject>();
    private bool shootflg = false;
    // Start is called before the first frame update
    void Start()
    {
        stageSpawer = GameObject.Find("StageSpawner");
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> TorpedoShoot()
    {
        if (shootflg) return null;
        shootflg = true;
        DrowingPersons = stageSpawer.GetComponent<StageSpawner>().GetDrowingPersons();
        for(int i = 0; i < DrowingPersons.Count; ++i)
        {
            if(!DrowingPersons[i].GetComponent<DrowningPersonBehavior>().GetRescued() && ((DrowingPersons[i].transform.position - transform.position).magnitude <= SuccessRange))
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
                DrowingPersons[i].GetComponent<DrowningPersonBehavior>().Rescued();
                RescuePersons.Add(DrowingPersons[i]);
            }
        }
        if(RescuePersons.Count != 0)
        {
            return RescuePersons;
        }
        else
            return null;
    }

    public void TargetDelete()
    {
        Destroy(this.gameObject);
    }
}
