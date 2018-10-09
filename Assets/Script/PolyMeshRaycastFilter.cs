using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIPolyImage))]
public class PolyMeshRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    private UIPolyImage image
    {
        get
        {
            if (m_Image == null)
                m_Image = GetComponent<UIPolyImage>();
            return m_Image;
        }
    }

    private RectTransform rectTransform
    {
        get
        {
            if (m_RectTransform == null)
                m_RectTransform = GetComponent<RectTransform>();
            return m_RectTransform;
        }
    }

    private UIPolyImage m_Image;
    private RectTransform m_RectTransform;

    // Detail: http://alienryderflex.com/polygon/
    bool IsPointInPoly(List<Vector2> verts, Vector2 point)
    {
        int i, j = verts.Count - 1;
        bool oddNodes = false;
        float x = point.x;
        float y = point.y;

        for (i = 0; i < verts.Count; i++)
        {
            if ((verts[i].y < y && verts[j].y >= y || verts[j].y < y && verts[i].y >= y) &&
                (verts[i].x <= x || verts[j].x <= x))
            {
                oddNodes ^= (verts[i].x + (y - verts[i].y) / (verts[j].y - verts[i].y) * (verts[j].x - verts[i].x) < x);
            }
            j = i;
        }

        return oddNodes;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (image == null || image.poly == null)
            return false;

        Vector2 localP = rectTransform.InverseTransformPoint(sp);
        if (!image.meshBounds.Contains(localP))
            return false;

        return IsPointInPoly(image.poly, localP);
    }
}
