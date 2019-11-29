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
    private bool destflg = false;
    public Canvas success;
    public Canvas failed;
    private Canvas result = null;
    private bool TextDisp = false;

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

            if(target.GetComponent<TorpedoBehavior>().torpedostate == TorpedoBehavior.TorpedoState.RESCUE && !TextDisp)
            {
                DispResult(true);
                TextDisp = true;
            }
            else if(target.GetComponent<TorpedoBehavior>().torpedostate == TorpedoBehavior.TorpedoState.FAILED && !TextDisp)
            {
                DispResult(false);
                TextDisp = true;
            }
        }

        if (destflg)
        {
            Destroy(result.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void SetTorpedo(GameObject torpedo, Vector3 Offset)
    {
        target = torpedo;
        offset = Offset;
    }

    public void DestTorpedoCamera()
    {
        destflg = true;
    }

    void DispResult(bool successflg)
    {
        if(successflg)
        {
            result = Instantiate(success);
            result.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            result.GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();
            result.GetComponent<Canvas>().planeDistance = 1;
        }
        else
        {
            result = Instantiate(failed);
            result.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            result.GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();
            result.GetComponent<Canvas>().planeDistance = 1;
        }
    }
}
