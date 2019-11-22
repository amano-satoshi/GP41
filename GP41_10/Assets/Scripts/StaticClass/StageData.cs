using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageData
{
    static int RescuePersonCnt = 0;

    // ============ ステージ情報リセット ===========
    static public void Reset()
    {
        RescuePersonCnt = 0;
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
}
