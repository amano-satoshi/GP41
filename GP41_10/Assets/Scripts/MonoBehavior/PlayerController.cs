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
    private float Vertical = 0f;
    private float Holizon = 0f;

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

        Vertical = Input.GetAxis("Pad1_LStick_H");
        Holizon = Input.GetAxis("Pad1_LStick_W");
        
        if (Mathf.Abs(Vertical) >= 0.7f || Mathf.Abs(Holizon) >= 0.7f)
        {
            cursorBehavior.CursorMoveStick(Vertical, Holizon);
        }

        //---------- ターゲット生成 ----------
        if(Input.GetKeyDown(KeyCode.Space) && stageSpawner.GetTargets().Count < 4)
        {
            stageSpawner.SpawnTarget();
        }

        if(Input.GetButtonDown("Pad1_A") && stageSpawner.GetTargets().Count < 4)
        {
            stageSpawner.SpawnTarget();
        }
        //---------- 魚雷発射 ----------
        if(Input.GetKeyDown(KeyCode.Return) && stageSpawner.GetTargets().Count != 0)
        {
            stageSpawner.TorpedoShoot();
            state.SetStageState(StageState.STAGE_STATE.SHOOT);
        }
        if (Input.GetButtonDown("Pad1_B") && stageSpawner.GetTargets().Count != 0)
        {
            stageSpawner.TorpedoShoot();
            state.SetStageState(StageState.STAGE_STATE.SHOOT);
        }
    }
}
