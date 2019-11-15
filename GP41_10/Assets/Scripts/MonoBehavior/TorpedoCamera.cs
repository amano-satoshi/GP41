///////////////////////////////////////////////////////////////
//
// 魚雷追従カメラ(TorpedoCamera : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/11
//
///////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoCamera : MonoBehaviour
{
    private GameObject target = null;    // 魚雷
    [SerializeField, Header("目的地に到達するまでの時間")]
    private float smoothing = 0f;
    private Vector3 offset = new Vector3(0f, 0f, 0f); // オフセット

    void Start()
    {
        
    }

    void Update()
    {
        if(target != null)
        {
            // 滑らかに追従
            Vector3 targetCamPos = target.transform.position + target.transform.forward * offset.z + target.transform.up * offset.y + transform.right * offset.x;
            transform.position = Vector3.Lerp(
                transform.position,
                targetCamPos,
                Time.deltaTime * smoothing
            );
            transform.rotation = target.transform.rotation;
        }
    }

    public void SetTorpedo(GameObject torpedo, Vector3 Offset)
    {
        target = torpedo;
        offset = Offset;
    }
}
