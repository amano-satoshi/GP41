///////////////////////////////////////////////////////////////
//
// ステージ生成(StageSpawner : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/20
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSpawner : MonoBehaviour
{
    public GameObject Cube;
    public GameObject SeaArea;
    public GameObject Bow;
    public GameObject Ship;
    public GameObject cursor1;
    public GameObject cursor2;
    public GameObject Target;
    public GameObject Torpedo;
    public GameObject Torpedo1p;
    public GameObject Torpedocamera;
    public GameObject cameraController;
    public GameObject stageTimer;
    public GameObject stageState;
    public GameObject oceanInput;
    public Canvas startProduction;
    public Canvas ShootProduction;
    public Canvas TorpedoLearning;
    public Canvas EndProduction;
    public GameObject[] Obstacle = new GameObject[4];
    [SerializeField, Header("デバッグモード")]
    private bool DebugMode = false;
    [SerializeField, Header("海流の方向の矢印の配置")]
    private Vector3[] SeaAreaBowPos;
    [SerializeField, Header("海流の方向")]
    private float[] SeaAreaAngle;
    [SerializeField, Header("海流の速度")]
    private float[] SeaAreaSpeed;
    [SerializeField, Header("海の荒さの度合")]
    [Range(0, 13)] private int SeaPowerLevel = 7;
    [SerializeField, Header("海の荒さ")]
    [Range(-6f, 5f)] private float[] SeaPower = new float[3];
    [SerializeField, Header("船の位置")]
    private Vector3[] ShipPos = new Vector3[4];
    [SerializeField, Header("カメラと魚雷とのオフセット（x:左右、y:上下、z:前後）")]
    private Vector3 offset = new Vector3(0f, 0f, 0f); // オフセット
    [SerializeField, Header("矢印の大きさの幅")]
    private float BowSize = 2.5f;
    [SerializeField, Header("矢印の拡大縮小速度")]
    private float BowChangeSpeed = 10f;
    private bool AddPersonLimit = false;
    private int[] AddPersonLimitNum = { 3, 5, 7 };
    private GameObject SeaAreaObj;
    private GameObject Bowobj;
    private GameObject Cubeobj;
    private Canvas StartProductionObj;
    private Canvas ShootProductionObj;
    private Canvas TorpedoLearningObj;
    private Canvas EndProductionObj;
    private List<GameObject> DrowingPersons = new List<GameObject>();
    private List<GameObject> targets = new List<GameObject>();
    private List<GameObject> targets1p = new List<GameObject>();
    private List<GameObject> RescueTarget = new List<GameObject>();
    private List<GameObject> cameraList = new List<GameObject>();
    private List<GameObject> TorpedoList = new List<GameObject>();
    private List<GameObject> BowList = new List<GameObject>();
    private List<GameObject> CursorList = new List<GameObject>();
    private List<int> RemainTorpedoList = new List<int>();
    private int[] RandomWave = new int[5];
    private int WaveSpeedChange = 5;
    private bool[] BowUpFlg = { false, false, false, false, false };

    public GameObject mapCamera;

    public AudioClip[] sounds = new AudioClip[4];
    AudioSource[] audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // ステージ情報リセット
        StageData.Reset();

        audioSource = mapCamera.GetComponents<AudioSource>();

        audioSource[0].Play();

        // おぼれている人
        if(DebugMode)
        {
            for (int x = 0; x < 10; ++x)
            {
                Cubeobj = Instantiate(Cube, new Vector3((x - 5) * 15f + 5f, -5f, (x % 5 - 2) * 15f), Quaternion.identity);
                Cubeobj.GetComponent<DrowningPersonBehavior>().SetID(x);
                DrowingPersons.Add(Cubeobj);                
            }
        }
        else
        {
            for (int x = 0; x < 10; ++x)
            {
                Cubeobj = Instantiate(Cube, new Vector3((x - 5) * 15f + 5f, -5f, (Random.Range(0, 5) - 2.5f) * 15f + 5f), Quaternion.identity);
                Cubeobj.GetComponent<DrowningPersonBehavior>().SetID(x);
                DrowingPersons.Add(Cubeobj);
            }
        }
        AddPersonLimit = true;

        // 海域
        SeaAreaObj = Instantiate(SeaArea, new Vector3(0f, -4f, 0f), Quaternion.Euler(0f, 180f, 0f));
        for (int i = 0; i < 5; ++i)
        {
            // 矢印
            Bowobj = Instantiate(Bow, SeaAreaBowPos[i], Quaternion.Euler(0f, -SeaAreaAngle[i] + 90f, 0f));
            BowList.Add(Bowobj);

            // 角度をラジアンに変換
            float rad = SeaAreaAngle[i] * Mathf.Deg2Rad;

            // それぞれの軸の成分を計算
            float x = Mathf.Cos(rad);
            float y = 0f;
            float z = Mathf.Sin(rad);

            // 流れの向きと速度を設定
            SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentVec(new Vector3(x, y, z));
            SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentSpeed(SeaAreaSpeed[0]);
        }
        oceanInput.transform.GetChild(0).GetComponent<Crest.ShapeGerstnerBatched>()._spectrum.SetWavePower(SeaPowerLevel, SeaPower[0]);
        Debug.Log(SeaAreaObj.transform.GetChild(0).GetComponent<OceanCurrent>().GetOceanCurrentSpeed());
        audioSource[1].clip = sounds[0];
        audioSource[1].loop = true;
        audioSource[1].Play();

        // 船
        for (int i = 0; i < 4; ++i)
        {
            Instantiate(Ship, ShipPos[i], Quaternion.Euler(0f, i * 90f, 0f));
        }

        // カーソル
        if (StageData.GetPlayerNum() == 0)
        {
            GameObject cursorObj = Instantiate(cursor1, new Vector3(0f, 1f, 0f), Quaternion.Euler(0f, 180f, 0f));
            CursorList.Add(cursorObj);
            int remainTorpedo = 4;
            RemainTorpedoList.Add(remainTorpedo);
        }
        else if (StageData.GetPlayerNum() == 1)
        {
            GameObject cursorObj = Instantiate(cursor1, new Vector3(40f, 1f, 0f), Quaternion.Euler(0f, 180f, 0f));
            cursorObj.GetComponent<MeshRenderer>().material.color = Color.red;
            CursorList.Add(cursorObj);
            int remainTorpedo = 2;
            RemainTorpedoList.Add(remainTorpedo);
            cursorObj = Instantiate(cursor2, new Vector3(-40f, 1f, 0f), Quaternion.Euler(0f, 180f, 0f));
            cursorObj.GetComponent<MeshRenderer>().material.color = Color.blue;
            CursorList.Add(cursorObj);
            RemainTorpedoList.Add(remainTorpedo);
        }

        // 波の変化のタイミング
        RandomWave[0] = Random.Range(150, 161);
        RandomWave[1] = Random.Range(120, 131);
        RandomWave[2] = Random.Range(90, 101);
        RandomWave[3] = Random.Range(60, 71);
        RandomWave[4] = Random.Range(30, 41);

        // 開始演出準備
        StartProductionObj = Instantiate(startProduction);
    }

    // Update is called once per frame
    void Update()
    {
        int times = stageTimer.GetComponent<RemainTime>().GetRemainTime();

        if(DebugMode)
        {
            if(Input.GetKeyDown(KeyCode.F6))
            {
                for (int x = 0; x < 10; ++x)
                {
                    Cubeobj = Instantiate(Cube, new Vector3((x - 5) * 15f + 5f, -5f, (x % 5 - 2) * -15f), Quaternion.identity);
                    DrowingPersons.Add(Cubeobj);
                }
            }
        }
        else
        {
            if (times % 10 == 0 && !AddPersonLimit)
            {
                // おぼれている人追加
                if (SeaAreaObj.transform.GetChild(0).GetComponent<OceanCurrent>().GetOceanCurrentSpeed() == 0.5f)
                {
                    for (int i = 0; i < AddPersonLimitNum[0]; ++i)
                    {
                        Cubeobj = Instantiate(Cube, new Vector3(Random.Range(0, AddPersonLimitNum[0]) * 15f + i * 50f - 65f, -5f, Random.Range(0, 11) * 5f - 25f), Quaternion.identity);
                        Cubeobj.GetComponent<DrowningPersonBehavior>().SetID(DrowingPersons.Count);
                        DrowingPersons.Add(Cubeobj);
                    }
                }
                else if (SeaAreaObj.transform.GetChild(0).GetComponent<OceanCurrent>().GetOceanCurrentSpeed() == 1.25f)
                {
                    for (int i = 0; i < AddPersonLimitNum[1]; ++i)
                    {
                        Cubeobj = Instantiate(Cube, new Vector3(Random.Range(0, AddPersonLimitNum[1]) * 7.5f + i * 25f - 65f, -5f, Random.Range(0, 11) * 5f - 25f), Quaternion.identity);
                        Cubeobj.GetComponent<DrowningPersonBehavior>().SetID(DrowingPersons.Count);
                        DrowingPersons.Add(Cubeobj);
                    }
                }
                else if (SeaAreaObj.transform.GetChild(0).GetComponent<OceanCurrent>().GetOceanCurrentSpeed() == 2f)
                {
                    for (int i = 0; i < AddPersonLimitNum[2]; ++i)
                    {
                        Cubeobj = Instantiate(Cube, new Vector3(Random.Range(0, AddPersonLimitNum[2]) * 5f + i * 17f - 65f, -5f, Random.Range(0, 11) * 5f - 25f), Quaternion.identity);
                        Cubeobj.GetComponent<DrowningPersonBehavior>().SetID(DrowingPersons.Count);
                        DrowingPersons.Add(Cubeobj);
                    }
                }
                AddPersonLimit = true;
            }
            else if (times % 10 != 0 && AddPersonLimit)
            {
                AddPersonLimit = false;
            }
        }

        // 波の荒さ変更
        if(DebugMode)
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                for (int i = 0; i < 5; ++i)
                {
                    SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentSpeed(SeaAreaSpeed[0]);
                }
                oceanInput.transform.GetChild(0).GetComponent<Crest.ShapeGerstnerBatched>()._spectrum.SetWavePower(SeaPowerLevel, SeaPower[0]);
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                for (int i = 0; i < 5; ++i)
                {
                    SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentSpeed(SeaAreaSpeed[1]);
                }
                oceanInput.transform.GetChild(0).GetComponent<Crest.ShapeGerstnerBatched>()._spectrum.SetWavePower(SeaPowerLevel, SeaPower[1]);
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                for (int i = 0; i < 5; ++i)
                {
                    SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentSpeed(SeaAreaSpeed[2]);
                }
                oceanInput.transform.GetChild(0).GetComponent<Crest.ShapeGerstnerBatched>()._spectrum.SetWavePower(SeaPowerLevel, SeaPower[2]);
            }
        }
        else
        {
            if ((times == RandomWave[0] && WaveSpeedChange == 5) || (times == RandomWave[1] && WaveSpeedChange == 4) ||
            (times == RandomWave[2] && WaveSpeedChange == 3) || (times == RandomWave[3] && WaveSpeedChange == 2) ||
            (times == RandomWave[4] && WaveSpeedChange == 1))
            {
                int randomwave = Random.Range(0, 3);
                audioSource[1].Stop();
                for (int i = 0; i < 5; ++i)
                {
                    SeaAreaObj.transform.GetChild(i).GetComponent<OceanCurrent>().SetOceanCurrentSpeed(SeaAreaSpeed[randomwave]);
                }
                audioSource[1].clip = sounds[randomwave];
                audioSource[1].loop = true;
                audioSource[1].Play();
                if (randomwave == 0)
                {
                    oceanInput.transform.GetChild(0).GetComponent<Crest.ShapeGerstnerBatched>()._spectrum.SetWavePower(SeaPowerLevel, SeaPower[0]);
                }
                else if (randomwave == 1)
                {
                    oceanInput.transform.GetChild(0).GetComponent<Crest.ShapeGerstnerBatched>()._spectrum.SetWavePower(SeaPowerLevel, SeaPower[1]);
                }
                else if (randomwave == 2)
                {
                    oceanInput.transform.GetChild(0).GetComponent<Crest.ShapeGerstnerBatched>()._spectrum.SetWavePower(SeaPowerLevel, SeaPower[2]);
                }
                Debug.Log(SeaAreaObj.transform.GetChild(0).GetComponent<OceanCurrent>().GetOceanCurrentSpeed());
                WaveSpeedChange--;
            }
        }

        // 矢印の拡大縮小
        for(int i = 0; i < BowList.Count; ++i)
        {
            if(BowUpFlg[i])
            {
                BowList[i].transform.localScale += new Vector3(BowChangeSpeed * Time.deltaTime, -5f, BowChangeSpeed * Time.deltaTime);
                if(BowList[i].transform.localScale.x > 5f + BowSize)
                {
                    BowUpFlg[i] = false;
                }
            }
            else
            {
                BowList[i].transform.localScale -= new Vector3(BowChangeSpeed * Time.deltaTime, -5f, BowChangeSpeed * Time.deltaTime);
                if (BowList[i].transform.localScale.x < 5f - BowSize)
                {
                    BowUpFlg[i] = true;
                }
            }
        }

        if(stageState.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.SHOOT && ShootProductionObj == null)
        {
            // 発射演出準備
            ShootProductionObj = Instantiate(ShootProduction);
        }

        if(stageState.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.LEARNING && TorpedoLearningObj == null)
        {
            // 学習演出準備
            TorpedoLearningObj = Instantiate(TorpedoLearning);
        }

        if (stageState.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.TIME_UP && EndProductionObj == null)
        {
            // 終了演出準備
            EndProductionObj = Instantiate(EndProduction);
            audioSource[0].Stop();
        }

        // デバッグ用
        if(DebugMode)
        {
            // リザルトシーンへ
            if (Input.GetKeyDown(KeyCode.F1))
            {
                SceneManager.LoadScene("ResultScene");
            }
            // プレイヤーセレクトシーンへ
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SceneManager.LoadScene("PlayerSelect");
            }
            // 救出者数取得
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log(StageData.GetRescuePersonCnt());
            }
            // 学習ゲージ取得
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                Debug.Log(StageData.GetLearningGauge());
            }
            // 学習ゲージ最大
            if (Input.GetKeyDown(KeyCode.F7))
            {
                StageData.MaxLearningGauge();
            }
            // 救出者数増加
            else if(Input.GetKeyDown(KeyCode.F8))
            {
                StageData.SetIncreaseRescuePersonCnt(20);
            }
        }
    }

    public Canvas GetStartProduction()
    {
        return StartProductionObj;
    }

    public Canvas GetShootProduction()
    {
        return ShootProductionObj;
    }

    public Canvas GetTorpedoLearning()
    {
        return TorpedoLearningObj;
    }

    public Canvas GetEndProduction()
    {
        return EndProductionObj;
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

    public float GetOceanCurrentSpeed()
    {
        return SeaAreaObj.transform.GetChild(0).GetComponent<OceanCurrent>().GetOceanCurrentSpeed();
    }

    public float[] GetSeaAreaSpeed()
    {
        return SeaAreaSpeed;
    }

    public List<GameObject> GetCursor()
    {
        return CursorList;
    }

    public List<int> GetRemainTorpedo()
    {
        return RemainTorpedoList;
    }

    public void ResetRemainTorpedo()
    {
        if(StageData.GetPlayerNum() == 0)
        {
            RemainTorpedoList[0] = 4;
        }
        else if (StageData.GetPlayerNum() == 1)
        {
            RemainTorpedoList[0] = 2;
            RemainTorpedoList[1] = 2;
        }
    }

    public void SpawnTarget(int SpawnPlayer)
    {
        GameObject target;
        if (StageData.GetPlayerNum() == 0)
        {
            if (SpawnPlayer == 1)
            {
                target = Instantiate(Target, new Vector3(CursorList[0].transform.position.x, CursorList[0].transform.position.y, CursorList[0].transform.position.z - 1f), Quaternion.identity);
                targets.Add(target);
                audioSource[2].PlayOneShot(sounds[3]);
                RemainTorpedoList[0] -= 1;
            }
        }
        else if (StageData.GetPlayerNum() == 1)
        {
            if (SpawnPlayer == 1)
            {
                target = Instantiate(Target, new Vector3(CursorList[0].transform.position.x, CursorList[0].transform.position.y, CursorList[0].transform.position.z - 1f), Quaternion.identity);
                targets.Add(target);
                audioSource[2].PlayOneShot(sounds[3]);
                RemainTorpedoList[0] -= 1;
            }
            else if (SpawnPlayer == 2)
            {
                target = Instantiate(Target, new Vector3(CursorList[1].transform.position.x, CursorList[1].transform.position.y, CursorList[1].transform.position.z - 1f), Quaternion.identity);
                targets1p.Add(target);
                audioSource[2].PlayOneShot(sounds[3]);
                RemainTorpedoList[1] -= 1;
            }
        }
    }

    public void ResetTarget()
    {
        GameObject target;
        GameObject target2;
        int count = targets.Count;
        int count2 = targets1p.Count;
        for (int i = 0; i < count; ++i)
        {
            target = targets[0];
            targets.RemoveAt(0);
            target.GetComponent<TargetBehavior>().TargetDelete();
        }
        for (int i = 0; i < count2; ++i)
        {
            target2 = targets1p[0];
            targets1p.RemoveAt(0);
            target2.GetComponent<TargetBehavior>().TargetDelete();
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
                drowningPerson = DrowingPersons[i]; // 退避
                DrowingPersons.RemoveAt(i);         // リストから削除
                drowningPerson.GetComponent<DrowningPersonBehavior>().DestDrowningPerson(); // 削除
                // ID設定しなおし
                for (int j = 0; j < DrowingPersons.Count; j++)
                {
                    DrowingPersons[j].GetComponent<DrowningPersonBehavior>().SetID(j);
                }
                    i--;
            }
        }
    }

    public void TorpedoShoot()
    {
        GameObject TorpedoObj;
        GameObject TorpedocameraObj;
        if (StageData.GetPlayerNum() == 0)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                RescueTarget = targets[i].GetComponent<TargetBehavior>().TorpedoShoot();
                TorpedoObj = Instantiate(Torpedo, ShipPos[i] - new Vector3(0f, 3f, 0f), Quaternion.Euler(0f, i * 90f, 0f));
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
                TorpedocameraObj.GetComponent<TorpedoCamera>().SetCamID(i);
                cameraList.Add(TorpedocameraObj);
            }
        }
        else if (StageData.GetPlayerNum() == 1)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                RescueTarget = targets[i].GetComponent<TargetBehavior>().TorpedoShoot();
                TorpedoObj = Instantiate(Torpedo, ShipPos[i] - new Vector3(0f, 3f, 0f), Quaternion.Euler(0f, i * 90f, 0f));
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
                TorpedocameraObj.GetComponent<TorpedoCamera>().SetCamID(i);
                cameraList.Add(TorpedocameraObj);
            }
            for (int k = 0; k < targets1p.Count; ++k)
            {
                RescueTarget = targets1p[k].GetComponent<TargetBehavior>().TorpedoShoot();
                TorpedoObj = Instantiate(Torpedo1p, ShipPos[targets.Count + k] - new Vector3(0f, 3f, 0f), Quaternion.Euler(0f, (targets.Count + k) * 90f, 0f));
                if (RescueTarget != null)
                {
                    for (int l = 0; l < RescueTarget.Count; ++l)
                    {
                        TorpedoObj.GetComponent<TorpedoBehavior>().SetDrowningPerson(RescueTarget[l]);
                    }
                }
                else
                {
                    TorpedoObj.GetComponent<TorpedoBehavior>().SetTarget(targets1p[k]);
                }
                TorpedoList.Add(TorpedoObj);
                TorpedocameraObj = Instantiate(Torpedocamera, TorpedoObj.transform.position + TorpedoObj.transform.up * offset.y + TorpedoObj.transform.forward * offset.z, Quaternion.identity);
                TorpedocameraObj.GetComponent<TorpedoCamera>().SetTorpedo(TorpedoObj, offset);
                TorpedocameraObj.GetComponent<TorpedoCamera>().SetCamID(targets.Count + k);
                cameraList.Add(TorpedocameraObj);
            }
        }

        cameraController.GetComponent<CameraController>().SetCameraList(cameraList);
    }

    public bool GetDebugMode()
    {
        return DebugMode;
    }
}
