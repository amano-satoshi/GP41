using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boatmove : MonoBehaviour
{
    private float sTime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sTime -= Time.deltaTime;
        if(sTime < 0f)
        GetComponent<UTJ.Alembic.AlembicStreamPlayer>().currentTime += 2f * Time.deltaTime;
    }
}
