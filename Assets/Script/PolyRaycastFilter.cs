using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// 用于检测不规则图形的射线检测方法，由于需要进行多边形的射线检测，
    /// 效率相对较低，所以，确认自己必须进行不规则射线检测时，才使用
    /// 该组件。
    /// </summary>
    [RequireComponent(typeof(UIPolyImage))]
    public class PolyRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
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

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (image.sprite == null)
                return false;

            //首先转换到本地坐标系中，方便下面的计算
            Vector2 localP = rectTransform.InverseTransformPoint(sp);

            Rect pixelRect = image.GetDrawingRect();
            if (!pixelRect.Contains(localP, false))
                return false;

            Vector2 delta = localP - pixelRect.min;
            Vector2 normalizedDelta = new Vector2(delta.x / pixelRect.width, delta.y / pixelRect.height);
            int x = Mathf.CeilToInt(normalizedDelta.x * image.sprite.rect.width+image.sprite.rect.xMin);
            int y = Mathf.CeilToInt(normalizedDelta.y * image.sprite.rect.height+image.sprite.rect.yMin);

            Color pixel = image.sprite.texture.GetPixel(x, y);

            return !Mathf.Approximately(pixel.a, 0f);
        }
    }
}
