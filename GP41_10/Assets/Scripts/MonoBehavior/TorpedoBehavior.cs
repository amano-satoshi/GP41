///////////////////////////////////////////////////////////////
//
// 魚雷の挙動(TorpedoBehavior : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/11
// 更新日 : 2019/11/15　AI作成
//
///////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoBehavior : MonoBehaviour
{
    enum CheckPlace
    {
        UP = 0,
        Left,
        Down,
        Right,

        MAX_PLACE
    }

    public enum TorpedoState
    {
        SEARCH = 0,
        RESCUE,
        EMERGENCY,
        RETURN,

        MAX_STATE
    }
    [SerializeField,Header("前進する速度")]
    private float TorpedoSpeed = 0; // 前進する速度
    [SerializeField, Header("前進する最大速度")]
    private float TorpedoMaxSpeed = 0; // 前進する最大速度
    [SerializeField, Header("前進する最低速度")]
    private float TorpedoMinSpeed = 0; // 前進する最低速度
    [SerializeField, Header("前進する速度の変化量")]
    private float TorpedoChangeSpeed = 0; // 前進する速度の変化量
    [SerializeField, Header("魚雷の傾く最大角度")]
    private float TorpedoMaxRot = 0; // 魚雷の傾く最大角度
    [SerializeField, Header("魚雷の傾く速度")]
    private float TorpedoRotSpeed = 0; // 魚雷の傾く速度
    private float TorpedoRotUD = 0; // 魚雷の現在角度（上下）
    private float TorpedoRotLR = 0; // 魚雷の現在角度（左右）
    [SerializeField, Header("障害物を感知する距離")]
    private int distance = 0;
    [SerializeField, Header("障害物を感知する範囲")]
    private float range = 0;
    private Ray ray, ray1, ray2;
    private RaycastHit hit;
    private bool[] hitplace = new bool[(int)CheckPlace.MAX_PLACE];
    private GameObject target;
    private float timeOut = 0.3f;
    private float timeOut2 = 0.3f;
    private float timeElapsed;
    private TorpedoState torpedoState = TorpedoState.SEARCH;
    private bool[] SearchDirection = new bool[(int)CheckPlace.MAX_PLACE];
    private bool destflg = false;

    private List<GameObject> DrowningPersonList = new List<GameObject>();
    private GameObject StageInfo;
    private TorpedoNPCBehavior TorpedoNPC;
    public float torpedospeed { get { return TorpedoSpeed; } }
    public TorpedoState torpedostate { get { return torpedoState; } }
    // ===================== Start =======================
    void Start()
    {
        StageInfo = GameObject.Find("StageSpawner");
        //target = GameObject.Find("person");
    }

    // ===================== Update =======================
    void Update()
    {
        timeElapsed += Time.deltaTime;
        
        switch(torpedoState)
        {
            case TorpedoState.SEARCH:
                Search();
                break;
            case TorpedoState.RESCUE:
                Rescue();
                break;
            case TorpedoState.EMERGENCY:
                decel();
                if(!SearchDirection[(int)CheckPlace.UP])
                {
                    turn(CheckPlace.UP);
                }
                else if (!SearchDirection[(int)CheckPlace.Left])
                {
                    turn(CheckPlace.Left);
                }
                else if (!SearchDirection[(int)CheckPlace.Down])
                {
                    turn(CheckPlace.Down);
                }
                else if (!SearchDirection[(int)CheckPlace.Right])
                {
                    turn(CheckPlace.Right);
                }
                if (TorpedoSpeed < TorpedoMinSpeed && timeElapsed >= timeOut2)
                {
                    torpedoState = TorpedoState.SEARCH;
                    if (TorpedoRotUD >= TorpedoMaxRot)
                        SearchDirection[(int)CheckPlace.UP] = true;
                    else if(TorpedoRotLR <= -TorpedoMaxRot)
                        SearchDirection[(int)CheckPlace.Left] = true;
                    else if (TorpedoRotUD <= -TorpedoMaxRot)
                        SearchDirection[(int)CheckPlace.Down] = true;
                    else if (TorpedoRotLR >= TorpedoMaxRot)
                        SearchDirection[(int)CheckPlace.Right] = true;
                    timeElapsed += Time.deltaTime;
                }
                break;
            case TorpedoState.RETURN:
                
                break;
            default:
                break;
        }

        if(destflg)
        {
            Destroy(this.gameObject);
        }
    }

    // 上向き
    void MoveUp()
    {
        Vector3 axis = transform.right; // 回転軸
        float angle = -TorpedoRotSpeed * Time.deltaTime; // 回転の角度
        TorpedoRotUD += angle;
        if (TorpedoRotUD < -TorpedoMaxRot)
        {
            TorpedoRotUD = -TorpedoMaxRot;
        }
        else
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis); // 軸axisの周りにangle回転させるクォータニオン
            transform.rotation = q * transform.rotation; // クォータニオンで回転させる
        }
    }

    // 下向き
    void MoveDown()
    {
        Vector3 axis = transform.right; // 回転軸
        float angle = TorpedoRotSpeed * Time.deltaTime; // 回転の角度
        TorpedoRotUD += angle;
        if (TorpedoRotUD > TorpedoMaxRot)
        {
            TorpedoRotUD = TorpedoMaxRot;
        }
        else
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis); // 軸axisの周りにangle回転させるクォータニオン
            transform.rotation = q * transform.rotation; // クォータニオンで回転させる
        }
    }

    // 左向き
    void MoveLeft()
    {
        Vector3 axis = transform.up; // 回転軸
        float angle = -TorpedoRotSpeed * Time.deltaTime; // 回転の角度
        TorpedoRotLR += angle;
        if (TorpedoRotLR < -TorpedoMaxRot)
        {
            TorpedoRotLR = -TorpedoMaxRot;
        }
        else
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis); // 軸axisの周りにangle回転させるクォータニオン
            transform.rotation = q * transform.rotation; // クォータニオンで回転させる
        }
    }

    // 右向き
    void MoveRight()
    {
        Vector3 axis = transform.up; // 回転軸
        float angle = TorpedoRotSpeed * Time.deltaTime; // 回転の角度
        TorpedoRotLR += angle;
        if (TorpedoRotLR > TorpedoMaxRot)
        {
            TorpedoRotLR = TorpedoMaxRot;
        }
        else
        {
            Quaternion q = Quaternion.AngleAxis(angle, axis); // 軸axisの周りにangle回転させるクォータニオン
            transform.rotation = q * transform.rotation; // クォータニオンで回転させる
        }
    }

    // 加速
    void accel()
    {
        TorpedoSpeed += TorpedoChangeSpeed;
    }

    // 減速
    void decel()
    {
        TorpedoSpeed -= TorpedoChangeSpeed;
    }

    // 障害物感知
    void HitCheck()
    {
        // 上チェック
        ray = new Ray(transform.position + transform.up * 0.5f + transform.up * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);
        //Debug.DrawLine(transform.position + transform.up * 0.5f + transform.up * range, (transform.position + transform.up * 0.5f + transform.up * range) + transform.forward * distance, Color.green);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.UP] = true;
        }
        else
        {
            hitplace[(int)CheckPlace.UP] = false;
        }

        // 左チェック
        ray = new Ray(transform.position + transform.up * 0.5f + -transform.right * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.Left] = true;
        }
        else
        {
            hitplace[(int)CheckPlace.Left] = false;
        }

        // 下チェック
        ray = new Ray(transform.position + transform.up * 0.5f + -transform.up * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.blue);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.Down] = true;
        }
        else
        {
            hitplace[(int)CheckPlace.Down] = false;
        }

        // 右チェック
        ray = new Ray(transform.position + transform.up * 0.5f + transform.right * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.yellow);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.Right] = true;
        }
        else
        {
            hitplace[(int)CheckPlace.Right] = false;
        }

        // 左上チェック
        Vector3 vec = (transform.up + -transform.right).normalized;
        ray = new Ray(transform.position + transform.up * 0.5f + vec * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.Left] = true;
            hitplace[(int)CheckPlace.UP] = true;
        }

        // 左下チェック
        vec = (-transform.up + -transform.right).normalized;
        ray = new Ray(transform.position + transform.up * 0.5f + vec * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.blue);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.Left] = true;
            hitplace[(int)CheckPlace.Down] = true;
        }

        // 右下チェック
        vec = (-transform.up + transform.right).normalized;
        ray = new Ray(transform.position + transform.up * 0.5f + vec * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.yellow);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.Right] = true;
            hitplace[(int)CheckPlace.Down] = true;
        }

        // 右上チェック
        vec = (transform.up + transform.right).normalized;
        ray = new Ray(transform.position + transform.up * 0.5f + vec * range, transform.forward);

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
        {
            hitplace[(int)CheckPlace.Right] = true;
            hitplace[(int)CheckPlace.UP] = true;
        }
    }

    // 回避
    void Avoidance()
    {
        bool up, left, down, right;
        up = hitplace[(int)CheckPlace.UP];
        left = hitplace[(int)CheckPlace.Left];
        down = hitplace[(int)CheckPlace.Down];
        right = hitplace[(int)CheckPlace.Right];
        if ((!up && !left && down && right) || (!up && !left && !down && right) || (up && !left && !down && right) ||
            (up && !left && down && !right) || (up && !left && down && right))
        {
            // 左に回避
            MoveLeft();
        }
        
        else if ((!up && left && down && !right) || (!up && left && !down && !right) || (up && left && !down && !right) || 
            (up && left && down && !right))
        {
            // 右に回避
            MoveRight();
        }
        
        else if ((!up && left && down && right) || (!up && !left && down && !right) || (!up && left && !down && right))
        {
            // 上に回避
            MoveUp();
        }
        
        else if ((up && !left && !down && !right) || (up && left && !down && right))
        {
            // 下に回避
            MoveDown();
        }
        // 全部当たっている
        else if (up && left && down && right)
        {
            // 左チェック
            Vector3 vec = (transform.forward + -transform.right).normalized;
            vec = (transform.forward + vec).normalized;
            vec = (transform.forward + vec).normalized;
            ray = new Ray(transform.position + transform.up * 0.5f + transform.right * range, vec);

            //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);

            Vector3 vec1 = (transform.up + transform.right).normalized;
            ray1 = new Ray(transform.position + transform.up * 0.5f + vec1 * range, vec);
            //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
            Debug.DrawLine(ray1.origin, ray1.origin + ray1.direction * distance, Color.green);

            Vector3 vec2 = (-transform.up + transform.right).normalized;
            ray2 = new Ray(transform.position + transform.up * 0.5f + vec2 * range, vec);
            //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
            Debug.DrawLine(ray2.origin, ray2.origin + ray2.direction * distance, Color.green);

            if ((Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle") || (Physics.Raycast(ray1, out hit, distance) && hit.collider.tag == "Obstacle") ||
                (Physics.Raycast(ray2, out hit, distance) && hit.collider.tag == "Obstacle"))
            {
                // 右チェック
                vec = (transform.forward + transform.right).normalized;
                vec = (transform.forward + vec).normalized;
                vec = (transform.forward + vec).normalized;
                ray = new Ray(transform.position + transform.up * 0.5f - transform.right * range, vec);
                //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);

                vec1 = (transform.up + -transform.right).normalized;
                ray1 = new Ray(transform.position + transform.up * 0.5f + vec1 * range, vec);
                //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
                Debug.DrawLine(ray1.origin, ray1.origin + ray1.direction * distance, Color.green);

                vec2 = (-transform.up + -transform.right).normalized;
                ray2 = new Ray(transform.position + transform.up * 0.5f + vec2 * range, vec);
                //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
                Debug.DrawLine(ray2.origin, ray2.origin + ray2.direction * distance, Color.green);

                if ((Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle") || (Physics.Raycast(ray1, out hit, distance) && hit.collider.tag == "Obstacle") ||
                (Physics.Raycast(ray2, out hit, distance) && hit.collider.tag == "Obstacle"))
                {
                    // 上チェック
                    vec = (transform.forward + transform.up).normalized;
                    vec = (transform.forward + vec).normalized;
                    vec = (transform.forward + vec).normalized;
                    ray = new Ray(transform.position + transform.up * 0.5f - transform.up * range, vec);

                    //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);
                    if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
                    {
                        // 下チェック
                        vec = (transform.forward - transform.up).normalized;
                        vec = (transform.forward + vec).normalized;
                        vec = (transform.forward + vec).normalized;
                        ray = new Ray(transform.position + transform.up * 0.5f + transform.up * range, vec);

                        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　↓Rayの色
                        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.green);
                        if (Physics.Raycast(ray, out hit, distance) && hit.collider.tag == "Obstacle")
                        {
                            torpedoState = TorpedoState.EMERGENCY;
                        }
                        else
                        {
                            MoveDown();
                        }
                    }
                    else
                    {
                        MoveUp();
                    }
                }
                else
                {
                    MoveRight();
                }
            }
            else
            {
                MoveLeft();
            }
        }
    }

    void turn(CheckPlace searchDirection)
    {
        switch(searchDirection)
        {
            case CheckPlace.UP:
                MoveUp();
                break;
            case CheckPlace.Left:
                MoveLeft();
                break;
            case CheckPlace.Down:
                MoveDown();
                break;
            case CheckPlace.Right:
                MoveRight();
                break;
            default:
                break;
        }
    }

    void Search()
    {
        if((target.transform.position - (transform.position + transform.up)).magnitude < 1f)
        {
            torpedoState = TorpedoState.RESCUE;
            target.GetComponent<DrowningPersonBehavior>().Rescued();
            StageData.IncreaseRescuePersonCnt();
        }
        else if ((target.transform.position - (transform.position + transform.up)).magnitude < 10f && timeElapsed >= timeOut2)
        {
            if (TorpedoSpeed > TorpedoMinSpeed) decel();
            timeElapsed = 0.0f;
        }
        else if ((target.transform.position - (transform.position + transform.up)).magnitude < 30f && timeElapsed >= timeOut)
        {
            if (TorpedoSpeed > TorpedoMaxSpeed / 2f) decel();
            else if (TorpedoSpeed < TorpedoMaxSpeed / 2f) accel();
            timeElapsed = 0.0f;
        }
        else if((target.transform.position - (transform.position + transform.up)).magnitude > 30f)
        {
            accel();
        }
        // --------------- 移動 -----------------
        transform.position += transform.forward * TorpedoSpeed * Time.deltaTime;
       

        // --------- 障害物感知 ---------
        HitCheck();

        // 障害物なし
        if (!hitplace[(int)CheckPlace.Down] && !hitplace[(int)CheckPlace.Left] && !hitplace[(int)CheckPlace.UP] && !hitplace[(int)CheckPlace.Right])
        {
            Vector3 vec = (target.transform.position - (transform.position + transform.up)).normalized;
            // 救出者が右にいる
            if (Vector3.Dot(vec, transform.right) <= 1f && Vector3.Dot(vec, transform.right) > 0.1f)
            {
                MoveRight();
            }
            // 救出者が左にいる
            else if (Vector3.Dot(vec, transform.right) >= -1f && Vector3.Dot(vec, transform.right) < 0.1f)
            {
                MoveLeft();
            }
            // 救出者が上にいる
            if (Vector3.Dot(vec, transform.up) <= 1f && Vector3.Dot(vec, transform.up) > 0.1f)
            {
                MoveUp();
            }
            // 救出者が下にいる
            else if (Vector3.Dot(vec, transform.up) >= -1f && Vector3.Dot(vec, transform.up) < 0.1f)
            {
                MoveDown();
            }
        }
        // 障害物あり
        else
        {
            // --------- 回避 ---------
            Avoidance();
        }

        // --------- 前進速度制限 -----------
        // 最大
        if (TorpedoSpeed > TorpedoMaxSpeed)
        {
            TorpedoSpeed = TorpedoMaxSpeed;
        }
        // 最小
        if (TorpedoSpeed < TorpedoMinSpeed)
        {
            TorpedoSpeed = TorpedoMinSpeed;
        }
    }

    void Rescue()
    {
        TorpedoSpeed = 0f;
        Vector3 axis = transform.right; // 回転軸
        if(TorpedoRotUD != 0f)
        {
            float angle = -TorpedoRotUD; // 回転の角度
            Quaternion q = Quaternion.AngleAxis(angle, axis); // 軸axisの周りにangle回転させるクォータニオン
            transform.rotation = q * transform.rotation; // クォータニオンで回転させる
            TorpedoRotUD -= TorpedoRotUD;
        }
    }
    // おぼれている人の情報セット
    public void SetDrowningPerson(GameObject DrowningPerson)
    {
        if (DrowningPersonList.Count != 0)
        {
            for (int i = 0; i < DrowningPersonList.Count; ++i)
            {
                if (DrowningPersonList[i].GetComponent<DrowningPersonBehavior>().GetID() ==
                    DrowningPerson.GetComponent<DrowningPersonBehavior>().GetID())
                {
                    return;
                }
            }
            DrowningPersonList.Add(DrowningPerson);
        }
        else
        {
            DrowningPersonList.Add(DrowningPerson);
        }
        target = DrowningPersonList[0];
        transform.LookAt(target.transform.position);
    }

    // おぼれている人の情報削除
    public void DeleteDrowningPerson(GameObject DrowningPerson)
    {
        if (DrowningPersonList.Count == 0)
            return;
        for(int i = 0; i < DrowningPersonList.Count; ++i)
        {
            if(DrowningPersonList[i].GetComponent<DrowningPersonBehavior>().GetID() == 
                DrowningPerson.GetComponent<DrowningPersonBehavior>().GetID())
            {
                DrowningPersonList.RemoveAt(i);
            }
        }
    }

    public void SetTarget(GameObject Target)
    {
        target = Target;
        transform.LookAt(target.transform.position);
    }

    public void DestTorpedo()
    {
        destflg = true;
    }
}
