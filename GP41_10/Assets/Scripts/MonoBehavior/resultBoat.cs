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
    private Graphic graphic0, graphic1; // 格納用
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
        }
    }
    
    public void SetAlpha(Graphic self, float alpha)
    {
        var color = self.color;
        color.a = alpha;
        self.color = color;
    }
}
