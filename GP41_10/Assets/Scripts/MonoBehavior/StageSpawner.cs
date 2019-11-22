using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    public GameObject Cube;
    public GameObject[] SeaArea;
    public GameObject Bow;
    public GameObject Ship;
    public GameObject Obstacle;
    [SerializeField, Header("海域の配置")]
    private Vector3[] SeaAreaPos;
    [SerializeField, Header("海流の方向の矢印の配置")]
    private Vector3[] SeaAreaBowPos;
    [SerializeField, Header("海流の方向")]
    private float[] SeaAreaAngle;
    [SerializeField, Header("海流の速度")]
    private float[] SeaAreaSpeed;
    [SerializeField, Header("船の位置")]
    private Vector3[] ShipPos = new Vector3[4];
    private GameObject SeaAreaObj;
    private GameObject Bowobj;
    private GameObject Cubeobj;
    private List<GameObject> DrowingPersons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        // おぼれている人
        for (int z = 0; z < 5; ++z)
            for (int x = 0; x < 10; ++x)
            {
                if (Random.Range(0, 5) == 1)
                {
                    Cubeobj = Instantiate(Cube, new Vector3((x - 5) * 10f + 5f, 0f, (z - 2.5f) * 10f + 5f), Quaternion.identity);
                    Cubeobj.GetComponent<MeshRenderer>().material.color = Color.red;
                    DrowingPersons.Add(Cubeobj);
                }
            }

        // 海域
        for(int i = 0; i < 5; ++i)
        {
            SeaAreaObj = Instantiate(SeaArea[i], SeaAreaPos[i], Quaternion.identity);
            Instantiate(Bow, SeaAreaBowPos[i], Quaternion.Euler(0f, -SeaAreaAngle[i] + 90f, 0f));
            // 角度をラジアンに変換
            float rad = SeaAreaAngle[i] * Mathf.Deg2Rad;

            // それぞれの軸の成分を計算
            float x = Mathf.Cos(rad);
            float y = 0f;
            float z = Mathf.Sin(rad);

            // 流れの向きと速度を設定
            SeaAreaObj.GetComponent<OceanCurrent>().SetOceanCurrentVec(new Vector3(x, y, z));
            SeaAreaObj.GetComponent<OceanCurrent>().SetOceanCurrentSpeed(SeaAreaSpeed[i]);
        }

        // 障害物
        for(int x = 0; x < 11; ++x)
        {
            int z = Random.Range(0, 4);
            Instantiate(Obstacle, new Vector3((x - 5) * 6.0f, -5.5f, (z - 2) * 5.0f), Quaternion.identity);
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
        
    }

    public List<GameObject> GetDrowingPersons()
    {
        return DrowingPersons;
    }

    public Vector3[] GetShipPos()
    {
        return ShipPos;
    }
}
