using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    private Text uiText;                                    // UIText コンポーネント

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
        // Textコンポーネント取得
        uiText = GetComponent<Text>();
        uiText.text = "２連鎖！";
    }

    public void ThreeCombo()
    {
        // Textコンポーネント取得
        uiText = GetComponent<Text>();
        uiText.text = "３連鎖！";
    }

    public void FourCombo()
    {
        // Textコンポーネント取得
        uiText = GetComponent<Text>();
        uiText.text = "４連鎖！";
    }
}
