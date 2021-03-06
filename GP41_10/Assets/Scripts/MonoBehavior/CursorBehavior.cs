﻿///////////////////////////////////////////////////////////////
//
// カーソル移動(CursorBehavior : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/01
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{
    [SerializeField, Header("カーソル移動速度")]
    private float CursorSpeed = 0f;
    [SerializeField, Header("カーソル移動制限")]
    private Vector2 LimitCursorPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > LimitCursorPos.x)
        {
            transform.position = new Vector3(LimitCursorPos.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -LimitCursorPos.x)
        {
            transform.position = new Vector3(-LimitCursorPos.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.z > LimitCursorPos.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, LimitCursorPos.y);
        }
        else if (transform.position.z < -LimitCursorPos.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -LimitCursorPos.y);
        }
    }

    // キーボード移動
    public void CursorMove(KeyCode Key)
    {
        if (Key == KeyCode.W || Key == KeyCode.I)
        {
            transform.position += new Vector3(0f, 0f, CursorSpeed * Time.deltaTime);
        }
        if (Key == KeyCode.S || Key == KeyCode.K)
        {
            transform.position -= new Vector3(0f, 0f, CursorSpeed * Time.deltaTime);
        }
        if (Key == KeyCode.A || Key == KeyCode.J)
        {
            transform.position -= new Vector3(CursorSpeed * Time.deltaTime, 0f, 0f);
        }
        if (Key == KeyCode.D || Key == KeyCode.L)
        {
            transform.position += new Vector3(CursorSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    // スティック移動
    public void CursorMoveStick(float vertical, float holizon)
    {
        Vector3 vec = new Vector3(holizon, 0f, -vertical);
        vec = vec.normalized;
        transform.position += vec * CursorSpeed * Time.deltaTime;
    }
}
