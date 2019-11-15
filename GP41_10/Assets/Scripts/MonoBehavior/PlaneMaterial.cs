using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(0.6f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
