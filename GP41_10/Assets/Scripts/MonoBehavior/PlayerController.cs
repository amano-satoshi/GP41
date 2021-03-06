﻿///////////////////////////////////////////////////////////////
//
// プレイヤー操作(PlayerController : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/20
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private List<GameObject> cursorList = new List<GameObject>();
    private List<CursorBehavior> cursorBehaviorList = new List<CursorBehavior>();
    private GameObject StageInfo;
    private StageSpawner stageSpawner;
    public GameObject stageState;
    private StageState state;
    private float Vertical = 0f;
    private float Holizon = 0f;
    private bool[] ShootFlg = { false, false };

    // Start is called before the first frame update
    void Start()
    {
        StageInfo = GameObject.Find("StageSpawner");
        stageSpawner = StageInfo.GetComponent<StageSpawner>();
        state = stageState.GetComponent<StageState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state.GetStageState() != StageState.STAGE_STATE.PREPARE)
        {
            return;
        }

        // カーソルリスト取得
        if(cursorBehaviorList.Count == 0)
        {
            cursorList = stageSpawner.GetCursor();
            for (int i = 0; i < cursorList.Count; ++i)
            {
                cursorBehaviorList.Add(cursorList[i].GetComponent<CursorBehavior>());
            }
        }

        //---------- カーソル移動 ----------
        if(!ShootFlg[0])
        {
            if (Input.GetKey(KeyCode.W))
            {
                cursorBehaviorList[0].CursorMove(KeyCode.W);
            }
            if (Input.GetKey(KeyCode.S))
            {
                cursorBehaviorList[0].CursorMove(KeyCode.S);
            }
            if (Input.GetKey(KeyCode.A))
            {
                cursorBehaviorList[0].CursorMove(KeyCode.A);
            }
            if (Input.GetKey(KeyCode.D))
            {
                cursorBehaviorList[0].CursorMove(KeyCode.D);
            }

            Vertical = Input.GetAxis("Pad1_LStick_H");
            Holizon = Input.GetAxis("Pad1_LStick_W");

            if (Mathf.Abs(Vertical) >= 0.7f || Mathf.Abs(Holizon) >= 0.7f)
            {
                cursorBehaviorList[0].CursorMoveStick(Vertical, Holizon);
            }
        }

        if(StageData.GetPlayerNum() == 1 && !ShootFlg[1])
        {
            if (Input.GetKey(KeyCode.I))
            {
                cursorBehaviorList[1].CursorMove(KeyCode.I);
            }
            if (Input.GetKey(KeyCode.K))
            {
                cursorBehaviorList[1].CursorMove(KeyCode.K);
            }
            if (Input.GetKey(KeyCode.J))
            {
                cursorBehaviorList[1].CursorMove(KeyCode.J);
            }
            if (Input.GetKey(KeyCode.L))
            {
                cursorBehaviorList[1].CursorMove(KeyCode.L);
            }
            Vertical = Input.GetAxis("Pad2_LStick_H");
            Holizon = Input.GetAxis("Pad2_LStick_W");

            if (Mathf.Abs(Vertical) >= 0.7f || Mathf.Abs(Holizon) >= 0.7f)
            {
                cursorBehaviorList[1].CursorMoveStick(Vertical, Holizon);
            }
        }

        //---------- ターゲット生成 ----------
        if (StageData.GetPlayerNum() == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && stageSpawner.GetRemainTorpedo()[0] > 0)
            {
                stageSpawner.SpawnTarget(1);
            }

            if (Input.GetButtonDown("Pad1_A") && stageSpawner.GetRemainTorpedo()[0] > 0)
            {
                stageSpawner.SpawnTarget(1);
            }
        }
        else if (StageData.GetPlayerNum() == 1)
        {
            if ((Input.GetButtonDown("Pad1_A") || Input.GetKeyDown(KeyCode.Space)) && stageSpawner.GetRemainTorpedo()[0] > 0 && !ShootFlg[0])
            {
                stageSpawner.SpawnTarget(1);
            }
            if ((Input.GetButtonDown("Pad2_A") || Input.GetKeyDown(KeyCode.N)) && stageSpawner.GetRemainTorpedo()[1] > 0 && !ShootFlg[1])
            {
                stageSpawner.SpawnTarget(2);
            }
        }
        //---------- 魚雷発射 ----------
        if (StageData.GetPlayerNum() == 0)
        {
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Pad1_B")) && stageSpawner.GetRemainTorpedo()[0] < 4)
            {
                stageSpawner.TorpedoShoot();
                state.SetStageState(StageState.STAGE_STATE.SHOOT);
            }
        }
        else if (StageData.GetPlayerNum() == 1)
        {
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Pad1_B")) && stageSpawner.GetRemainTorpedo()[0] < 2 && !ShootFlg[0])
            {
                ShootFlg[0] = true;
            }
            if ((Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Pad2_B")) && stageSpawner.GetRemainTorpedo()[1] < 2 && !ShootFlg[1])
            {
                ShootFlg[1] = true;
            }
            if(ShootFlg[0] && ShootFlg[1])
            {
                stageSpawner.TorpedoShoot();
                state.SetStageState(StageState.STAGE_STATE.SHOOT);
            }
        }
    }

    public bool[] GetShootFlg()
    {
        return ShootFlg;
    }

    public void ResetShootFlg()
    {
        ShootFlg[0] = false;
        ShootFlg[1] = false;
    }
}
