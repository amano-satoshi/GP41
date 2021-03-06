﻿///////////////////////////////////////////////////////////////
//
// 学習ゲージ管理(LearningGauge : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/12/09
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningGauge : MonoBehaviour
{
    private Vector3[] vertpos = new Vector3[4];
    private int gauge = 0;
    [SerializeField, Header("ゲージ移動速度")]
    private float GaugeSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        vertpos[0] = new Vector3(85f, -10f);
        vertpos[1] = new Vector3(90f, 10f);
        vertpos[2] = new Vector3(90f, 10f);
        vertpos[3] = new Vector3(85f, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        gauge = StageData.GetLearningGauge();
        vertpos[0] = Vector3.Lerp(vertpos[0], new Vector3(85f - gauge * 16.5f, -10f), GaugeSpeed * Time.deltaTime);
        vertpos[1] = Vector3.Lerp(vertpos[1], new Vector3(90f - gauge * 15.5f, 10f + gauge * 1f), GaugeSpeed * Time.deltaTime);
        GetComponent<createImage>().SetVertPos(vertpos);
    }
}
