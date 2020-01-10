///////////////////////////////////////////////////////////////
//
// レーダー表示(DispRader : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/02
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispRader : MonoBehaviour
{
    public GameObject Rader;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(0f, 0f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Rader.transform.position;
        transform.position += new Vector3(0f, 5f, 0f);
    }
}
