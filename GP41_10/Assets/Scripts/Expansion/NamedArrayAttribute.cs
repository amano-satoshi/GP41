//------------------------------------------------------------------------
//
// (C) Copyright 2017 Urahimono Project Inc.
// 改変 : Satoshi Amano
//
//------------------------------------------------------------------------
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class CustomListLabelAttribute : PropertyAttribute
{
    public CustomListLabelAttribute(string[] i_prefix)
    {
        Prefix = i_prefix;
    }

    public string[] Prefix
    {
        get;
        private set;
    }
} // class CustomListLabelAttribute

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(CustomListLabelAttribute))]
public class CustomListLabelDrawer : PropertyDrawer
{
    public override void OnGUI(Rect i_position, SerializedProperty i_property, GUIContent i_label)
    {
        int index = GetElementIndex(i_property);
        if (index >= 0)
        {
            i_label.text = string.Format("{0}", attribute.Prefix[index]);
        }

        EditorGUI.PropertyField(i_position, i_property, i_label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }

    private new CustomListLabelAttribute attribute
    {
        get { return (CustomListLabelAttribute)base.attribute; }
    }

    private int GetElementIndex(SerializedProperty i_property)
    {
        string propertyPath = i_property.propertyPath;

        var propertys = propertyPath.Split('.');

        // このデータが配列内の要素ならば、「aaaa.Array.data[...]」の形になるはずだ！
        if (propertys.Length < 3)
        {
            return -1;
        }

        // クラスを経由して、パスが長くなった場合でも、このデータが配列内の要素ならば、その後ろから二番目は「Array」になるはずだ！
        string arrayProperty = propertys[propertys.Length - 2];
        if (arrayProperty != "Array")
        {
            return -1;
        }

        // このデータが配列内の要素ならば、data[...]の形になっているはずだ！
        var paths = propertyPath.Split('.');
        var lastPath = paths[propertys.Length - 1];
        if (!lastPath.StartsWith("data["))
        {
            return -1;
        }

        // 数字の要素だけ抜き出すんだ！
        var regex = new System.Text.RegularExpressions.Regex(@"[^0-9]");
        var countText = regex.Replace(lastPath, "");
        int index = 0;
        if (!int.TryParse(countText, out index))
        {
            return -1;
        }

        return index;
    }

} // class CustomListLabelDrawer

#endif // UNITY_EDITOR