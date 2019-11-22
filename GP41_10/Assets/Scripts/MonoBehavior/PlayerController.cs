using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Cursor;
    public GameObject Target;
    public GameObject Torpedo;
    public GameObject Torpedocamera;
    public GameObject cameraController;
    private CursorBehavior cursorBehavior;
    [SerializeField, Header("カメラと魚雷とのオフセット（x:左右、y:上下、z:前後）")]
    private Vector3 offset = new Vector3(0f, 0f, 0f); // オフセット
    private List<GameObject> targets = new List<GameObject>();
    private List<GameObject> RescueTarget = new List<GameObject>();
    private List<GameObject> cameraList = new List<GameObject>();
    private GameObject StageInfo;

    // Start is called before the first frame update
    void Start()
    {
        cursorBehavior = Cursor.GetComponent<CursorBehavior>();
        StageInfo = GameObject.Find("StageSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        //---------- カーソル移動 ----------
        if(Input.GetKey(KeyCode.W))
        {
            cursorBehavior.CursorMove(KeyCode.W);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cursorBehavior.CursorMove(KeyCode.S);
        }
        if (Input.GetKey(KeyCode.A))
        {
            cursorBehavior.CursorMove(KeyCode.A);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cursorBehavior.CursorMove(KeyCode.D);
        }

        //---------- ターゲット生成 ----------
        if(Input.GetKeyDown(KeyCode.Space) && targets.Count < 4)
        {
            SpawnTarget();
        }

        //---------- 魚雷発射 ----------
        if(Input.GetKeyDown(KeyCode.Return) && targets.Count != 0)
        {
            GameObject TorpedoObj;
            GameObject TorpedocameraObj;
            StageSpawner stageSpawner = StageInfo.GetComponent<StageSpawner>();
            for (int i = 0; i < targets.Count; ++i)
            {
                RescueTarget = targets[i].GetComponent<TargetBehavior>().TorpedoShoot();
                switch(i)
                {
                    case 0:
                        TorpedoObj = Instantiate(Torpedo, stageSpawner.GetShipPos()[0], Quaternion.Euler(0f, 0f, 0f));
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
                        TorpedocameraObj = Instantiate(Torpedocamera, TorpedoObj.transform.position + TorpedoObj.transform.up * offset.y + TorpedoObj.transform.forward * offset.z, Quaternion.identity);
                        TorpedocameraObj.GetComponent<TorpedoCamera>().SetTorpedo(TorpedoObj, offset);
                        cameraList.Add(TorpedocameraObj);
                        break;
                    case 1:
                        TorpedoObj = Instantiate(Torpedo, stageSpawner.GetShipPos()[1], Quaternion.Euler(0f, 0f, 0f));
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
                        TorpedocameraObj = Instantiate(Torpedocamera, TorpedoObj.transform.position + TorpedoObj.transform.up * offset.y + TorpedoObj.transform.forward * offset.z, Quaternion.identity);
                        TorpedocameraObj.GetComponent<TorpedoCamera>().SetTorpedo(TorpedoObj, offset);
                        cameraList.Add(TorpedocameraObj);
                        break;
                    case 2:
                        TorpedoObj = Instantiate(Torpedo, stageSpawner.GetShipPos()[2], Quaternion.Euler(0f, 0f, 0f));
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
                        TorpedocameraObj = Instantiate(Torpedocamera, TorpedoObj.transform.position + TorpedoObj.transform.up * offset.y + TorpedoObj.transform.forward * offset.z, Quaternion.identity);
                        TorpedocameraObj.GetComponent<TorpedoCamera>().SetTorpedo(TorpedoObj, offset);
                        cameraList.Add(TorpedocameraObj);
                        break;
                    case 3:
                        TorpedoObj = Instantiate(Torpedo, stageSpawner.GetShipPos()[3], Quaternion.Euler(0f, 0f, 0f));
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
                        TorpedocameraObj = Instantiate(Torpedocamera, TorpedoObj.transform.position + TorpedoObj.transform.up * offset.y + TorpedoObj.transform.forward * offset.z, Quaternion.identity);
                        TorpedocameraObj.GetComponent<TorpedoCamera>().SetTorpedo(TorpedoObj, offset);
                        cameraList.Add(TorpedocameraObj);
                        break;
                    default:
                        break;
                }
            }
            switch(targets.Count)
            {
                case 1:
                    cameraController.GetComponent<CameraController>().ChangeCameraState(CameraController.CameraState.SHOOT_ONE, cameraList);
                    break;
                case 2:
                    cameraController.GetComponent<CameraController>().ChangeCameraState(CameraController.CameraState.SHOOT_TWO, cameraList);
                    break;
                case 3:
                    cameraController.GetComponent<CameraController>().ChangeCameraState(CameraController.CameraState.SHOOT_THREE, cameraList);
                    break;
                case 4:
                    cameraController.GetComponent<CameraController>().ChangeCameraState(CameraController.CameraState.SHOOT_FOUR, cameraList);
                    break;
                default:
                    break;
            }
        }

        //---------- ターゲットリセット ----------
        if (Input.GetKey(KeyCode.LeftShift) && targets.Count != 0)
        {
            ResetTarget();
        }
    }

    private void SpawnTarget()
    {
        GameObject target;
        target = Instantiate(Target, Cursor.transform.position, Quaternion.identity);
        targets.Add(target);
    }

    private void ResetTarget()
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
}
