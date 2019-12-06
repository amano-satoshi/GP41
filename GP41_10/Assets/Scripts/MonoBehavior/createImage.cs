using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class createImage : Graphic
{
    [SerializeField, Header("初期ゲージ配置")]
    private Vector3[] vertpos = new Vector3[4];
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIVertex vert = UIVertex.simpleVert;
        vert.color = color;

        var uv = Vector4.zero;

        // left bottom
        vert.position = vertpos[0];
        vert.uv0 = new Vector2(uv.x, uv.y);
        vert.color = Color.red;
        vh.AddVert(vert);

        // left top
        vert.position = vertpos[1];
        vert.uv0 = new Vector2(uv.x, uv.w);
        vert.color = Color.blue;
        vh.AddVert(vert);

        // right top
        vert.position = vertpos[2];
        vert.uv0 = new Vector2(uv.z, uv.w);
        vert.color = Color.green;
        vh.AddVert(vert);

        // right bottom
        vert.position = vertpos[3];
        vert.uv0 = new Vector2(uv.z, uv.y);
        vert.color = Color.white;
        vh.AddVert(vert);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }

    public void SetVertPos(Vector3[] vertPos)
    {
        for(int i = 0; i < 4; ++i)
        {
            vertpos[i] = vertPos[i];
        }
        SetVerticesDirty();
    }
}
