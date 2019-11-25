using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{
    [SerializeField, Header("カーソル移動速度")]
    private float CursorSpeed = 0f;
    [SerializeField, Header("カーソル移動制限")]
    private Vector2 LimitCursorPos;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 0.2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > LimitCursorPos.x)
        {
            transform.position = new Vector3(LimitCursorPos.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -LimitCursorPos.x)
        {
            transform.position = new Vector3(-LimitCursorPos.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.z > LimitCursorPos.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, LimitCursorPos.y);
        }
        else if (transform.position.z < -LimitCursorPos.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -LimitCursorPos.y);
        }
    }

    public void CursorMove(KeyCode Key)
    {
        if (Key == KeyCode.W)
        {
            transform.position += new Vector3(0f, 0f, CursorSpeed * Time.deltaTime);
        }
        if (Key == KeyCode.S)
        {
            transform.position -= new Vector3(0f, 0f, CursorSpeed * Time.deltaTime);
        }
        if (Key == KeyCode.A)
        {
            transform.position -= new Vector3(CursorSpeed * Time.deltaTime, 0f, 0f);
        }
        if (Key == KeyCode.D)
        {
            transform.position += new Vector3(CursorSpeed * Time.deltaTime, 0f, 0f);
        }
    }
}
