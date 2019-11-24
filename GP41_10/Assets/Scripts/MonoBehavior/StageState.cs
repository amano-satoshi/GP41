using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageState : MonoBehaviour
{
    public enum STAGE_STATE
    {
        PREPARE = 0,
        RESCUE,
        TIME_UP,

        MAX_STATE
    }

    STAGE_STATE stageState;
    public GameObject stageTimer;
    public GameObject StageInfo;
    private StageSpawner stageSpawner;

    // Start is called before the first frame update
    void Start()
    {
        stageState = STAGE_STATE.PREPARE;
        stageSpawner = StageInfo.GetComponent<StageSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stageTimer.GetComponent<RemainTime>().GetRemainTime() <= 0f && stageState != STAGE_STATE.TIME_UP)
        {
            stageState = STAGE_STATE.TIME_UP;
            Debug.Log(StageData.GetRescuePersonCnt());
        }
        if (stageState == STAGE_STATE.RESCUE)
        {
            for (int i = 0; i < stageSpawner.GetTorpedo().Count; ++i)
            {
                if (stageSpawner.GetTorpedo()[i].GetComponent<TorpedoBehavior>().torpedostate != TorpedoBehavior.TorpedoState.RESCUE)
                    return;
            }
            stageSpawner.ResetTorpedoCamera();
            stageSpawner.ResetTorpedo();
            stageSpawner.RescueDrowningPerson();
            stageSpawner.ResetTarget();
            stageState = STAGE_STATE.PREPARE;
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
