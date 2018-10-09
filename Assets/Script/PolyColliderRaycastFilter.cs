using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 基于 PolygonCollider2D 组件的射线检测，系统提供的 Physics2DRaycaster 虽然也能
/// 实现类似功能，但是会需要多个 Raycaster 来实现。
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
public class PolyColliderRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    PolygonCollider2D polyCollider
    {
        get
        {
            if (m_PolyCollider == null)
                m_PolyCollider = GetComponent<PolygonCollider2D>();
            return m_PolyCollider;
        }
    }

    private RectTransform rectTransform
    {
        get
        {
            if (m_RectTransform == null)
                m_RectTransform = transform as RectTransform;
            return m_RectTransform;
        }
    }

    private PolygonCollider2D m_PolyCollider;
    private RectTransform m_RectTransform;

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (polyCollider == null)
            return false;

        if (eventCamera != null)
        {
            //sp不包含 z坐标值，所以需要单独计算z值
            float zDistance2Cam = rectTransform.position.z - eventCamera.transform.position.z;
            Vector3 sp3 = new Vector3(sp.x, sp.y, zDistance2Cam);
            return polyCollider.OverlapPoint(eventCamera.ScreenToWorldPoint(sp3));
        }
        else
        {
            //Overlay 模式下，屏幕坐标xy = 世界坐标xy，z = 0
            return polyCollider.OverlapPoint(sp);
        }
    }
}
