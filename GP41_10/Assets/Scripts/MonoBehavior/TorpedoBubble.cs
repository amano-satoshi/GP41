using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoBubble : MonoBehaviour
{
    public GameObject TorpedoBubbleObj;
    private GameObject torpedoBubble;
    [SerializeField, Header("泡発射間隔")]
    private float bubbleTime = 0f;
    private float CurrentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime += Time.deltaTime;
        if(CurrentTime >= bubbleTime)
        {
            torpedoBubble = Instantiate(TorpedoBubbleObj, transform.position, transform.rotation);
            torpedoBubble.transform.localScale *= -1;
            CurrentTime = 0f;
        }
    }
}
