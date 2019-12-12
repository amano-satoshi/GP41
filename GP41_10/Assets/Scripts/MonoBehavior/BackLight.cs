using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackLight : MonoBehaviour
{
    public Image BackLightImage;
    public Color color;
    public float AlphaSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(color.a > 1.0f || color.a < 0.0f)
        {
            AlphaSpeed *= -1;
        }

        color.a += AlphaSpeed;

        BackLightImage.color = color;
    }
}
