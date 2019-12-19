using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class resultBoat : MonoBehaviour
{
    [SerializeField]
    public GameObject boat0, boat1, boat2;  // ボート

    [SerializeField]
    public GameObject numSprite;
    private Text textnumText;
    public GameObject ninkyujo;
    public Vector3 ninkyujoSpeed;
    public GameObject Tairyo;
    public GameObject Iikanji;
    public float StampSpeed;

    [SerializeField]
    private int rescueNum;// 助けた人数。メインからもらってくる

    [SerializeField]
    public int rescueBorder1, rescueBorder2; // ボートの数が変わる人数

    [SerializeField]
    public float boatStartTime, boatStopTime;

    [SerializeField]
    public float textStartTime, textAlphaSpeed;

    [SerializeField]
    public Canvas canvas;

    [SerializeField]
    public GameObject player;


    private BoatAlignNormal[] boatAlignNormal = new BoatAlignNormal[3]; // ボート格納用
    private float elapsedTime;
    private float textAlpha;
    private int boatNum;
    private bool isEnd;
    private bool bOne;

    private bool bninkyu;
    private bool bninzuu;
    private bool bhyoka;
    private bool bBoatStop;

    private List<GameObject> WorkList = new List<GameObject>();

    private List<GameObject> playerList = new List<GameObject>();
    string rescueNumText;

    public GameObject FadeObj;

    // Start is called before the first frame update
    void Start()
    {
        boatAlignNormal[0] = boat0.GetComponent<BoatAlignNormal>();
        boatAlignNormal[1] = boat1.GetComponent<BoatAlignNormal>();
        boatAlignNormal[2] = boat2.GetComponent<BoatAlignNormal>();

        elapsedTime = 0.0f;
        rescueNum = StageData.GetRescuePersonCnt();
        rescueNum = 15;
        CreateNum(rescueNum);
        

        // 人間の生成
        for(int i = 0, j = 0; i < rescueNum; i++, j++)
        {
            playerList.Add(Instantiate(player));

            // 配置
            if(i > rescueBorder2 - 1)
            {// 3隻目
                playerList[i].transform.parent = boat2.transform;
                playerList[i].transform.position = new Vector3(boat2.transform.position.x + (j * 3) - 9,
                   boat2.transform.position.y + 10,
                   boat2.transform.position.z);
            }

            else if(i > rescueBorder1 - 1)
            {// 2隻目
                playerList[i].transform.parent = boat1.transform;
                playerList[i].transform.position = new Vector3(boat1.transform.position.x + (j * 3) - 9,
                   boat1.transform.position.y + 10,
                   boat1.transform.position.z);
            }
            else
            {// 1隻目
                playerList[i].transform.parent = boat0.transform;
                playerList[i].transform.position = new Vector3(boat0.transform.position.x + (j * 3) - 8,
                   boat0.transform.position.y + 10,
                   boat0.transform.position.z);
            }

            if(j > rescueBorder1 - 1)
            {
                j = 0;
            }
        }

        Destroy(player);


        bOne = false;
        bninkyu = false;
        bninzuu = false;
        bhyoka = false;
        bBoatStop = false;

        // 評価の表示
        if (rescueNum > rescueBorder2)
        {// 大漁
            Tairyo.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
            Tairyo.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.0f);
            Destroy(Iikanji); // いい感じは削除
        }

        else if(rescueNum > rescueBorder1)
        {// いい感じ
            Iikanji.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
            Iikanji.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.0f);

            Destroy(Tairyo); // 大漁は削除
        }

        else
        {// 表示なし
            Destroy(Tairyo); // 大漁は削除
            Destroy(Iikanji); // いい感じは削除
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd && bBoatStop)
        {

            if (Input.GetButtonDown("Pad1_A") || Input.GetKeyDown(KeyCode.Return))
            {
                FadeObj.transform.SetAsLastSibling();
                FadeObj.GetComponent<FadeController>().SetFadeOut(true);
                FadeObj.GetComponent<FadeController>().SetFadeIn(false);
            }

            if (FadeObj.GetComponent<FadeController>().GetAlpha() >= 1.0f)
            {
                Debug.Log("入った");
                SceneManager.LoadScene("Title");
            }
            return;
        }

        elapsedTime += Time.deltaTime;

        if (rescueNum <= rescueBorder1)
        {
            boatNum = 1;
            //boatAlignNormal[0]
        }
        else if (rescueNum > rescueBorder1 && rescueNum <= rescueBorder2)
        {
            boatNum = 2;
        }
        else if (rescueNum > rescueBorder2)
        {
            boatNum = 3;
        }

        if (elapsedTime > boatStopTime)
        {   // ボートが止まった

            if (boatAlignNormal[0].rawForward != 0.0f)
                for (int i = 0; i < boatNum; i++)
                    boatAlignNormal[i].rawForward = 0.0f;

            bBoatStop = true;
        }
        else if (elapsedTime > boatStartTime)
        {// ボートが動いている
            if (boatAlignNormal[0].rawForward != 1.0f)
                for (int i = 0; i < boatNum; i++)
                    boatAlignNormal[i].rawForward = 1.0f;


        }

        if(!bninkyu)
        {
            ninkyujo.transform.position += ninkyujoSpeed;
            if(ninkyujo.transform.localPosition.x < -75f)
            {
                bninkyu = true;
                bninzuu = true;
            }
        }

        if (bninzuu)
        {
            for (int i = 0; i < WorkList.Count; ++i)
            {
                WorkList[i].transform.position += new Vector3(5.3f, 0.0f, 0.0f);

                if(WorkList[0].transform.localPosition.x > -510f)
                {
                    bninzuu = false;
                    bhyoka = true;
                }
            }

        }

        if(bhyoka)
        {
            // 評価の表示
            if (rescueNum > rescueBorder2)
            {// 大漁
                Tairyo.transform.localScale += new Vector3(-0.1f, -0.1f, -0.1f);

                Tairyo.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1.0f);
                if(Tairyo.transform.localScale.x <= 0.7f)
                {
                    Tairyo.transform.localScale = new Vector3(0.7f, 0.7f, 1.0f);
                    isEnd = true;
                }
            }

            else if (rescueNum > rescueBorder1)
            {// いい感じ
                Iikanji.transform.localScale += new Vector3(-0.1f, -0.1f, -0.1f);
                Iikanji.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1.0f);

                if (Iikanji.transform.localScale.x <= 1.0f)
                {
                    Iikanji.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    isEnd = true;
                }
            }
            else
            {
                isEnd = true;
            }

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
            }
           
          
            
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

            //numObj.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.6f);

            

            //graphic2 = numObj.GetComponent<Image>();


            //子供として登録
            numObj.transform.parent = canvas.transform;


            //現在チェックしている桁の数字を割り出す
            int digNum = ChkDigit.GetPointDigit(point, i + 1);

            //ポイントから数字を切り替える
            numObj.GetComponent<NumCtrl>().ChangeSprite(digNum);

            Vector3 pos = new Vector3(-1300 - i * 200, 229f, 0f);
            numObj.transform.localPosition = pos;
      //      numObj.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

            //          numObj = null;
            WorkList.Add(numObj);
        }

        Destroy(numSprite);
    }
}
