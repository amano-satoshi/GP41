using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject cursor;
    private CursorBehavior cursorBehavior;
    private GameObject StageInfo;
    private StageSpawner stageSpawner;
    public GameObject stageState;
    private StageState state;

    // Start is called before the first frame update
    void Start()
    {
        cursorBehavior = cursor.GetComponent<CursorBehavior>();
        StageInfo = GameObject.Find("StageSpawner");
        stageSpawner = StageInfo.GetComponent<StageSpawner>();
        state = stageState.GetComponent<StageState>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("ResultScene");
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log(StageData.GetRescuePersonCnt());
        }
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
            Debug.Log(StageData.GetLearningGauge());
        }
        if (state.GetStageState() != StageState.STAGE_STATE.PREPARE)
        {
            return;
        }
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
        if(Input.GetKeyDown(KeyCode.Space) && stageSpawner.GetTargets().Count < 4)
        {
            stageSpawner.SpawnTarget();
        }

        //---------- 魚雷発射 ----------
        if(Input.GetKeyDown(KeyCode.Return) && stageSpawner.GetTargets().Count != 0)
        {
            stageSpawner.TorpedoShoot();
            state.SetStageState(StageState.STAGE_STATE.SHOOT);
        }
    }
}
