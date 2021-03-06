﻿///////////////////////////////////////////////////////////////
//
// ステージの状態管理(StageState : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/29
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageState : MonoBehaviour
{
    public enum STAGE_STATE
    {
        START = 0,          // ステージ開始時
        PREPARE,            // 発射場所選択時
        SHOOT,              // 発射時
        PRODUCTION,         // 発射演出時
        RESCUE,             // 救出者探索時
        TORPEDO_RESULT,     // 救出結果表示時
        LEARNING,           // 学習項目選択時
        TIME_UP,            // 時間切れ時

        MAX_STATE
    }

    STAGE_STATE stageState;
    public GameObject stageTimer;
    public GameObject StageInfo;
    public GameObject playerController;
    public Canvas Combo;
    public GameObject Fade;
    private Canvas comboText = null;
    private StageSpawner stageSpawner;
    private PlayerController playerControllerObj;
    [SerializeField, Header("発射待機時間")]
    private float shoottime = 0f;
    [SerializeField, Header("演出待機時間")]
    private float waittime = 0f;
    private float worktime = 0f;
    private float shootworktime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        stageState = STAGE_STATE.START;
        stageSpawner = StageInfo.GetComponent<StageSpawner>();
        playerControllerObj = playerController.GetComponent<PlayerController>();
        worktime = waittime;
        shootworktime = shoottime;
    }

    // Update is called once per frame
    void Update()
    {
        if(stageState == STAGE_STATE.START)
        {
            if(stageSpawner.GetStartProduction() != null && stageSpawner.GetStartProduction().GetComponent<StartProduction>().GetProduction())
            {
                stageSpawner.GetStartProduction().GetComponent<StartProduction>().DelProduction();
                stageTimer.GetComponent<RemainTime>().StartTimer();
                stageState = STAGE_STATE.PREPARE;
            }
        }
        int n = 0;
        if(stageTimer.GetComponent<RemainTime>().GetRemainTime() <= 0f && stageState != STAGE_STATE.TIME_UP && stageState != STAGE_STATE.RESCUE &&
            stageState != STAGE_STATE.TORPEDO_RESULT && stageState != STAGE_STATE.SHOOT && stageState != STAGE_STATE.PRODUCTION)
        {
            stageState = STAGE_STATE.TIME_UP;
        }
        if(stageState == STAGE_STATE.SHOOT)
        {
            if (stageSpawner.GetShootProduction() != null && stageSpawner.GetShootProduction().GetComponent<ShootProduction>().GetProduction())
            {
                stageSpawner.GetShootProduction().GetComponent<ShootProduction>().DelProduction();
                stageState = STAGE_STATE.PRODUCTION;
            }
        }
        if (stageState == STAGE_STATE.PRODUCTION)
        {
            shootworktime -= Time.deltaTime;
            if (shootworktime <= 0f)
            {
                shootworktime = shoottime;
                stageState = STAGE_STATE.RESCUE;
            }
        }
        if (stageState == STAGE_STATE.RESCUE)
        {
            for (int i = 0; i < stageSpawner.GetTorpedo().Count; ++i)
            {
                if (stageSpawner.GetTorpedo()[i].GetComponent<TorpedoBehavior>().torpedostate == TorpedoBehavior.TorpedoState.RESCUE)
                {
                    n++;
                    continue;
                }
                else if(stageSpawner.GetTorpedo()[i].GetComponent<TorpedoBehavior>().torpedostate == TorpedoBehavior.TorpedoState.FAILED)
                {
                    continue;
                }
                else
                {
                    return;
                }
            }
            if(n >= 2)
            {
                comboText = Instantiate(Combo);
                if(n == 2)
                {
                    comboText.GetComponent<ComboText>().TwoCombo();
                    StageData.SetLearningGauge(n + (n - 1));
                }
                else if (n == 3)
                {
                    comboText.GetComponent<ComboText>().ThreeCombo();
                    StageData.SetLearningGauge(n + (n - 1) + (n - 2));
                }
                else if (n == 4)
                {
                    comboText.GetComponent<ComboText>().FourCombo();
                    StageData.SetLearningGauge(n + (n - 1) + (n - 2) + (n - 3));
                }
            }
            else
            {
                StageData.SetLearningGauge(n);
            }
            stageState = STAGE_STATE.TORPEDO_RESULT;
        }
        if(stageState == STAGE_STATE.TORPEDO_RESULT)
        {
            worktime -= 1f * Time.deltaTime;
            if(worktime <= 0f)
            {
                worktime = waittime;

                if (stageTimer.GetComponent<RemainTime>().GetRemainTime() <= 0f)
                {
                    stageState = STAGE_STATE.TIME_UP;
                    Debug.Log(StageData.GetRescuePersonCnt());
                    return;
                }

                if (comboText != null)
                {
                    Destroy(comboText.gameObject);
                    comboText = null;
                }

                if (StageData.GetLearningGauge() >= 10)
                {
                    StageData.SetLearningGauge(-10);
                    stageState = STAGE_STATE.LEARNING;
                }
                else
                {
                    stageSpawner.ResetTorpedoCamera();
                    stageSpawner.ResetTorpedo();
                    stageSpawner.RescueDrowningPerson();
                    stageSpawner.ResetTarget();
                    stageSpawner.ResetRemainTorpedo();
                    playerControllerObj.ResetShootFlg();
                    stageState = STAGE_STATE.PREPARE;
                }
            }
        }
        if(stageState == STAGE_STATE.LEARNING)
        {
            if (stageSpawner.GetTorpedoLearning() != null && stageSpawner.GetTorpedoLearning().GetComponent<TorpedoLearningMain>().GetEnd())
            {
                stageSpawner.GetTorpedoLearning().GetComponent<TorpedoLearningMain>().DelLearning();
                stageSpawner.ResetTorpedoCamera();
                stageSpawner.ResetTorpedo();
                stageSpawner.RescueDrowningPerson();
                stageSpawner.ResetTarget();
                stageSpawner.ResetRemainTorpedo();
                playerControllerObj.ResetShootFlg();
                stageState = STAGE_STATE.PREPARE;
            }
        }
        if(stageState == STAGE_STATE.TIME_UP)
        {
            if (stageSpawner.GetEndProduction() != null && stageSpawner.GetEndProduction().GetComponent<EndProduction>().GetProduction())
            {
                //stageSpawner.GetEndProduction().GetComponent<EndProduction>().DelProduction();
                // リザルトへ
                Fade.GetComponent<FadeController>().SetFadeOut(true);
                Fade.GetComponent<FadeController>().SetFadeIn(false);
                if(Fade.GetComponent<FadeController>().GetAlpha() >= 1.0f)
                {
                    SceneManager.LoadScene("ResultScene");
                }
            }
        }
    }
    // ============ ステージ状態リセット ===========
    public void Reset()
    {
        stageState = STAGE_STATE.START;
    }

    // ============ ステージ状態取得 =============
    public STAGE_STATE GetStageState()
    {
        return stageState;
    }

    // ============ ステージ状態変更 =============
    public void SetStageState(STAGE_STATE state)
    {
        stageState = state;
    }
}
