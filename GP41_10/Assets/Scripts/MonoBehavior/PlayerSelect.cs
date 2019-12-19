using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject FadeObj;

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

        float pd1h = Input.GetAxis("Pad1_H");
        float pd1lsh = Input.GetAxis("Pad1_LStick_H");
       
        if ((pd1lsh < -0.5f ||
            pd1h > 0.3f ||
            Input.GetKeyDown(KeyCode.W)) && nStatePlayer != 0)
        {
            nBigTime = 0;
            nStatePlayer = 0;
            bOne = false;
            fScaleSpeed = Speed;
            nMoveCurrentTime = 0.0f;
        }

        else if ((pd1lsh > 0.5f ||
            pd1h < -0.3f ||
            Input.GetKeyDown(KeyCode.S)) && nStatePlayer != 1)
        {
            nBigTime = 0;
            nStatePlayer = 1;
            bOne = false;
            fScaleSpeed = Speed;
            nMoveCurrentTime = 0.0f;

        }
        Debug.Log(FadeObj.GetComponent<FadeController>().GetAlpha());

        if (Input.GetButtonDown("Pad1_A") ||
            Input.GetKeyDown(KeyCode.Return))
        {

            bSelectedPlayer = true;
            StageData.SetPlayerNum(nStatePlayer);
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
  //                      TwoModel.gameObject.transform.position += MoveSpeed * Time.deltaTime;
                    }

                    if (nMoveCurrentTime < nMoveTime * 2)
                    {// イルカが進む
                        TwoModel.gameObject.transform.position += MoveSpeed * Time.deltaTime;
                        nMoveCurrentTime += Time.deltaTime;
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

                    if (nMoveCurrentTime < nMoveTime)
                    {// イルカが進む
                        TwoModel.gameObject.transform.position += MoveSpeed * Time.deltaTime;
                        nMoveCurrentTime += Time.deltaTime;
                    }

                    break;
                default:
                    // 例外処理
                    break;
            }
        }
        else
        {
            FadeObj.GetComponent<FadeController>().SetFadeOut(true);
            FadeObj.GetComponent<FadeController>().SetFadeIn(false);

            if (FadeObj.GetComponent<FadeController>().GetAlpha() >= 1.0f)
            {
                Debug.Log("入った");
                SceneManager.LoadScene("GameMain");
            }
        }
    }
}
