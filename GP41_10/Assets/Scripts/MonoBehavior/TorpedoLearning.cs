using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorpedoLearning : MonoBehaviour
{
    [SerializeField, Header("カウントダウンのImage")]
    Image image;
    [SerializeField, Header("学習項目のImage")]
    Image[] images = new Image[3];
    [SerializeField, Header("数字のスプライト（０から９）")]
    Sprite[] numberSprites = new Sprite[10];
    [SerializeField, Header("制限時間")]
    private float gameTime = 0f;        // 選択制限時間 [s]
    private float currentTime;
    private bool ProductionFlg;
    private bool EndFlg;
    private bool DelFlg;
    private int Choice;
    private float EraseTime;
    private Image CutIn;
    private Text StatusUp;
    private int count;
    private int ProductionNum;
    private Vector3 EndPos;

    // Start is called before the first frame update
    void Start()
    {
        CutIn = transform.GetChild(6).GetComponent<Image>();
        StatusUp = transform.GetChild(7).GetComponent<Text>();

        // 残り時間を設定
        currentTime = gameTime;
        ProductionFlg = false;
        EndFlg = false;
        Choice = 0;
        EraseTime = 4f;
        count = 1;
        ProductionNum = 0;
        EndPos = new Vector3(-21f, 0f, 0f);
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
            if (Mathf.FloorToInt(EraseTime) == 3 && count == 1)
            {
                if(ProductionNum == 0)
                {
                    StatusUp.text = "スピードアップ!";
                }
                else if(ProductionNum == 1)
                {
                    StatusUp.text = "検知範囲拡大!";
                }
                count--;
            }
            else if (Mathf.FloorToInt(EraseTime) == 0 && count == 0)
            {
                Destroy(StatusUp.gameObject);
                Destroy(CutIn.gameObject);
                EndFlg = true;
                count--;
            }
            MoveProduction();
        }
        else if(!ProductionFlg)
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
                Choice += 3;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))         // 右
        {
            Choice++;
            if (Choice > 2)
            {
                Choice -= 3;
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
            else if (Choice == 2)
            {
                int choice = Random.Range(0, 2);
                if (choice == 0)
                {
                    StageData.SetBuffSpeed(1f);
                    ProductionNum = 0;
                }
                else if (choice == 1)
                {
                    StageData.SetBuffRange(1f);
                    ProductionNum = 1;
                }
            }
            ProductionFlg = true;
        }
    }

    void SetPanel()
    {
        for (int i = 0; i < 3; ++i)
        {
            if (i == Choice)
            {
                images[i].rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                images[i].rectTransform.localScale = new Vector3(1f, 1f, 1f);
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
        CutIn.rectTransform.position -= new Vector3(1242f * Time.deltaTime, 0f, 0f);
        StatusUp.rectTransform.position -= new Vector3(1242f * Time.deltaTime, 0f, 0f);
        if (CutIn.rectTransform.localPosition.x <= EndPos.x)
        {
            CutIn.rectTransform.localPosition = EndPos;
            StatusUp.rectTransform.localPosition = EndPos;
        }
    }
}
