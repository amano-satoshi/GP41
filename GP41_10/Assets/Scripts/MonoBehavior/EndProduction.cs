///////////////////////////////////////////////////////////////
//
// 終了演出(EndProduction : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/29
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndProduction : MonoBehaviour
{
    private float ProductionTime = 4f;  // 演出時間
    private float Remaintime;   // 演出残り時間
    private bool Startflg;
    private bool Endflg;
    private bool Delflg;
    private int count = 1;
    private Vector3 EndPos;
    private Image CutIn;
    [SerializeField, Header("カットインスピード")]
    private float CutInSpeed = 2500f;
    // Start is called before the first frame update
    void Start()
    {
        Remaintime = ProductionTime;
        CutIn = transform.GetChild(0).GetComponent<Image>();
        EndPos = new Vector3(-66f, 0f, 0f);
        Startflg = false;
        Endflg = false;
        Delflg = false;
    }

    // Update is called once per frame
    void Update()
    {
        Remaintime -= Time.deltaTime;
        if (Remaintime < 0f)
        {
            Remaintime = 0f;
        }
        if (Mathf.FloorToInt(Remaintime) == 3 && count == 1)
        {
            count--;
            Startflg = true;    // 演出開始
        }
        else if (Mathf.FloorToInt(Remaintime) == 0 && count == 0)
        {
            //Destroy(End.gameObject);
            //Destroy(CutIn.gameObject);
            Endflg = true;  // 演出終了
            count--;
        }

        if (Startflg)
        {
            MoveProduction();
        }

        if (Delflg)
        {
            Destroy(this.gameObject);
        }
    }

    public void DelProduction()
    {
        Delflg = true;
    }

    public bool GetProduction()
    {
        return Endflg;
    }

    // 移動演出
    void MoveProduction()
    {
        CutIn.rectTransform.position -= new Vector3(CutInSpeed * Time.deltaTime, 0f, 0f);
        if (CutIn.rectTransform.localPosition.x <= EndPos.x)
        {
            CutIn.rectTransform.localPosition = EndPos;
            Startflg = false;
        }
    }
}
