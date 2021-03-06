﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageData
{
    static int RescuePersonCnt = 0;
    static int LearningGauge = 0;
    static float BuffSpeed = 0f;
    static float BuffRange = 0f;
    public static int PlayerNum = 1;

    // ============ ステージ情報リセット ===========
    static public void Reset()
    {
        RescuePersonCnt = 0;
        LearningGauge = 0;
        BuffSpeed = 0f;
        BuffRange = 0f;
    }

    // ============ 救出者数取得 =============
    static public int GetRescuePersonCnt()
    {
        return RescuePersonCnt;
    }

    // ============ 救出者数増加 =============
    static public void IncreaseRescuePersonCnt()
    {
        RescuePersonCnt++;
    }

    // ============ 救出者数任意数増加 =============
    static public void SetIncreaseRescuePersonCnt(int num)
    {
        RescuePersonCnt += num;
    }

    // ============ ポイント数取得 =============
    static public int GetLearningGauge()
    {
        return LearningGauge;
    }

    // ============ ポイント数増減 =============
    static public void SetLearningGauge(int point)
    {
        LearningGauge += point;
    }

    // ============ ポイント数最大 =============
    static public void MaxLearningGauge()
    {
        LearningGauge = 10;
    }

    // ============ 強化スピード数取得 =============
    static public float GetBuffSpeed()
    {
        return BuffSpeed;
    }

    // ============ 強化スピード数増減 =============
    static public void SetBuffSpeed(float speed)
    {
        BuffSpeed += speed;
    }

    // ============ 検知範囲取得 =============
    static public float GetBuffRange()
    {
        return BuffRange;
    }

    // ============ 検知範囲増減 =============
    static public void SetBuffRange(float Range)
    {
        BuffRange += Range;
    }

    // ============ プレイ人数取得 =============
    static public int GetPlayerNum()
    {
        return PlayerNum;
    }

    // ============ プレイ人数セット =============
    static public void SetPlayerNum(int Num)
    {
        PlayerNum = Num;
    }
}
