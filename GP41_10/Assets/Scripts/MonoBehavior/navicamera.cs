using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navicamera : MonoBehaviour
{
    public GameObject target;    // 魚雷
    private float smoothing = 3f;
    private Vector3 offset = new Vector3(0f, 1f, -2.5f); // オフセット
    Quaternion targetRot;
    private float timeOut = 0f;
    private float timeElapsed;

    void Start()
    {
        targetRot = target.transform.rotation;
    }

    void Update()
    {
        if (target != null)
        {
            timeElapsed += Time.deltaTime;
            // 滑らかに追従
            Vector3 targetCamPos = target.transform.position + target.transform.forward * offset.z + target.transform.up * offset.y + transform.right * offset.x;
            transform.position = Vector3.Lerp(
                transform.position,
                targetCamPos,
                Time.deltaTime * smoothing
            );
            if (timeElapsed >= timeOut)
            {
                targetRot = target.transform.rotation;
                timeElapsed = 0.0f;
            }
            //transform.rotation = target.transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, Time.deltaTime * smoothing);
        }   
    }
}
