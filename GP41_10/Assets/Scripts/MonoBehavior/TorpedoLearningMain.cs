﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorpedoLearningMain : MonoBehaviour
{
    [SerializeField, Header("カウントダウンのImage")]
    Image image;
    [SerializeField, Header("学習項目のImage")]
    Image[] images = new Image[2];
    [SerializeField, Header("数字のスプライト（０から９）")]
    Sprite[] numberSprites = new Sprite[10];
    [SerializeField, Header("制限時間")]
    private float gameTime = 0f;        // 選択制限時間 [s]
    [SerializeField, Header("選択時の拡大率")]
    private float panelScale = 1.5f;
    [SerializeField, Header("選択時の移動場所")]
    private Vector3 panelPos = Vector3.zero;
    [SerializeField, Header("演出時間")]
    private float ProductionTime = 3f;        // 演出時間 [s]
    private float currentTime;
    private bool ProductionFlg;
    private bool EndFlg;
    private bool DelFlg;
    private int Choice;
    private float EraseTime;
    private Image SpeedUp;
    private Image RangeUp;
    private int ProductionNum;
    private Vector3 EndPos;

    // Start is called before the first frame update
    void Start()
    {
        SpeedUp = images[0];
        RangeUp = images[1];

        // 残り時間を設定
        currentTime = gameTime;
        ProductionFlg = false;
        EndFlg = false;
        Choice = 0;
        EraseTime = 4f;
        ProductionNum = 0;
        EndPos = new Vector3(-66f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (ProductionFlg && !EndFlg)
        {
            EraseTime -= Time.deltaTime;
            if (EraseTime < 0f)
            {
                EraseTime = 0f;
            }
            
            if (Mathf.FloorToInt(EraseTime) == 0)
            {
                EndFlg = true;
            }
            MoveProduction();
        }
        else if (!ProductionFlg)
        {
            // パネル選択
            ChoicePanel();

            // パネル反映
            SetPanel();

            // 時間管理
            CountTimer();
        }

        if (DelFlg)
        {
            Destroy(this.gameObject);
        }
    }

    void ChoicePanel()
    {
        // 選択
        if (Input.GetKeyDown(KeyCode.A))         // 左
        {
            Choice--;
            if (Choice < 0)
            {
                Choice += 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))         // 右
        {
            Choice++;
            if (Choice > 1)
            {
                Choice -= 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) || currentTime <= 0f)    // 決定
        {
            if (Choice == 0)
            {
                StageData.SetBuffSpeed(1f);
                ProductionNum = 0;
            }
            else if (Choice == 1)
            {
                StageData.SetBuffRange(1f);
                ProductionNum = 1;
            }
            ProductionFlg = true;
        }
    }

    void SetPanel()
    {
        for (int i = 0; i < 2; ++i)
        {
            if (i == Choice)
            {
                images[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                images[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
            }
            else
            {
                images[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                images[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
            }
        }
    }

    void CountTimer()
    {
        // 残り時間を計算する
        currentTime -= Time.deltaTime;

        // ゼロ秒以下にならないようにする
        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
        }

        SetNumbers();
    }

    void SetNumbers()
    {
        if (EndFlg)
        {
            return;
        }
        int times = Mathf.FloorToInt(currentTime);

        image.sprite = numberSprites[times];
    }

    public void DelLearning()
    {
        DelFlg = true;
    }

    public bool GetEnd()
    {
        return EndFlg;
    }

    void MoveProduction()
    {
        // 選択時の演出
        if (ProductionNum == 0)
        {
            // スピードアップ選択時の演出
            SpeedUp.transform.SetAsLastSibling();
            SpeedUp.rectTransform.localScale = Vector3.Lerp(SpeedUp.rectTransform.localScale, 
                new Vector3(panelScale, panelScale, panelScale), ProductionTime * Time.deltaTime);
            SpeedUp.rectTransform.localPosition = Vector3.Lerp(SpeedUp.rectTransform.localPosition,
                panelPos, ProductionTime * Time.deltaTime);
        }
        else if (ProductionNum == 1)
        {
            // 検知範囲拡大選択時の演出
            RangeUp.rectTransform.localScale = Vector3.Lerp(RangeUp.rectTransform.localScale, 
                new Vector3(panelScale, panelScale, panelScale), ProductionTime * Time.deltaTime);
            RangeUp.rectTransform.localPosition = Vector3.Lerp(RangeUp.rectTransform.localPosition,
                panelPos, ProductionTime * Time.deltaTime);
        }
    }
}
