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

    public void TwoCombo()
    {
        images[0].enabled = true;
    }

    public void ThreeCombo()
    {
        images[1].enabled = true;
    }

    public void FourCombo()
    {
        images[2].enabled = true;
    }
}
