using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    public GameObject OneSprite;
    public GameObject TwoSprite;

    public GameObject Alpha;

    public GameObject OneModel;
    public GameObject TwoModel;

    public Vector3 OneDefaultScale;
    public Vector3 TwoDefaultScale;

    public float BigScale;

    public int nStatePlayer;        // 0: 1人 1: 2人

    float fScaleSpeed;
    const float Speed = 0.003f;

    public int nBigMaxTime;
    private int nBigTime;

    public Vector3 MoveSpeed;
    public Vector3 TowModelDefaultPos;
    public float nMoveTime;
    private float nMoveCurrentTime = 0.0f;

    bool bSelectedPlayer;
    bool bOne;
    bool bOneOne;

    // Start is called before the first frame update
    void Start()
    {
        fScaleSpeed = Speed;
        bOne = false;
        bOneOne = false;
        nBigTime = 0;
        nStatePlayer = 0;
        bSelectedPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        nBigTime++;

        if(nBigTime >= nBigMaxTime)
        {
            nBigTime = 0;
            fScaleSpeed *= -1;
        }

        if(Input.GetKeyDown(KeyCode.W) && nStatePlayer != 0)
        {
            nBigTime = 0;
            nStatePlayer = 0;
            bOne = false;
            fScaleSpeed = Speed;
        }

        if (Input.GetKeyDown(KeyCode.S) && nStatePlayer != 1)
        {
            nBigTime = 0;
            nStatePlayer = 1;
            bOne = false;
            fScaleSpeed = Speed;

        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            bSelectedPlayer = true;
            //StageData
        }

        if (!bSelectedPlayer)
        {
            switch (nStatePlayer)
            {
                case 0:
                    // 1人が選ばれている
                    if(!bOne)
                    {
                        // 描画順を前に
                        Alpha.gameObject.transform.SetSiblingIndex(1);
                        OneSprite.gameObject.transform.SetSiblingIndex(2);
                        TwoSprite.gameObject.transform.SetSiblingIndex(0);

                        OneSprite.gameObject.transform.localScale += new Vector3(
                        BigScale,
                        BigScale,
                        BigScale);
                        bOne = true;

                        if (!bOneOne)
                        {
                        }
                        else
                        {
                            TwoModel.gameObject.transform.position = new Vector3(-2.156564f, -1.096563f, 0f);
                        }

                    }
                    nMoveCurrentTime = 0.0f;


                    OneSprite.gameObject.transform.localScale += new Vector3(
                        fScaleSpeed,
                        fScaleSpeed,
                        fScaleSpeed);

                    TwoSprite.gameObject.transform.localScale = new Vector3(
                       TwoDefaultScale.x,
                       TwoDefaultScale.y,
                       TwoDefaultScale.z);

                    if (bOneOne)
                    {
                        TwoModel.gameObject.transform.position += MoveSpeed * Time.deltaTime;
                    }

                    break;

                case 1:
                    // 2人が選ばれている

                    bOneOne = true;

                    // 描画順を前に
                    Alpha.gameObject.transform.SetSiblingIndex(1);
                    OneSprite.gameObject.transform.SetSiblingIndex(0);
                    TwoSprite.gameObject.transform.SetSiblingIndex(2);

                    OneSprite.gameObject.transform.localScale = new Vector3(
                       OneDefaultScale.x,
                       OneDefaultScale.y,
                       OneDefaultScale.z);

                    if(!bOne)
                    {
                        TwoSprite.gameObject.transform.localScale += new Vector3(
                          BigScale,
                          BigScale,
                          BigScale);
                        bOne = true;

                        TwoModel.gameObject.transform.position = TowModelDefaultPos;
                    }
                    TwoSprite.gameObject.transform.localScale += new Vector3(
                       fScaleSpeed,
                       fScaleSpeed,
                       fScaleSpeed);

                    if (nMoveCurrentTime <= nMoveTime)
                    {// イルカが進む
                        TwoModel.gameObject.transform.position += MoveSpeed * Time.deltaTime;
                        nMoveCurrentTime += 0.1f;
                    }

                    break;
                default:
                    // 例外処理
                    break;
            }
        }
    }
}
