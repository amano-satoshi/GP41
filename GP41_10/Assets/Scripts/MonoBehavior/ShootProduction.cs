///////////////////////////////////////////////////////////////
//
// 発射演出(ShootProduction : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/29
//
///////////////////////////////////////////////////////////////
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
    [SerializeField, Header("カットインスピード")]
    private float CutInSpeed = 1242f;
    private GameObject mapCamera;

    public AudioClip sound1;
    AudioSource[] audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Remaintime = ProductionTime;
        CutIn = transform.GetChild(0).GetComponent<Image>();
        EndPos = new Vector3(-66f, 0f, 0f);
        mapCamera = GameObject.Find("MapCamera");
        audioSource = mapCamera.GetComponents<AudioSource>();
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
            audioSource[2].PlayOneShot(sound1);
            count--;
            Startflg = true;
        }
        else if (Mathf.FloorToInt(Remaintime) == 0 && count == 0)
        {
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
        CutIn.rectTransform.position -= new Vector3(CutInSpeed * Time.deltaTime, 0f, 0f);
        if(CutIn.rectTransform.localPosition.x <= EndPos.x)
        {
            CutIn.rectTransform.localPosition = EndPos;
            Startflg = false;
        }
    }
}
