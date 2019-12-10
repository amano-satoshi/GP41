using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    struct BUBBLE               // バブル構造体
    {
        public int nNum;                // 識別用ナンバー
        public GameObject gameobject;   // ゲームオブジェクト
        public bool bMoveEnd;           // 個々の移動終了フラグ
        public float fMoveTime;          // 移動する時間
    };

    public GameObject bubble;
    public GameObject BubblePrehab;

    public const int MAX_BUBBLE = 120;

    public float goaltime;

    BUBBLE[] bubbles = new BUBBLE[MAX_BUBBLE];

    bool bNext;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < MAX_BUBBLE; i++)
        {
            bubbles[i].gameobject = GameObject.Instantiate(
                bubble,
                bubble.transform.position,
                bubble.transform.rotation);

            // 親子付け
            bubbles[i].gameobject.transform.parent = BubblePrehab.transform;

            bubbles[i].nNum = i;
            bubbles[i].bMoveEnd = false;
            bubbles[i].gameobject.transform.localPosition = new Vector3(Random.Range(-900, 900), Random.Range(-1700, -800), 0.0f);
            float RandScl = Random.Range(10, 65);
            bubbles[i].gameobject.transform.localScale = new Vector3(RandScl / 10, RandScl / 10, 1.0f);
            bubbles[i].fMoveTime = 0.0f;
        }

        bNext = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pad1_A") || 
            Input.GetKeyDown(KeyCode.A))
        {
            bNext = true;
        }

        if (bNext)
        {
            for (int i = 0; i < MAX_BUBBLE; i++)
            {
                if (bubbles[i].bMoveEnd)
                    continue;

                float Speed = Random.Range(10f, 100f);
                bubbles[i].gameobject.transform.position += new Vector3(0.0f, Speed / 10.0f, 0.0f);
                bubbles[i].fMoveTime += 0.1f;

                if (bubbles[i].fMoveTime >= goaltime)
                {
                    bubbles[i].bMoveEnd = true;
                }
            }
        }

        // テスト用
        if (Input.GetButtonDown("Pad1_A"))
        {
            Debug.Log("Pad1_A");
        }

        if (Input.GetButtonDown("Pad1_B"))
        {
            Debug.Log("Pad1_B");
        }

        if (Input.GetButtonDown("Pad1_X"))
        {
            Debug.Log("Pad1_X");
        }

        if (Input.GetButtonDown("Pad1_Y"))
        {
            Debug.Log("Pad1_Y");
        }

        float pd1w = Input.GetAxis("Pad1_W");
        float pd1h = Input.GetAxis("Pad1_H");
        if ((pd1w != 0) || (pd1h != 0))
        {
            Debug.Log("D Pad:" + pd1w + "," + pd1h);
        }

        //R Stick
        float pd1rsh = Input.GetAxis("Pad1_RStick_H");
        if ((pd1rsh != 0))
        {
            Debug.Log("R stickH:" + pd1rsh);
        }
        float pd1rsw = Input.GetAxis("Pad1_RStick_W");

        if (pd1rsw != 0)
        {
            //Debug.Log("R stickW:" + pd1rsw);
        }

        //L Stick
        float pd1lsh = Input.GetAxis("Pad1_LStick_H");
        if ((pd1lsh != 0))
        {
            Debug.Log("L stickH:" +pd1lsh);
        }

        float pd1lsw = Input.GetAxis("Pad1_LStick_W");

        if ((pd1lsw != 0))
        {
            //Debug.Log("L stickW:" + pd1lsw);
        }
    }
}
