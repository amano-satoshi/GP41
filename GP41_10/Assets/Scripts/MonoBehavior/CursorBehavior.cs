using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{
    [SerializeField, Header("カーソル移動速度")]
    private float CursorSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 0.2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
