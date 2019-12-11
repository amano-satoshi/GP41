using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resultBoat : MonoBehaviour
{
    [SerializeField]
    public GameObject boat0, boat1, boat2;  // ボート

    [SerializeField]
    public GameObject text0, text1;

    [SerializeField]
    public GameObject numSprite;
    public GameObject numText;
    private Text textnumText;

    [SerializeField]
    private int rescueNum;// 助けた人数。メインからもらってくる

    [SerializeField]
    public int rescueBorder1, rescueBorder2; // ボートの数が変わる人数

    [SerializeField]
    public float boatStartTime, boatStopTime;

    [SerializeField]
    public float textStartTime, textAlphaSpeed;


    private BoatAlignNormal[] boatAlignNormal = new BoatAlignNormal[3]; // ボート格納用
    private Graphic graphic0, graphic1, graphic2; // 格納用
    private float elapsedTime;
    private float textAlpha;
    private int boatNum;
    private bool isEnd;
    string rescueNumText;

    // Start is called before the first frame update
    void Start()
    {
        boatAlignNormal[0] = boat0.GetComponent<BoatAlignNormal>();
        boatAlignNormal[1] = boat1.GetComponent<BoatAlignNormal>();
        boatAlignNormal[2] = boat2.GetComponent<BoatAlignNormal>();
        graphic0 = text0.GetComponent<Text>();
        graphic1 = text1.GetComponent<Text>();

        elapsedTime = 0.0f;
        //rescueNum = 0;

        // ここを変更
        CreateNum(rescueNum);

        rescueNumText = (rescueNum).ToString();
        textnumText = numText.GetComponent<Text>();
        textnumText.text = rescueNumText;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd)
            return;

        elapsedTime += Time.deltaTime;

        if (rescueNum < rescueBorder1)
        {
            boatNum = 1;
        }
        else if (rescueNum >= rescueBorder1 && rescueNum <= rescueBorder2)
        {
            boatNum = 2;
        }
        else if (rescueNum > rescueBorder2)
        {
            boatNum = 3;
        }

        if (elapsedTime > boatStopTime)
        {
            if (boatAlignNormal[0].rawForward != 0.0f)
                for (int i = 0; i < boatNum; i++)
                    boatAlignNormal[i].rawForward = 0.0f;
        }
        else if (elapsedTime > boatStartTime)
        {
            if (boatAlignNormal[0].rawForward != 1.0f)
                for (int i = 0; i < boatNum; i++)
                    boatAlignNormal[i].rawForward = 1.0f;
        }

        if (elapsedTime > textStartTime)
        {
            if (textAlpha < 1.0f)
            {
                textAlpha += textAlphaSpeed;
            }
            else
            {
                textAlpha = 1.0f;
                isEnd = true;
            }
            SetAlpha(graphic0, textAlpha);
            SetAlpha(graphic1, textAlpha);
            SetAlpha(graphic2, textAlpha);
        }
    }

    public void SetAlpha(Graphic self, float alpha)
    {
        var color = self.color;
        color.a = alpha;
        self.color = color;
    }

    //描画用の数字を作る
    private void CreateNum(int point)
    {

        //桁を割り出す
        int digit = ChkDigit.CheckDigit(point);

        //桁の分だけオブジェクトを作り登録していく
        for (int i = 0; i < digit; i++)
        {

            GameObject numObj = Instantiate(numSprite) as GameObject;

            //numObj = GetComponent<SpriteRenderer>();

            //graphic2 = numObj.GetComponent<Image>();


            //子供として登録
            numObj.transform.parent = transform;


            //現在チェックしている桁の数字を割り出す
            int digNum = ChkDigit.GetPointDigit(point, i + 1);

            //ポイントから数字を切り替える
            numObj.GetComponent<NumCtrl>().ChangeSprite(digNum);

            //サイズをゲットする
            float size_w = numObj.GetComponent<SpriteRenderer>().bounds.size.x;

            //位置をずらす
            float ajs_x = size_w * i - (size_w * digit) / 10;
            Vector3 pos = new Vector3(-30f - ajs_x, 15f, 50f);
            numObj.transform.position = pos;

            numObj = null;
        }
    }
}
