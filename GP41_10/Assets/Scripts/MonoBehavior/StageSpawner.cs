using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    public GameObject Cube;
    public GameObject SeaArea;
    public GameObject Bow;
    public GameObject Ship;
    public GameObject cursor;
    public GameObject Target;
    public GameObject Torpedo;
    public GameObject Torpedocamera;
    public GameObject cameraController;
    public GameObject stageTimer;
    public GameObject[] Obstacle = new GameObject[4];
    [SerializeField, Header("海流の方向の矢印の配置")]
    private Vector3[] SeaAreaBowPos;
    [SerializeField, Header("海流の方向")]
    private float[] SeaAreaAngle;
    [SerializeField, Header("海流の速度")]
    private float[] SeaAreaSpeed;
    [SerializeField, Header("船の位置")]
    private Vector3[] ShipPos = new Vector3[4];
    [SerializeField, Header("カメラと魚雷とのオフセット（x:左右、y:上下、z:前後）")]
    private Vector3 offset = new Vector3(0f, 0f, 0f); // オフセット
    private int AddPersonLimit;
    private GameObject SeaAreaObj;
    private GameObject Bowobj;
    private GameObject Cubeobj;
    private List<GameObject> DrowingPersons = new List<GameObject>();
    private List<GameObject> targets = new List<GameObject>();
    private List<GameObject> RescueTarget = new List<GameObject>();
    private List<GameObject> cameraList = new List<GameObject>();
    private List<GameObject> TorpedoList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // ステージ情報リセット
        StageData.Reset();

        // おぼれている人
        for (int z = 0; z < 5; ++z)
            for (int x = 0; x < 10; ++x)
            {
                if (Random.Range(0, 5) == 1)
                {
                    Cubeobj = Instantiate(Cube, new Vector3((x - 5) * 15f + 5f, 0f, (z - 2.5f) * 15f + 5f), Quaternion.identity);
                    Cubeobj.GetComponent<MeshRenderer>().material.color = Color.red;
                    DrowingPersons.Add(Cubeobj);
                }
            }

        AddPersonLimit = 2;

        // 海域
        SeaAreaObj = Instantiate(SeaArea, Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 5; ++i)
        {
            Instantiate(Bow, SeaAreaBowPos[i], Quaternion.Euler(0f, -SeaAreaAngle[i] + 90f, 0f));
            // 角度をラジアンに変換
            float rad = SeaAreaAngle[i] * Mathf.Deg2Rad;

            // それぞれの軸の成分を計算
            float x = Mathf.Cos(rad);
            float y = 0f;
            float z = Mathf.Sin(rad);

            // 流れの向きと速度を設定
            SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentVec(new Vector3(x, y, z));
            SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentSpeed(SeaAreaSpeed[i]);
        }

        // 障害物
        for (int x = 0; x < 16; ++x)
        {
            int z = Random.Range(0, 6);
            int rock = Random.Range(0, 4);
            Instantiate(Obstacle[rock], new Vector3((x - 5) * 6.0f, -7f, (z - 2) * 8.0f), Quaternion.identity);
        }

        // 船
        for(int i = 0; i < 4; ++i)
        {
            Instantiate(Ship, ShipPos[i], Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int minutes = Mathf.FloorToInt(stageTimer.GetComponent<RemainTime>().GetRemainTime() / 60F);
        int seconds = Mathf.FloorToInt(stageTimer.GetComponent<RemainTime>().GetRemainTime() - minutes * 60);
        if (minutes == 2 && seconds == 0 && AddPersonLimit == 2)
        {
            // おぼれている人追加
            for (int z = 0; z < 5; ++z)
                for (int x = 0; x < 10; ++x)
                {
                    if (Random.Range(0, 5) == 1)
                    {
                        Cubeobj = Instantiate(Cube, new Vector3((x - 5) * 15f + 5f, 0f, (z - 2.5f) * 15f + 5f), Quaternion.identity);
                        Cubeobj.GetComponent<MeshRenderer>().material.color = Color.red;
                        DrowingPersons.Add(Cubeobj);
                    }
                }
            AddPersonLimit--;
        }
        else if(minutes == 1 && seconds == 0 && AddPersonLimit == 1)
        {
            // おぼれている人追加
            for (int z = 0; z < 5; ++z)
                for (int x = 0; x < 10; ++x)
                {
                    if (Random.Range(0, 5) == 1)
                    {
                        Cubeobj = Instantiate(Cube, new Vector3((x - 5) * 15f + 5f, 0f, (z - 2.5f) * 15f + 5f), Quaternion.identity);
                        Cubeobj.GetComponent<MeshRenderer>().material.color = Color.red;
                        DrowingPersons.Add(Cubeobj);
                    }
                }
            AddPersonLimit--;
        }
    }

    public List<GameObject> GetDrowingPersons()
    {
        return DrowingPersons;
    }

    public Vector3[] GetShipPos()
    {
        return ShipPos;
    }

    public List<GameObject> GetTargets()
    {
        return targets;
    }

    public List<GameObject> GetTorpedo()
    {
        return TorpedoList;
    }

    public List<GameObject> GetTorpedoCamera()
    {
        return cameraList;
    }

    public void SpawnTarget()
    {
        GameObject target;
        target = Instantiate(Target, cursor.transform.position, Quaternion.identity);
        targets.Add(target);
    }

    public void ResetTarget()
    {
        GameObject target;
        int count = targets.Count;
        for (int i = 0; i < count; ++i)
        {
            target = targets[0];
            targets.RemoveAt(0);
            target.GetComponent<TargetBehavior>().TargetDelete();
        }
    }

    public void ResetTorpedo()
    {
        GameObject torpedo;
        int count = TorpedoList.Count;
        for (int i = 0; i < count; ++i)
        {
            torpedo = TorpedoList[0];
            TorpedoList.RemoveAt(0);
            torpedo.GetComponent<TorpedoBehavior>().DestTorpedo();
        }
    }

    public void ResetTorpedoCamera()
    {
        GameObject torpedoCamera;
        int count = cameraList.Count;
        for (int i = 0; i < count; ++i)
        {
            torpedoCamera = cameraList[0];
            cameraList.RemoveAt(0);
            torpedoCamera.GetComponent<TorpedoCamera>().DestTorpedoCamera();
        }
    }

    public void RescueDrowningPerson()
    {
        GameObject drowningPerson;
        for(int i = 0; i < DrowingPersons.Count; i++)
        {
            if(DrowingPersons[i].GetComponent<DrowningPersonBehavior>().GetRescued())
            {
                drowningPerson = DrowingPersons[i];
                DrowingPersons.RemoveAt(i);
                drowningPerson.GetComponent<DrowningPersonBehavior>().DestDrowningPerson();
                i--;
            }
        }
    }

    public void TorpedoShoot()
    {
        GameObject TorpedoObj;
        GameObject TorpedocameraObj;
        for (int i = 0; i < targets.Count; ++i)
        {
            RescueTarget = targets[i].GetComponent<TargetBehavior>().TorpedoShoot();
            TorpedoObj = Instantiate(Torpedo, ShipPos[i], Quaternion.Euler(0f, 0f, 0f));
            if (RescueTarget != null)
            {
                for (int j = 0; j < RescueTarget.Count; ++j)
                {
                    TorpedoObj.GetComponent<TorpedoBehavior>().SetDrowningPerson(RescueTarget[j]);
                }
            }
            else
            {
                TorpedoObj.GetComponent<TorpedoBehavior>().SetTarget(targets[i]);
            }
            TorpedoList.Add(TorpedoObj);
            TorpedocameraObj = Instantiate(Torpedocamera, TorpedoObj.transform.position + TorpedoObj.transform.up * offset.y + TorpedoObj.transform.forward * offset.z, Quaternion.identity);
            TorpedocameraObj.GetComponent<TorpedoCamera>().SetTorpedo(TorpedoObj, offset);
            cameraList.Add(TorpedocameraObj);
        }

        cameraController.GetComponent<CameraController>().SetCameraList(cameraList);
    }
}
