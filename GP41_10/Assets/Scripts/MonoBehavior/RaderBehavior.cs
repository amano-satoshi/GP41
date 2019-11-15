using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaderBehavior : MonoBehaviour
{
    [SerializeField, Header("レーダーの移動速度")]
    private Vector3 RaderSpeed = new Vector3(0f, 0f, 0f);
    [SerializeField, Header("レーダーのスタート地点")]
    private Vector3 RaderStart = new Vector3(0f, 0f, 0f);
    [SerializeField, Header("レーダーのゴール地点")]
    private Vector3 RaderGoal = new Vector3(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        transform.position = RaderStart;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= RaderSpeed * Time.deltaTime;
        if(transform.position.x < RaderGoal.x)
        {
            transform.position = RaderStart;
        }
    }
}
