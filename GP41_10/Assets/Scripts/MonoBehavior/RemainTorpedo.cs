using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainTorpedo : MonoBehaviour
{
    [SerializeField, Header("残り魚雷数")]
    Image[] images = new Image[4];
    public GameObject stageSpawner;
    private StageSpawner stageSpawnerObj;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; ++i)
        {
            images[i].enabled = true;
        }
        stageSpawnerObj = stageSpawner.GetComponent<StageSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(stageSpawnerObj.GetTargets().Count)
        {
            case 0:
                for (int i = 0; i < 4; ++i)
                {
                    images[i].enabled = true;
                }
                break;
            case 1:
                for (int i = 0; i < 3; ++i)
                {
                    images[i].enabled = true;
                }
                for (int i = 3; i < 4; ++i)
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
                images[0].enabled = true;
                for (int i = 1; i < 4; ++i)
                {
                    images[i].enabled = false;
                }
                break;
            case 4:
                for (int i = 0; i < 4; ++i)
                {
                    images[i].enabled = false;
                }
                break;
            default:
                break;
        }
    }
}
