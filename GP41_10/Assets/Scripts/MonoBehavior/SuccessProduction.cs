///////////////////////////////////////////////////////////////
//
// 成功演出(SuccessProduction : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/25
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessProduction : MonoBehaviour
{
    [SerializeField, Header("成功表示最大")]
    private Vector3 MaxSize = Vector3.zero;
    [SerializeField, Header("拡大スピード")]
    private float Speed = 0f;
    private bool Up = true;
    private bool End = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().transform.localScale = new Vector3(0.6f, 0.3f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(End)
        {
            return;
        }
        if(Up)
        {
            GetComponent<Image>().transform.localScale += new Vector3(Speed * 6f * Time.deltaTime, Speed * 3f * Time.deltaTime, Speed * Time.deltaTime);
            if (GetComponent<Image>().transform.localScale.z > MaxSize.z)
            {
                Up = false;
            }
        }
        else
        {
            GetComponent<Image>().transform.localScale -= new Vector3(Speed * 6f * Time.deltaTime, Speed * 3f * Time.deltaTime, Speed * Time.deltaTime);
            if (GetComponent<Image>().transform.localScale.z <= 1f)
            {
                GetComponent<Image>().transform.localScale = new Vector3(6f, 3f, 1f);
                End = true;
            }
        }
    }
}
