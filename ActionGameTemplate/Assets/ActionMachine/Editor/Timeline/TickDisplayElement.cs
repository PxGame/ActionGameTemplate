/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/12/6 23:21:27
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    /// <summary>
    /// TickDisplayElement
    /// </summary>
    public class TickDisplayElement : ImmediateModeElement
    {
        private IMGUIContainer imgui;

        private float step = 5;

        public TickDisplayElement()
        {
            imgui = new IMGUIContainer(OnGUIHandler);
            imgui.style.flexGrow = 1;
            imgui.style.flexShrink = 1;
            Add(imgui);
        }

        private void OnGUIHandler()
        {
            GUI.color = Color.black;
            int count = 0;
            GUIContent content = new GUIContent();
            for (float i = layout.xMin + 10; i < layout.xMax; i += step)
            {
                if (count % 10 == 0)
                {
                    content.text = (count / 10).ToString();
                    var size = GUI.skin.label.CalcSize(content);
                    var rt = Rect.MinMaxRect(i - size.x / 2, layout.yMax - 15 - size.y, i + size.x / 2, layout.yMax - 15);
                    GUI.Label(rt, content);
                }
                count++;
            }
        }

        protected override void ImmediateRepaint()
        {
            EditorTool.ApplyWireMaterial();
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            int count = 0;
            for (float i = layout.xMin + 10; i < layout.xMax; i += step)
            {
                count %= 10;

                GL.Vertex(new Vector2(i, layout.yMax));
                GL.Vertex(new Vector2(i, layout.yMax - (count == 0 ? 14 : (count == 5 ? 10 : 6))));

                count++;
            }
            GL.End();
        }

        public new class UxmlFactory : UxmlFactory<TickDisplayElement, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}