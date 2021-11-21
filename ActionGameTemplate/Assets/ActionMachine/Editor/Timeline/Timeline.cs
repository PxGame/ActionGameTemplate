/*
 * 作者：Peter Xiang
 * 联系方式：565067150@qq.com
 * 文档: https://github.com/PxGame
 * 创建时间: 2021/11/26 1:34:36
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using System;

namespace XMLib.AM
{
    /// <summary>
    /// TimelineElement
    /// </summary>
    public class Timeline : VisualElement
    {
        private SerializedProperty _property;

        private List<TrackItem> _trackItems = new List<TrackItem>();

        private VisualElement _toolbarTopContainer;
        private VisualElement _trackHeaderContainer;
        private VisualElement _trackHeaderContent;
        private VisualElement _toolbarBottomContainer;
        private VisualElement _tickContainer;
        private VisualElement _trackBodyContainer;
        private VisualElement _trackBodyContent;
        private Scroller _verticalScroller;
        private Scroller _horizontalScroller;

        private float trackHeight => 30f;
        private float trackbodyWidth => 600;

        private float scrollableWidth => _trackBodyContent.layout.width - _trackBodyContainer.layout.width;

        private float scrollableHeight => _trackBodyContent.layout.height - _trackBodyContainer.layout.height;

        private Vector2 scrollOffset
        {
            get => new Vector2(_horizontalScroller.value, _verticalScroller.value);
        }

        public Timeline()
        {
            var uxml = ResourceUtility.LoadUXML("Timeline/Timeline");
            uxml.CloneTree(this);

            _toolbarTopContainer = this.Q<VisualElement>("toolbar-top-container");
            _toolbarBottomContainer = this.Q<VisualElement>("toolbar-bottom-container");

            _tickContainer = this.Q<VisualElement>("tick-container");

            _trackHeaderContainer = this.Q<VisualElement>("track-header-container");
            _trackHeaderContent = this.Q<VisualElement>("track-header-content");

            _trackBodyContainer = this.Q<VisualElement>("track-body-container");
            _trackBodyContent = this.Q<VisualElement>("track-body-content");

            _verticalScroller = this.Q<Scroller>("vertical-scroller");
            _horizontalScroller = this.Q<Scroller>("horizontal-scroller");

            _trackBodyContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

            this.RegisterCallback<WheelEvent>(OnScrollWheel);

            _verticalScroller.valueChanged += (y) => UpdateContentTransform();
            _horizontalScroller.valueChanged += (x) => UpdateContentTransform();

            Refresh();
        }

        private void OnScrollWheel(WheelEvent evt)
        {
            var scroller = evt.altKey ? _horizontalScroller : _verticalScroller;
            var scrollableValue = evt.altKey ? scrollableWidth : scrollableHeight;

            float value = scroller.value;
            if (scrollableValue > 0f)
            {
                if (evt.delta.y < 0f)
                {
                    scroller.ScrollPageUp(Mathf.Abs(evt.delta.y));
                }
                else if (evt.delta.y > 0f)
                {
                    scroller.ScrollPageDown(Mathf.Abs(evt.delta.y));
                }
            }
            if (scroller.value != value)
            {
                evt.StopPropagation();
            }
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            if (evt.oldRect.size == evt.newRect.size) { return; }

            UpdateScroller();
            UpdateContentTransform();
        }

        /// <summary>
        /// 指向的值必须为 TrackData 的集合
        /// </summary>
        /// <param name="property"></param>
        public void Inspect(SerializedProperty property)
        {
            if (property != null && (!property.isArray || !string.Equals(property.arrayElementType, nameof(TrackData))))
            {
                throw new Exception("传入的 property 必须为 TrackData 的数组");
            }

            _property = property;

            Refresh();
        }

        private void Refresh()
        {
            UpdateTracks();
            UpdateContentRect();
            UpdateScroller();
            UpdateContentTransform();

            this.IncrementVersion(VersionChangeType.Repaint);
        }

        private void UpdateTracks()
        {
            foreach (var item in _trackItems)
            {
                _trackHeaderContent.Remove(item.header);
                _trackBodyContent.Remove(item.body);
            }
            _trackItems.Clear();
            if (_property != null)
            {
                for (int i = 0; i < _property.arraySize; i++)
                {
                    var track = _property.GetArrayElementAtIndex(i);
                    AppendTrack(track);
                }
            }
        }

        private void UpdateContentTransform()
        {
            Vector2 offset = scrollOffset;
            Vector2 bodyPos;

            bodyPos.x = EditorTool.RoundToPixelGrid(-offset.x);
            bodyPos.y = EditorTool.RoundToPixelGrid(-offset.y);

            Vector2 headerPos = new Vector2(0, bodyPos.y);

            _trackHeaderContent.transform.position = headerPos;
            _trackBodyContent.transform.position = bodyPos;
        }

        private void UpdateContentRect()
        {
            float height = _trackItems.Count * trackHeight;
            float width = trackbodyWidth;

            _trackHeaderContent.style.height = height;
            _trackBodyContent.style.height = height;
            _trackBodyContent.style.width = width;

            _trackHeaderContent.IncrementVersion(VersionChangeType.Repaint);
            _trackBodyContent.IncrementVersion(VersionChangeType.Repaint);
        }

        private void UpdateScroller()
        {
            float factor = ((_trackBodyContent.layout.width > 1E-30f) ? (_trackBodyContainer.layout.width / _trackBodyContent.layout.width) : 1f);
            float factor2 = ((_trackBodyContent.layout.height > 1E-30f) ? (_trackBodyContainer.layout.height / _trackBodyContent.layout.height) : 1f);
            _horizontalScroller.Adjust(factor);
            _verticalScroller.Adjust(factor2);

            _horizontalScroller.SetEnabled(scrollableWidth > 0f);
            _verticalScroller.SetEnabled(scrollableHeight > 0f);

            _horizontalScroller.lowValue = 0f;
            _verticalScroller.lowValue = 0f;

            _horizontalScroller.highValue = scrollableWidth > 0 ? scrollableWidth : 0f;
            _verticalScroller.highValue = scrollableHeight > 0 ? scrollableHeight : 0f;

            _horizontalScroller.value = Mathf.Clamp(_horizontalScroller.value, _horizontalScroller.lowValue, _horizontalScroller.highValue);
            _verticalScroller.value = Mathf.Clamp(_verticalScroller.value, _verticalScroller.lowValue, _verticalScroller.highValue);

            _verticalScroller.IncrementVersion(VersionChangeType.Repaint);
            _horizontalScroller.IncrementVersion(VersionChangeType.Repaint);
        }

        private TrackItem AppendTrack(SerializedProperty trackProperty)
        {
            TrackHeader header = new TrackHeader();
            header.style.flexShrink = 0;
            header.style.flexGrow = 0;
            header.style.height = trackHeight;
            header.Inspect(trackProperty);
            _trackHeaderContent.Add(header);

            TrackBody body = new TrackBody();
            body.style.flexShrink = 0;
            body.style.flexGrow = 0;
            body.style.height = trackHeight;
            body.Inspect(trackProperty);
            _trackBodyContent.Add(body);

            var track = new TrackItem()
            {
                property = trackProperty,
                header = header,
                body = body
            };

            _trackItems.Add(track);

            return track;
        }

        public new class UxmlFactory : UxmlFactory<Timeline, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }

        public class TrackItem
        {
            public SerializedProperty property;
            public TrackHeader header;
            public TrackBody body;
        }
    }
}