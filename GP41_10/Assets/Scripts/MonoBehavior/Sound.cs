using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public int nNum;        // 0:タイトル       1:人数選択
                            // 2:メイン         3:リザルト

    private bool bKey;

    private AudioSource[] sources;
    void Start()
    {
        sources = gameObject.GetComponents<AudioSource>();
        sources[0].Play();      // BGM
        Debug.Log(sources[0].name + "再生");

        bKey = false;
    }


    void Update()
    {
        float pd1h = Input.GetAxis("Pad1_H");
         //L Stick
        float pd1lsh = Input.GetAxis("Pad1_LStick_H");

        switch (nNum)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    sources[1].Play();
                }
                break;

            case 1:
                if ((pd1lsh < -0.5f ||
                    pd1h > 0.3f ||
                    Input.GetKeyDown(KeyCode.W))
                    &&
                    bKey)
                {
                    sources[1].Play();
                    Debug.Log("CURSOR選択音");
                    bKey = false;
                }
                else if ((pd1lsh > 0.5f ||
                        pd1h < -0.3f
                           || Input.GetKeyDown(KeyCode.S)) &&
                    !bKey)
                {
                    sources[2].Play();
                    Debug.Log("CURSOR選択音");
                    bKey = true;
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    sources[3].Play();
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