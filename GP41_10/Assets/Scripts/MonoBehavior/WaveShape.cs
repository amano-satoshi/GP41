using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveShape : MonoBehaviour
{
    public GameObject stageSpawner;
    private StageSpawner stageSpawnerObj;
    // Start is called before the first frame update
    void Start()
    {
        stageSpawnerObj = stageSpawner.GetComponent<StageSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        float width = stageSpawnerObj.GetOceanCurrentSpeed() / 10f;
        GetComponent<Image>().material.SetFloat("_SinWidth", width);
    }
}
