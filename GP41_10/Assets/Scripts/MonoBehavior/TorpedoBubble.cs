using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoBubble : MonoBehaviour
{
    public GameObject TorpedoBubbleObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(TorpedoBubbleObj, transform.position, transform.rotation);
    }
}
