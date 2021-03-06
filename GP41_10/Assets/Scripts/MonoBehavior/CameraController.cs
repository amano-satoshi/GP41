﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public enum CameraState
    {
        DEFAULT = 0,    // マップカメラのみ
        SHOOT,          // 魚雷カメラとマップカメラ

        MAX_CAMERASTATE
    }
    public GameObject MapCamera;
    public GameObject stageState;
    public Canvas flame;
    private Canvas cameraFlame;
    private CameraState camerastate;
    private StageState state;
    private List<GameObject> CameraList = new List<GameObject>();
    [SerializeField, Header("マップカメラの配置"), CustomListLabelAttribute(new string[] {"発射前", "1機発射", "2機発射", "3機発射", "4機発射" })]
    private Rect[] MapRect = new Rect[5];
    [SerializeField, Header("UI部分を除いた画面の大きさ")]
    private Vector2 DispSize = new Vector2(0f, 0f);

    private bool changeflg = true;

    // Start is called before the first frame update
    void Start()
    {
        camerastate = CameraState.DEFAULT;
        state = stageState.GetComponent<StageState>();
        cameraFlame = Instantiate(flame);
        cameraFlame.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        cameraFlame.GetComponent<Canvas>().worldCamera = MapCamera.GetComponent<Camera>();
        cameraFlame.GetComponent<Canvas>().planeDistance = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state.GetStageState())
        {
            case StageState.STAGE_STATE.PREPARE:
                ResetCameraState();
                break;
            case StageState.STAGE_STATE.PRODUCTION:
                camerastate = CameraState.SHOOT;
                break;
            default:
                break;
        }

        SetCamera();
    }

    // 魚雷カメラのリスト作成
    public void SetCameraList(List<GameObject> cameraList)
    {
        if(cameraList != null)
        {
            CameraList = cameraList;
        }
    }

    // 魚雷カメラのリストの取得
    public List<GameObject> GetCameraList()
    {
        if (CameraList != null)
        {
            return CameraList;
        }
        else
        {
            return null;
        }
    }

    // 魚雷カメラのリストのリセット
    void ResetCameraState()
    {
        camerastate = CameraState.DEFAULT;
        CameraList.Clear();
    }

    // カメラ表示位置の指定
    void SetCamera()
    {
        if(camerastate == CameraState.DEFAULT)
        {
            if (changeflg)
            {
                MapCamera.GetComponent<Camera>().rect = MapRect[0];
                cameraFlame.transform.GetChild(0).GetComponent<Image>().enabled = false;
                changeflg = false;
            }
        }
        else if(camerastate == CameraState.SHOOT)
        {
            if(!changeflg)
            {
                MapCamera.GetComponent<Camera>().rect = MapRect[CameraList.Count];
                cameraFlame.transform.GetChild(0).GetComponent<Image>().enabled = true;
                switch (CameraList.Count)
                {
                    case 1:
                        CameraList[0].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x, DispSize.y);
                        break;
                    case 2:
                        CameraList[0].GetComponent<Camera>().rect = new Rect(0f, DispSize.y / 2f, DispSize.x, DispSize.y / 2f);
                        CameraList[1].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x, DispSize.y / 2f);
                        break;
                    case 3:
                        CameraList[0].GetComponent<Camera>().rect = new Rect(0f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                        CameraList[1].GetComponent<Camera>().rect = new Rect(DispSize.x / 2f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                        CameraList[2].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x / 2f, DispSize.y / 2f);
                        cameraFlame.transform.GetChild(0).GetComponent<Image>().rectTransform.localScale = new Vector2(
                            cameraFlame.transform.GetChild(0).GetComponent<Image>().rectTransform.localScale.x * 1.65f,
                            cameraFlame.transform.GetChild(0).GetComponent<Image>().rectTransform.localScale.y * 1.65f);
                        break;
                    case 4:
                        CameraList[0].GetComponent<Camera>().rect = new Rect(0f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                        CameraList[1].GetComponent<Camera>().rect = new Rect(DispSize.x / 2f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                        CameraList[2].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x / 2f, DispSize.y / 2f);
                        CameraList[3].GetComponent<Camera>().rect = new Rect(DispSize.x / 2f, 0f, DispSize.x / 2f, DispSize.y / 2f);
                        break;
                    default:
                        break;
                }
                changeflg = true;
            }
        }
    }
}
