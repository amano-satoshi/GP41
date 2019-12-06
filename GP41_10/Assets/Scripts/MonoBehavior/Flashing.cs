using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour
{
    private float Alpha = 1f;
    private bool UpAlpha = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!UpAlpha)
        {
            Alpha -= 2f * Time.deltaTime;
            if(Alpha <= 0f)
            {
                Alpha = 0f;
                UpAlpha = true;
            }
        }
        else
        {
            Alpha += 2f * Time.deltaTime;
            if (Alpha >= 1f)
            {
                Alpha = 1f;
                UpAlpha = false;
            }
        }

        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, 
            GetComponent<Image>().color.b, Alpha);
    }
}
