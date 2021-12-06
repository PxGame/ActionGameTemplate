// UnityEngine.UIElements.TwoPaneSplitViewResizer
using UnityEngine;
using UnityEngine.UIElements;

namespace XMLib.AM
{
    internal class TwoPaneSplitViewResizer : MouseManipulator
    {
        private Vector2 m_Start;

        protected bool m_Active;

        private TwoPaneSplitView m_SplitView;

        private int m_Direction;

        private TwoPaneSplitViewOrientation m_Orientation;

        private VisualElement fixedPane => m_SplitView.fixedPane;

        private VisualElement flexedPane => m_SplitView.flexedPane;

        private float fixedPaneMinDimension
        {
            get
            {
                if (m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
                {
                    return fixedPane.resolvedStyle.minWidth.value;
                }
                return fixedPane.resolvedStyle.minHeight.value;
            }
        }

        private float flexedPaneMinDimension
        {
            get
            {
                if (m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
                {
                    return flexedPane.resolvedStyle.minWidth.value;
                }
                return flexedPane.resolvedStyle.minHeight.value;
            }
        }

        public TwoPaneSplitViewResizer(TwoPaneSplitView splitView, int dir, TwoPaneSplitViewOrientation orientation)
        {
            m_Orientation = orientation;
            m_SplitView = splitView;
            m_Direction = dir;
            base.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.LeftMouse
            });
            m_Active = false;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            base.target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            base.target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            base.target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            base.target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            base.target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            base.target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        public void ApplyDelta(float delta)
        {
            float num = ((m_Orientation == TwoPaneSplitViewOrientation.Horizontal) ? fixedPane.resolvedStyle.width : fixedPane.resolvedStyle.height);
            float num2 = num + delta;
            if (num2 < num && num2 < fixedPaneMinDimension)
            {
                num2 = fixedPaneMinDimension;
            }
            float num3 = ((m_Orientation == TwoPaneSplitViewOrientation.Horizontal) ? m_SplitView.resolvedStyle.width : m_SplitView.resolvedStyle.height);
            num3 -= flexedPaneMinDimension;
            if (num2 > num && num2 > num3)
            {
                num2 = num3;
            }
            if (m_Orientation == TwoPaneSplitViewOrientation.Horizontal)
            {
                fixedPane.style.width = num2;
                if (m_SplitView.fixedPaneIndex == 0)
                {
                    base.target.style.left = num2;
                }
                else
                {
                    base.target.style.left = m_SplitView.resolvedStyle.width - num2;
                }
            }
            else
            {
                fixedPane.style.height = num2;
                if (m_SplitView.fixedPaneIndex == 0)
                {
                    base.target.style.top = num2;
                }
                else
                {
                    base.target.style.top = m_SplitView.resolvedStyle.height - num2;
                }
            }
        }

        protected void OnMouseDown(MouseDownEvent e)
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
            }
            else if (CanStartManipulation(e))
            {
                m_Start = e.localMousePosition;
                m_Active = true;
                base.target.CaptureMouse();
                e.StopPropagation();
            }
        }

        protected void OnMouseMove(MouseMoveEvent e)
        {
            if (m_Active && base.target.HasMouseCapture())
            {
                Vector2 vector = e.localMousePosition - m_Start;
                float num = vector.x;
                if (m_Orientation == TwoPaneSplitViewOrientation.Vertical)
                {
                    num = vector.y;
                }
                float delta = (float)m_Direction * num;
                ApplyDelta(delta);
                e.StopPropagation();
            }
        }

        protected void OnMouseUp(MouseUpEvent e)
        {
            if (m_Active && base.target.HasMouseCapture() && CanStopManipulation(e))
            {
                m_Active = false;
                base.target.ReleaseMouse();
                e.StopPropagation();
            }
        }
    }
}