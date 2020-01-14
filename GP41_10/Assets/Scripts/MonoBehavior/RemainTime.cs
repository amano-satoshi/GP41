///////////////////////////////////////////////////////////////
//
// 制限時間(RemainTime : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/18
//
///////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainTime : MonoBehaviour
{
    [SerializeField, Header("残り時間の桁数分")]
    Image[] images = new Image[3];
    [SerializeField, Header("数字のスプライト（０から９）")]
    Sprite[] numberSprites = new Sprite[10];
    [SerializeField, Header("制限時間")]
    private float gameTime = 0f;        // ゲーム制限時間 [s]
    [SerializeField, Header("カウントダウン開始時間")]
    private float CountDownTime = 0f;        // カウントダウン開始時間 [s]
    private float currentTime;                              // 残り時間タイマー
    private bool ActiveTimer;
    private bool SizeUp;
    private bool CountFlg;
    private bool EndFlg;
    public GameObject mapCamera;

    public AudioClip sound1;
    AudioSource[] audioSource;

    void Start()
    {
        ActiveTimer = false;
        SizeUp = false;
        CountFlg = false;
        EndFlg = false;
        // 残り時間を設定
        currentTime = gameTime;
        audioSource = mapCamera.GetComponents<AudioSource>();
    }

    void Update()
    {
        if(ActiveTimer)
        {
            // 残り時間を計算する
            currentTime -= Time.deltaTime;
        }

        // ゼロ秒以下にならないようにする
        if (currentTime <= 0f)
        {
            currentTime = 0f;
        }

        SetNumbers();
    }

    public int GetRemainTime()
    {
        return Mathf.FloorToInt(currentTime);
    }

    public void StartTimer()
    {
        ActiveTimer = true;
    }

    void SetNumbers()
    {
        if(EndFlg)
        {
            return;
        }
        int times = Mathf.FloorToInt(currentTime);
        string str = string.Format("{000}", times);
        if(times >= 100)
        {
            images[0].sprite = numberSprites[Convert.ToInt32(str.Substring(0, 1))];
            images[1].sprite = numberSprites[Convert.ToInt32(str.Substring(1, 1))];
            images[2].sprite = numberSprites[Convert.ToInt32(str.Substring(2, 1))];
        }
        else if(times >= 10)
        {
            images[0].sprite = numberSprites[0];
            images[1].sprite = numberSprites[Convert.ToInt32(str.Substring(0, 1))];
            images[2].sprite = numberSprites[Convert.ToInt32(str.Substring(1, 1))];
        }
        else
        {
            images[0].sprite = numberSprites[0];
            images[1].sprite = numberSprites[0];
            images[2].sprite = numberSprites[Convert.ToInt32(str.Substring(0, 1))];
        }

        if(CountFlg)
        {
            Countdown();
        }

        if(times == CountDownTime && !CountFlg)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (images[i].color != Color.red)
                {
                    images[i].color = Color.red;
                }
                images[i].rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            } 
            audioSource[2].clip = sound1;
            audioSource[2].loop = true;
            audioSource[2].Play();
            CountFlg = true;
        }

        if (times <= 0 && !EndFlg)
        {
            audioSource[2].Stop();
            EndFlg = true;
        }
    }

    void Countdown()
    {
        for (int i = 0; i < 3; ++i)
        {
            if (SizeUp)
            {
                images[i].rectTransform.localScale += new Vector3(1f * Time.deltaTime, 1f * Time.deltaTime, 1f * Time.deltaTime);
                if (images[2].rectTransform.localScale.x >= 1.5f)
                {
                    SizeUp = false;
                }
            }
            else
            {
                images[i].rectTransform.localScale -= new Vector3(1f * Time.deltaTime, 1f * Time.deltaTime, 1f * Time.deltaTime);
                if (images[2].rectTransform.localScale.x <= 1f)
                {
                    SizeUp = true;
                }
            }
        }
    }
}
