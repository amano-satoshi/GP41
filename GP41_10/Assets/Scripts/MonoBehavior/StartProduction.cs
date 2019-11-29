using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartProduction : MonoBehaviour
{
    private float ProductionTime = 4f;
    private float Remaintime;
    private bool Endflg = false;
    private bool Delflg = false;
    private int count = 2;
    private Image CutIn;
    private Text Ready;
    private Text Go;
    // Start is called before the first frame update
    void Start()
    {
        Remaintime = ProductionTime;
        CutIn = transform.GetChild(0).GetComponent<Image>();
        Ready = transform.GetChild(1).GetComponent<Text>();
        Go = transform.GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Remaintime -= Time.deltaTime;
        if(Remaintime < 0f)
        {
            Remaintime = 0f;
        }
        if (Mathf.FloorToInt(Remaintime) == 2 && count == 2)
        {
            Ready.text = "READY";
            count--;
        }
        else if(Mathf.FloorToInt(Remaintime) == 1 && count == 1)
        {
            Destroy(Ready.gameObject);
            Go.text = "GO!!";
            count--;
        }
        else if(Mathf.FloorToInt(Remaintime) == 0 && count == 0)
        {
            Destroy(Go.gameObject);
            Destroy(CutIn.gameObject);
            Endflg = true;
            count--;
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
}
