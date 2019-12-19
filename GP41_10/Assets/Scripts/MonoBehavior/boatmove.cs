using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boatmove : MonoBehaviour
{
    private float sTime = 2f;
    private bool AnimationFlg;
    // Start is called before the first frame update
    void Start()
    {
        AnimationFlg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(AnimationFlg)
        {
            sTime -= Time.deltaTime;
            if (sTime < 0f)
                GetComponent<UTJ.Alembic.AlembicStreamPlayer>().currentTime += 2f * Time.deltaTime;
        }
    }

    public void StartAnimation()
    {
        AnimationFlg = true;
    }
}
