using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcamera : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera TestCamera;
    public GameObject TestObj;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            TestCamera.transform.position += new Vector3(0f, 0f, 1.1f);
            TestObj.transform.position += new Vector3(0f, 0f, 1.1f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            TestCamera.transform.position += new Vector3(-1.1f, 0f, 0f);
            TestObj.transform.position += new Vector3(-1.1f, 0f, 0f);
            
        }

        if (Input.GetKey(KeyCode.S))
        {
            TestCamera.transform.position += new Vector3(0f, 0f, -1.1f);
            TestObj.transform.position += new Vector3(0f, 0f, -1.1f);
            
        }

        if (Input.GetKey(KeyCode.D))
        {
            TestCamera.transform.position += new Vector3(1.1f, 0f, 0f);
            TestObj.transform.position += new Vector3(1.1f, 0f, 0f);
            
        }
    }
}
