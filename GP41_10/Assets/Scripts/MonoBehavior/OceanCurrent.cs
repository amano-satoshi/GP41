using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanCurrent : MonoBehaviour
{
    private Vector3 OceanCurrentVec = new Vector3(0f, 0f, 0f);
    private float OceanCurrentSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOceanCurrentVec(Vector3 vec)
    {
        OceanCurrentVec = vec;
    }

    public Vector3 GetOceanCurrentVec()
    {
        return OceanCurrentVec;
    }

    public void SetOceanCurrentSpeed(float spd)
    {
        OceanCurrentSpeed = spd;
    }

    public float GetOceanCurrentSpeed()
    {
        return OceanCurrentSpeed;
    }
}
