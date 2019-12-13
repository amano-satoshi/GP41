using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public int nNum;        // 0:タイトル       1:人数選択
                            // 2:メイン         3:リザルト

    private AudioSource[] sources;
    void Start()
    {
        sources = gameObject.GetComponents<AudioSource>();
        sources[0].Play();      // BGM
        Debug.Log(sources[0].name + "再生");
    }


    void Update()
    {
        switch(nNum)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    sources[1].Play();
                }
                break;

            case 1:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
                {
                    sources[1].Play();
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    sources[2].Play();
                }
                break;

            case 2:
                break;

            case 3:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    sources[1].Play();
                }
                break;

            default:
                break;

        }
        
    }
}