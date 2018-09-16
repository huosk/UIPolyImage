using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class UIPolyImage : Image
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (overrideSprite == null)
            {
                base.OnPopulateMesh(vh);
                return;
            }

            switch (type)
            {
                case Type.Simple:
                case Type.Filled:
                case Type.Sliced:
                case Type.Tiled:
                    GenerateSimpleSprite(vh, preserveAspect);
                    break;
            }
        }

        void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
        {
            Vector4 v = GetDrawingDimensions(lPreserveAspect);
            var color32 = color;

            float width = v.z - v.x;
            float height = v.w - v.y;

            //将sprite.pivot进行归一化
            Vector2 spritePivot = new Vector2(sprite.pivot.x / sprite.rect.width, sprite.pivot.y / sprite.rect.height);
            Vector2 pivotOffset = Vector2.Scale(spritePivot - rectTransform.pivot, new Vector2(width, height));

            Vector3 scale = new Vector3(1, 1, 1);
            scale.x = width / sprite.bounds.size.x;
            scale.y = height / sprite.bounds.size.y;
            Matrix4x4 trsMT = Matrix4x4.TRS(pivotOffset, Quaternion.identity, scale);

            vh.Clear();
            for (int i = 0; i < sprite.vertices.Length; i++)
            {
                vh.AddVert(trsMT.MultiplyPoint3x4(sprite.vertices[i]), color32, sprite.uv[i]);
            }

            for (int i = 0; i < sprite.triangles.Length; i += 3)
            {
                vh.AddTriangle(sprite.triangles[i], sprite.triangles[i + 1], sprite.triangles[i + 2]);
            }
        }

        public Rect GetDrawingRect()
        {
            Vector4 v = GetDrawingDimensions(preserveAspect);
            return Rect.MinMaxRect(v.x, v.y, v.z, v.w);
        }

        /// Image's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            var padding = overrideSprite == null ? Vector4.zero : UnityEngine.Sprites.DataUtility.GetPadding(overrideSprite);
            var size = overrideSprite == null ? Vector2.zero : new Vector2(overrideSprite.rect.width, overrideSprite.rect.height);

            Rect r = GetPixelAdjustedRect();
            // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            int spriteW = Mathf.RoundToInt(size.x);
            int spriteH = Mathf.RoundToInt(size.y);

            var v = new Vector4(
                    padding.x / spriteW,
                    padding.y / spriteH,
                    (spriteW - padding.z) / spriteW,
                    (spriteH - padding.w) / spriteH);

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f)
            {
                var spriteRatio = size.x / size.y;
                var rectRatio = r.width / r.height;

                if (spriteRatio > rectRatio)
                {
                    var oldHeight = r.height;
                    r.height = r.width * (1.0f / spriteRatio);
                    r.y += (oldHeight - r.height) * rectTransform.pivot.y;
                }
                else
                {
                    var oldWidth = r.width;
                    r.width = r.height * spriteRatio;
                    r.x += (oldWidth - r.width) * rectTransform.pivot.x;
                }
            }

            v = new Vector4(
                    r.x + r.width * v.x,
                    r.y + r.height * v.y,
                    r.x + r.width * v.z,
                    r.y + r.height * v.w
                    );

            return v;
        }
    }
}