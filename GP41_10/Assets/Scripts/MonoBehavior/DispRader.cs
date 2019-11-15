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
        transform.position += new Vector3(0f, 0.1f, 0f);
    }
}
