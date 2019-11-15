using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DrawLine
{
    public static void Draw(Vector3 start, Vector3 end, float size, Color color)
    {
        GameObject newLine = new GameObject("Line");
        LineRenderer lRend = newLine.AddComponent<LineRenderer>();
        lRend.startWidth = size;
        lRend.endWidth = size;
        lRend.material.color = color;
        lRend.SetPosition(0, start);
        lRend.SetPosition(1, end);
    }
}
