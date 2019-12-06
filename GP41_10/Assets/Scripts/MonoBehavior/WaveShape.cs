using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveShape : MonoBehaviour
{
    public GameObject stageSpawner;
    private StageSpawner stageSpawnerObj;
    [SerializeField, Header("波のスプライト")]
    private Sprite[] waveImages = new Sprite[12];
    [SerializeField, Header("波形イメージ")]
    private Image waveShape;
    [SerializeField, Header("アニメーションスピード")]
    private float AnimationSpeed = 0.1f;
    private float SpriteChange;
    private int currentWavenum = 0;
    // Start is called before the first frame update
    void Start()
    {
        stageSpawnerObj = stageSpawner.GetComponent<StageSpawner>();
        SpriteChange = AnimationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        WaveAnimation();
    }

    void WaveAnimation()
    {
        SpriteChange -= Time.deltaTime;
        float currentSpeed = stageSpawnerObj.GetOceanCurrentSpeed();
        float[] SeaAreaSpeed = stageSpawnerObj.GetSeaAreaSpeed();
        if (SpriteChange <= 0f)
        {
            if (currentSpeed == SeaAreaSpeed[0])
            {
                if(currentWavenum > 3)
                {
                    currentWavenum = 0;
                    waveShape.sprite = waveImages[currentWavenum];
                }
                else
                {
                    currentWavenum++;
                    if(currentWavenum > 3)
                    {
                        currentWavenum = 0;
                        waveShape.sprite = waveImages[currentWavenum];
                    }
                    else
                    {
                        waveShape.sprite = waveImages[currentWavenum];
                    }
                }
            }
            else if (currentSpeed == SeaAreaSpeed[1])
            {
                if (currentWavenum > 7 || currentWavenum < 4)
                {
                    currentWavenum = 4;
                    waveShape.sprite = waveImages[currentWavenum];
                }
                else
                {
                    currentWavenum++;
                    if (currentWavenum > 7)
                    {
                        currentWavenum = 4;
                        waveShape.sprite = waveImages[currentWavenum];
                    }
                    else
                    {
                        waveShape.sprite = waveImages[currentWavenum];
                    }
                }
            }
            else if (currentSpeed == SeaAreaSpeed[2])
            {
                if (currentWavenum < 8)
                {
                    currentWavenum = 8;
                    waveShape.sprite = waveImages[currentWavenum];
                }
                else
                {
                    currentWavenum++;
                    if (currentWavenum > 11)
                    {
                        currentWavenum = 8;
                        waveShape.sprite = waveImages[currentWavenum];
                    }
                    else
                    {
                        waveShape.sprite = waveImages[currentWavenum];
                    }
                }
            }
            SpriteChange = AnimationSpeed;
        }
        
    }
}
