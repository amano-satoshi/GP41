using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraState
    {
        DEFAULT = 0,
        SHOOT_ONE,
        SHOOT_TWO,
        SHOOT_THREE,
        SHOOT_FOUR,

        MAX_CAMERASTATE
    }
    public GameObject MapCamera;
    private CameraState camerastate;
    private List<GameObject> CameraList = new List<GameObject>();
    [SerializeField, Header("マップカメラの配置"), CustomListLabelAttribute(new string[] {"発射前", "1機発射", "2機発射", "3機発射", "4機発射" })]
    private Rect[] MapRect = new Rect[(int)CameraState.MAX_CAMERASTATE];
    [SerializeField, Header("UI部分を除いた画面の大きさ")]
    private Vector2 DispSize = new Vector2(0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        camerastate = CameraState.DEFAULT;
    }

    // Update is called once per frame
    void Update()
    {
        switch(camerastate)
        {
            case CameraState.DEFAULT:
                MapCamera.GetComponent<Camera>().rect = MapRect[0];
                break;
            case CameraState.SHOOT_ONE:
                MapCamera.GetComponent<Camera>().rect = MapRect[1];
                CameraList[0].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x, DispSize.y);
                break;
            case CameraState.SHOOT_TWO:
                MapCamera.GetComponent<Camera>().rect = MapRect[2];
                CameraList[0].GetComponent<Camera>().rect = new Rect(0f, DispSize.y / 2f, DispSize.x, DispSize.y / 2f);
                CameraList[1].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x, DispSize.y / 2f);
                break;
            case CameraState.SHOOT_THREE:
                MapCamera.GetComponent<Camera>().rect = MapRect[3];
                CameraList[0].GetComponent<Camera>().rect = new Rect(0f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                CameraList[1].GetComponent<Camera>().rect = new Rect(DispSize.x / 2f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                CameraList[2].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x / 2f, DispSize.y / 2f);
                break;
            case CameraState.SHOOT_FOUR:
                MapCamera.GetComponent<Camera>().rect = MapRect[4];
                CameraList[0].GetComponent<Camera>().rect = new Rect(0f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                CameraList[1].GetComponent<Camera>().rect = new Rect(DispSize.x / 2f, DispSize.y / 2f, DispSize.x / 2f, DispSize.y / 2f);
                CameraList[2].GetComponent<Camera>().rect = new Rect(0f, 0f, DispSize.x / 2f, DispSize.y / 2f);
                CameraList[3].GetComponent<Camera>().rect = new Rect(DispSize.x / 2f, 0f, DispSize.x / 2f, DispSize.y / 2f);
                break;
            default:
                break;
        }
    }

    public void ChangeCameraState(CameraState state, List<GameObject> cameraList = null)
    {
        camerastate = state;
        if(cameraList != null)
        {
            CameraList = cameraList;
        }
    }

    public void ResetCameraState()
    {
        camerastate = CameraState.DEFAULT;
        CameraList.Clear();
    }
}
