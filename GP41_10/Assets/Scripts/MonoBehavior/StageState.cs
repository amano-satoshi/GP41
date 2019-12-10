using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageState : MonoBehaviour
{
    public enum STAGE_STATE
    {
        START = 0,          // ステージ開始時
        PREPARE,            // 発射場所選択時
        SHOOT,              // 発射時
        RESCUE,             // 救出者探索時
        TORPEDO_RESULT,     // 救出結果表示時
        LEARNING,           // 学習項目選択時
        TIME_UP,            // 時間切れ時

        MAX_STATE
    }

    STAGE_STATE stageState;
    public GameObject stageTimer;
    public GameObject StageInfo;
    public Canvas Combo;
    private Canvas comboText = null;
    private StageSpawner stageSpawner;
    [SerializeField, Header("演出待機時間")]
    private float waittime = 0f;
    private float worktime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        stageState = STAGE_STATE.START;
        stageSpawner = StageInfo.GetComponent<StageSpawner>();
        worktime = waittime;
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
            stageState != STAGE_STATE.TORPEDO_RESULT && stageState != STAGE_STATE.SHOOT)
        {
            stageState = STAGE_STATE.TIME_UP;
            Debug.Log(StageData.GetRescuePersonCnt());
        }
        if(stageState == STAGE_STATE.SHOOT)
        {
            if (stageSpawner.GetShootProduction() != null && stageSpawner.GetShootProduction().GetComponent<ShootProduction>().GetProduction())
            {
                stageSpawner.GetShootProduction().GetComponent<ShootProduction>().DelProduction();
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
                    comboText.gameObject.GetComponentInChildren<ComboText>().TwoCombo();
                    StageData.SetLearningGauge(n + (n - 1));
                }
                else if (n == 3)
                {
                    comboText.gameObject.GetComponentInChildren<ComboText>().ThreeCombo();
                    StageData.SetLearningGauge(n + (n - 1) + (n - 2));
                }
                else if (n == 4)
                {
                    comboText.gameObject.GetComponentInChildren<ComboText>().FourCombo();
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

                if (StageData.GetLearningGauge() >= 10)
                {
                    StageData.SetLearningGauge(-10);
                    if (comboText != null)
                    {
                        Destroy(comboText.gameObject);
                        comboText = null;
                    }
                    stageState = STAGE_STATE.LEARNING;
                }
                else
                {
                    stageSpawner.ResetTorpedoCamera();
                    stageSpawner.ResetTorpedo();
                    stageSpawner.RescueDrowningPerson();
                    stageSpawner.ResetTarget();
                    if (comboText != null)
                    {
                        Destroy(comboText.gameObject);
                        comboText = null;
                    }
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
                stageState = STAGE_STATE.PREPARE;
            }
        }
        if(stageState == STAGE_STATE.TIME_UP)
        {
            if (stageSpawner.GetEndProduction() != null && stageSpawner.GetEndProduction().GetComponent<EndProduction>().GetProduction())
            {
                //stageSpawner.GetEndProduction().GetComponent<EndProduction>().DelProduction();
                // リザルトへ

            }
        }
    }
    // ============ ステージ状態リセット ===========
    public void Reset()
    {
        stageState = STAGE_STATE.PREPARE;
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
