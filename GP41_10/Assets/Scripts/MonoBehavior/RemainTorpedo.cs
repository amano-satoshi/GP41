///////////////////////////////////////////////////////////////
//
// 残り魚雷数表示(RemainTorpedo : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/12/01
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainTorpedo : MonoBehaviour
{
    [SerializeField, Header("残り魚雷数")]
    Image[] images = new Image[4];
    public Image[] Readyimages = new Image[2];
    public GameObject stageSpawner;
    public GameObject playerController;
    private StageSpawner stageSpawnerObj;
    private PlayerController playerControllerObj;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; ++i)
        {
            images[i].enabled = true;
        }
        stageSpawnerObj = stageSpawner.GetComponent<StageSpawner>();
        playerControllerObj = playerController.GetComponent<PlayerController>();
        Readyimages[0].enabled = false;
        Readyimages[1].enabled = false;
        if (StageData.GetPlayerNum() == 1)
        {
            for (int i = 2; i < 4; ++i)
            {
                images[i].color = Color.red;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 1人用
        if (StageData.GetPlayerNum() == 0)
        {
            switch (stageSpawnerObj.GetRemainTorpedo()[0])
            {
                case 0:
                    for (int i = 0; i < 4; ++i)
                    {
                        images[i].enabled = false;
                    }
                    break;
                case 1:
                    images[0].enabled = true;
                    for (int i = 1; i < 4; ++i)
                    {
                        images[i].enabled = false;
                    }
                    break;
                case 2:
                    for (int i = 0; i < 2; ++i)
                    {
                        images[i].enabled = true;
                    }
                    for (int i = 2; i < 4; ++i)
                    {
                        images[i].enabled = false;
                    }
                    break;
                case 3:
                    for (int i = 0; i < 3; ++i)
                    {
                        images[i].enabled = true;
                    }
                    for (int i = 3; i < 4; ++i)
                    {
                        images[i].enabled = false;
                    }
                    break;
                case 4:
                    for (int i = 0; i < 4; ++i)
                    {
                        images[i].enabled = true;
                    }
                    break;
                default:
                    break;
            }
        }
        // 2人用
        else if (StageData.GetPlayerNum() == 1)
        {
            switch (stageSpawnerObj.GetRemainTorpedo()[0])
            {
                case 0:
                    for (int i = 2; i < 4; ++i)
                    {
                        images[i].enabled = false;
                    }
                    break;
                case 1:
                    images[2].enabled = true;
                    images[3].enabled = false;
                    break;
                case 2:
                    for (int i = 2; i < 4; ++i)
                    {
                        images[i].enabled = true;
                    }
                    break;
                default:
                    break;
            }
            switch (stageSpawnerObj.GetRemainTorpedo()[1])
            {
                case 0:
                    for (int i = 0; i < 2; ++i)
                    {
                        images[i].enabled = false;
                    }
                    break;
                case 1:
                    images[0].enabled = true;
                    images[1].enabled = false;
                    break;
                case 2:
                    for (int i = 0; i < 2; ++i)
                    {
                        images[i].enabled = true;
                    }
                    break;
                default:
                    break;
            }
            Readyimages[0].enabled = playerControllerObj.GetShootFlg()[0];
            Readyimages[1].enabled = playerControllerObj.GetShootFlg()[1];
        }
    }
}
