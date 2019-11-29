using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProduction : MonoBehaviour
{
    private float ProductionTime = 4f;
    private float Remaintime;
    private bool Startflg = false;
    private bool Endflg = false;
    private bool Delflg = false;
    private int count = 1;
    private Vector3 EndPos;
    private Image CutIn;
    private Text Shoot;
    // Start is called before the first frame update
    void Start()
    {
        Remaintime = ProductionTime;
        CutIn = transform.GetChild(0).GetComponent<Image>();
        Shoot = transform.GetChild(1).GetComponent<Text>();
        EndPos = new Vector3(-21f, 0f, 0f);
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
            Shoot.text = "発射!!";
            count--;
            Startflg = true;
        }
        else if (Mathf.FloorToInt(Remaintime) == 0 && count == 0)
        {
            Destroy(Shoot.gameObject);
            Destroy(CutIn.gameObject);
            Endflg = true;
            count--;
        }

        if(Startflg)
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

    void MoveProduction()
    {
        CutIn.rectTransform.position -= new Vector3(1242f * Time.deltaTime, 0f, 0f);
        Shoot.rectTransform.position -= new Vector3(1242f * Time.deltaTime, 0f, 0f);
        if(CutIn.rectTransform.localPosition.x <= EndPos.x)
        {
            CutIn.rectTransform.localPosition = EndPos;
            Shoot.rectTransform.localPosition = EndPos;
            Startflg = false;
        }
    }
}
