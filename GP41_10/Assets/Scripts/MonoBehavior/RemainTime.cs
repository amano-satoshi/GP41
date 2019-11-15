///////////////////////////////////////////////////////////////
//
// 制限時間(RemainTime : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/18
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainTime : MonoBehaviour
{
    [SerializeField, Header("制限時間")]
    private float gameTime = 0f;        // ゲーム制限時間 [s]
    private Text uiText;                                    // UIText コンポーネント
    private float currentTime;                              // 残り時間タイマー

    void Start()
    {
        // Textコンポーネント取得
        uiText = GetComponent<Text>();
        // 残り時間を設定
        currentTime = gameTime;
    }

    void Update()
    {
        // 残り時間を計算する
        currentTime -= Time.deltaTime;

        // ゼロ秒以下にならないようにする
        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
        }
        int minutes = Mathf.FloorToInt(currentTime / 60F);
        int seconds = Mathf.FloorToInt(currentTime - minutes * 60);
        uiText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
