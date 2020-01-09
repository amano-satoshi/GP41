///////////////////////////////////////////////////////////////
//
// レーダー移動(RaderBehavior : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/20
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderBehavior : MonoBehaviour
{
    [SerializeField, Header("レーダーの移動速度")]
    private Vector3 RaderSpeed = new Vector3(0f, 0f, 0f);
    [SerializeField, Header("レーダーのスタート地点")]
    private Vector3 RaderStart = new Vector3(0f, 0f, 0f);
    [SerializeField, Header("レーダーのゴール地点")]
    private Vector3 RaderGoal = new Vector3(0f, 0f, 0f);

    public GameObject stagestate;

    public GameObject mapCamera;

    public AudioClip sound;
    AudioSource[] audioSource;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = RaderStart;
        audioSource = mapCamera.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stagestate.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.START ||
            stagestate.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.TIME_UP)
        {
            return;
        }
        transform.position -= RaderSpeed * Time.deltaTime;
        if(transform.position.x < RaderGoal.x)
        {
            transform.position = RaderStart;
            if(stagestate.GetComponent<StageState>().GetStageState() == StageState.STAGE_STATE.PREPARE)
            {
                //audioSource[2].PlayOneShot(sound);
            }
        }
    }
}
