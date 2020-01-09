///////////////////////////////////////////////////////////////
//
// 失敗演出(FailedProduction : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/25
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailedProduction : MonoBehaviour
{
    [SerializeField, Header("演出開始位置")]
    private float StartPos = 0f;
    [SerializeField, Header("落下スピード")]
    private float FallSpeed = 0f;
    [SerializeField, Header("傾く速さ")]
    private float RotSpeed = 0f;
    [SerializeField, Header("傾く最大角度")]
    private float MaxRot = 0f;
    [SerializeField, Header("傾く準備時間")]
    private float PreTime = 0f;
    private bool Fall = false;
    private bool Rotation = false;
    private Vector3 InitPos;
    private Quaternion InitRot;
    private float CurrentRot;
    private float CurrentTime;
    // Start is called before the first frame update
    void Start()
    {
        InitPos = GetComponent<Image>().rectTransform.position;
        InitRot = GetComponent<Image>().rectTransform.localRotation;
        CurrentRot = 0f;
        CurrentTime = 0f;
        GetComponent<Image>().rectTransform.position += new Vector3(0f, StartPos, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Rotation)
        {
            return;
        }
        // 文字落下
        if(!Fall)
        {
            GetComponent<Image>().rectTransform.position -= new Vector3(0f, FallSpeed * Time.deltaTime, 0f);
            if(InitPos.y > GetComponent<Image>().rectTransform.position.y)
            {
                GetComponent<Image>().rectTransform.position = InitPos;
                Fall = true;
            }
        }
        // 文字傾き
        else
        {
            CurrentTime += Time.deltaTime;
            if(CurrentTime >= PreTime)
            {
                CurrentRot += RotSpeed * Time.deltaTime;
                GetComponent<Image>().rectTransform.localRotation *= Quaternion.AngleAxis(RotSpeed * Time.deltaTime, Vector3.forward);
                if (CurrentRot >= MaxRot)
                {
                    GetComponent<Image>().rectTransform.localRotation = InitRot;
                    GetComponent<Image>().rectTransform.localRotation *= Quaternion.AngleAxis(MaxRot, Vector3.forward);
                    Rotation = true;
                }
            }
        }
    }
}
