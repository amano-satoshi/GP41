using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChkDigit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    * 整数の桁数を返す
    * */
    public static int CheckDigit(int num)
    {

        //0の場合1桁として返す
        if (num == 0) return 1;

        //対数とやらを使って返す
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);

    }

    /**
    * 数値の中から指定した桁の値をかえす
    * */
    public static int GetPointDigit(int num, int digit)
    {

        int res = 0;
        int pow_dig = (int)Mathf.Pow(10, digit);
        if (digit == 1)
        {
            res = num - (num / pow_dig) * pow_dig;
        }
        else
        {
            res = (num - (num / pow_dig) * pow_dig) / (int)Mathf.Pow(10, (digit - 1));
        }

        return res;
    }
}
