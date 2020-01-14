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
    private GameObject stagestate;
    private GameObject stagespawner;
    public GameObject DispPerson;
    private GameObject DrowingPerson;
    private Vector3 vec = new Vector3(0f, 0f, 0f);
    private float speed = 0f;
    private bool Rescue = false;
    private bool destflg = false;
    // Start is called before the first frame update
    void Start()
    {
        stagestate = GameObject.Find("StageState");
        if(stagespawner == null)
        {
            stagespawner = GameObject.Find("StageSpawner");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(stagestate.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.START ||
            stagestate.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.TIME_UP)
        {
            return;
        }
        // 救助されていなければ移動
        if (!Rescue)
        {
            transform.position += vec * speed * Time.deltaTime;
        }

        transform.rotation = Quaternion.identity;

        // マップカメラに位置表示
        if (DispTime > 0f)
        {
            DispTime -= Time.deltaTime;
            if (DispTime <= 0f && DrowingPerson != null)
            {
                Destroy(DrowingPerson);
                DispTime = 0f;
            }
        }

        if (destflg)
        {
            if (DrowingPerson != null)
            {
                Destroy(DrowingPerson);
                DispTime = 0f;
            }
            Destroy(this.gameObject);
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

    // 流される向きと速さ
    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag == "SeaArea")
        {
            vec += collider.gameObject.GetComponent<OceanCurrent>().GetOceanCurrentVec();
            vec = vec.normalized;
            speed = collider.gameObject.GetComponent<OceanCurrent>().GetOceanCurrentSpeed();
        }
    }

    // レーダー表示
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Island" && !Rescue)
        {
            Rescued();
            if(stagespawner == null)
            {
                stagespawner = GameObject.Find("StageSpawner");
            }
            stagespawner.GetComponent<StageSpawner>().RescueDrowningPerson();
        }
        if (collider.gameObject.tag == "Rader" && !Rescue)
        {
            Vector3 pos = new Vector3(transform.position.x, 5f, transform.position.z);
            DrowingPerson = Instantiate(DispPerson, pos, Quaternion.identity);
            DrowingPerson.GetComponent<MeshRenderer>().material.color = Color.red;
            DispTime = DispMaxTime;
        }
    }

    // 救出された
    public void Rescued()
    {
        if (DrowingPerson != null)
        {
            Destroy(DrowingPerson);
            DispTime = 0f;
        }
        Rescue = true;
    }

    // 救助されたかどうか
    public bool GetRescued()
    {
        return Rescue;
    }

    // おぼれている人消去
    public void DestDrowningPerson()
    {
        destflg = true;
    }
}
