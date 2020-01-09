///////////////////////////////////////////////////////////////
//
// コンボ表示(ComboText : MonoBehaviour)
// Author : Satoshi Amano
// 作成日 : 2019/11/25
//
///////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    public Image[] images = new Image[3];                                    // Image コンポーネント

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 2コンボ
    public void TwoCombo()
    {
        images[0].enabled = true;
    }

    // 3コンボ
    public void ThreeCombo()
    {
        images[1].enabled = true;
    }

    // 4コンボ
    public void FourCombo()
    {
        images[2].enabled = true;
    }
}
