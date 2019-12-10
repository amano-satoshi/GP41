﻿///////////////////////////////////////////////////////////////
//
// 魚雷追従カメラ(TorpedoCamera : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/11
//
///////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorpedoCamera : MonoBehaviour
{
    private GameObject target = null;    // 魚雷
    [SerializeField, Header("目的地に到達するまでの時間")]
    private float smoothing = 0f;
    [SerializeField, Header("成功失敗表示位置")]
    private Vector2 DispPos = Vector2.zero;
    private Vector3 offset = new Vector3(0f, 0f, 0f); // オフセット
    private bool destflg = false;
    public Canvas success;
    public Canvas failed;
    private Canvas result = null;
    private bool TextDisp = false;
    private int camID = 0;

    private GameObject mapCamera;
    private GameObject stageSpawner;
    private List<GameObject> cameraList;

    public AudioClip[] sounds = new AudioClip[2];
    AudioSource audioSource;

    void Start()
    {
        mapCamera = GameObject.Find("MapCamera");
        stageSpawner = GameObject.Find("StageSpawner");
        audioSource = mapCamera.GetComponent<AudioSource>();
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
                audioSource.PlayOneShot(sounds[0]);
            }
            else if(target.GetComponent<TorpedoBehavior>().torpedostate == TorpedoBehavior.TorpedoState.FAILED && !TextDisp)
            {
                DispResult(false);
                TextDisp = true;
                //audioSource.PlayOneShot(sounds[1]);
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

    public void SetCamID(int ID)
    {
        camID = ID;
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
            //result.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            //result.GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();
            //result.GetComponent<Canvas>().planeDistance = 1;
            result.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            cameraList = stageSpawner.GetComponent<StageSpawner>().GetTorpedoCamera();
            if(cameraList.Count == 2)
            {
                if(camID == 0)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(0f, DispPos.y, 0f);
                }
                else if (camID == 1)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(0f, DispPos.y, 0f);
                }
            }
            else if (cameraList.Count == 3)
            {
                if (camID == 0)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(-DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 1)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 2)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(DispPos.x, DispPos.y, 0f);
                }
            }
            else if (cameraList.Count == 4)
            {
                if (camID == 0)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(-DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 1)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 2)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 3)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(-DispPos.x, DispPos.y, 0f);
                }
            }
        }
        else
        {
            result = Instantiate(failed);
            //result.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            //result.GetComponent<Canvas>().worldCamera = this.gameObject.GetComponent<Camera>();
            //result.GetComponent<Canvas>().planeDistance = 1;
            result.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            cameraList = stageSpawner.GetComponent<StageSpawner>().GetTorpedoCamera();
            if (cameraList.Count == 2)
            {
                if (camID == 0)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(0f, DispPos.y, 0f);
                }
                else if (camID == 1)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(0f, DispPos.y, 0f);
                }
            }
            else if (cameraList.Count == 3)
            {
                if (camID == 0)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(-DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 1)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 2)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(DispPos.x, DispPos.y, 0f);
                }
            }
            else if (cameraList.Count == 4)
            {
                if (camID == 0)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(-DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 1)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        += new Vector3(DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 2)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(DispPos.x, DispPos.y, 0f);
                }
                else if (camID == 3)
                {
                    result.GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().rectTransform.position
                        -= new Vector3(-DispPos.x, DispPos.y, 0f);
                }
            }
        }
    }
}
