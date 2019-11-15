///////////////////////////////////////////////////////////////
//
// 並走する魚雷の挙動(TorpedoNPCBehavior : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/10/18
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoNPCBehavior : MonoBehaviour
{
    private enum MoveType
    {
        MOVE,           // 並走
        RESCUE,         // 救助に向かう
        STOP,           // 到着

        MAX_MOVETYPE
    }
    public enum MoveSide
    {
        RIGHT,
        LEFT,
        NONE,

        MAX_MOVESIDE
    }
    private GameObject Player;
    private GameObject Rescue;
    private MoveType movetype;
    private MoveSide moveside;
    [SerializeField, Header("プレイヤーからの距離")]
    private Vector3 offset = new Vector3(0f, 0f, 0f);
    private Vector3 vec = new Vector3(0f, 0f, 0f);
    [SerializeField, Header("発射時のスピード")]
    private float speed = 0f;
    // =============== Start ================
    void Start()
    {
        Player = GameObject.Find("torpedo");
        movetype = MoveType.MOVE;
    }

    // =============== Update ===============
    void Update()
    {
        switch(movetype)
        {
            case MoveType.MOVE:
                transform.rotation = Player.transform.rotation;
                if(moveside == MoveSide.LEFT)
                {
                    transform.position = Player.transform.position + Player.transform.forward * offset.z +
                        Player.transform.up * offset.x;
                }
                else if (moveside == MoveSide.RIGHT)
                {
                    transform.position = Player.transform.position - Player.transform.forward * offset.z +
                        Player.transform.up * offset.x;
                }
                break;
            case MoveType.RESCUE:
                vec = (Rescue.transform.position - new Vector3(0.0f, 1.0f, 0.0f)) - transform.position;
                if(Mathf.Abs(vec.magnitude) < 0.1f)
                {
                    movetype = MoveType.STOP;
                }
                vec = vec.normalized;
                transform.position += vec * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(vec);
                transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
                break;
            case MoveType.STOP:
                break;
            default:
                break;
        }
    }

    // =============== 左右どちらにつくか ==================
    public void SetSide(MoveSide side)
    {
        moveside = side;
    }

    // =============== 左右どちらについているか ==================
    public MoveSide GetSide()
    {
        return moveside;
    }

    // =============== 発射 ================
    public void Shoot(GameObject Target)
    {
        Rescue = Target;
        movetype = MoveType.RESCUE;
        moveside = MoveSide.NONE;
    }
}
