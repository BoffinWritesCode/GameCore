using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public class UISingleAxisLayout : UILayoutElement
    {
        private AxisLayoutMode _layoutMode;
        public AxisLayoutMode LayoutMode { get => _layoutMode; set { _layoutMode = value; UpdateChildLayouts(); } }

        /// <summary>
        /// The amount of pixel spacing between each child.
        /// </summary>
        public float Spacing { get; set; }

        /// <summary>
        /// The size of the children. Only applies when the layout mode is set to <see cref="AxisLayoutMode.SetChildSize"/>.
        /// </summary>
        public float ChildSize { get; set; }

        public Axis MyAxis { get; protected set; }

        public UISingleAxisLayout(Axis axis, AxisLayoutMode mode = AxisLayoutMode.StretchToFitAllInParentsSize) : base()
        {
            MyAxis = axis;

            _layoutMode = mode;   
        }

        protected void ChildrenControlTheirSizeUpdate(Axis axis, float spacing)
        {
            RectangleF rect = CalculateRect();
            float currentPos = axis == Axis.Horizontal ? Padding.Left : Padding.Top;
            for (int i = 0; i < Children.Count; i++)
            {
                if (!Children[i].Active) continue;

                // gets the size based on the child's rect and this as the parent.
                // the child will still have it's alternate axis sized changed, but their main axis size will stay as specified.
                RectangleF childAttemptedSize = Children[i].Rect.Calculate(rect);

                switch (axis)
                {
                    case Axis.Horizontal: Children[i].Rect.AnchorTopLeft(childAttemptedSize.Width, rect.Height - Padding.Top - Padding.Bottom, currentPos, Padding.Top); break;
                    case Axis.Vertical: Children[i].Rect.AnchorTopLeft(rect.Width - Padding.Left - Padding.Right, childAttemptedSize.Height, Padding.Left, currentPos); break;
                }

                currentPos += (axis == Axis.Horizontal ? childAttemptedSize.Width : childAttemptedSize.Height) + spacing;
            }
        }

        // TODO: Mixing MinSize and MaxSize constraints causes issues. Needed?
        protected void StretchToFitUpdate(Axis axis, float spacing)
        {
            RectangleF rect = CalculateRect();
            float spacingArea = (Children.Count - 1) * spacing;
            float available = (axis == Axis.Horizontal ? rect.Width - Padding.Left - Padding.Right : rect.Height - Padding.Top - Padding.Bottom) - spacingArea;
            float elementSize = available / Children.Count;
            
            // go through each element and set them to the element size.
            // if any elements modify their size due to constraints, adjust and re-do?
            float initialSize = axis == Axis.Horizontal ? Padding.Left : Padding.Top;
            float currentValue = initialSize;
            Dictionary<int, float> constrainedIndices = new Dictionary<int, float>();
            for (int i = 0; i < Children.Count; i++)
            {
                if (!Children[i].Active) continue;

                switch (axis)
                {
                    case Axis.Horizontal: Children[i].Rect.AnchorTopLeft(elementSize, rect.Height - Padding.Top - Padding.Bottom, currentValue, Padding.Top); break;
                    case Axis.Vertical: Children[i].Rect.AnchorTopLeft(rect.Width - Padding.Right - Padding.Left, elementSize, Padding.Left, currentValue); break;
                }
                
                // if this has already been constrained
                if (constrainedIndices.TryGetValue(i, out float size))
                {
                    switch (axis)
                    {
                        case Axis.Horizontal: Children[i].Rect.AnchorTopLeft(size, rect.Height - Padding.Top - Padding.Bottom, currentValue, Padding.Top); break;
                        case Axis.Vertical: Children[i].Rect.AnchorTopLeft(rect.Width - Padding.Right - Padding.Left, size, Padding.Left, currentValue); break;
                    }

                    if (i < Children.Count - 1) currentValue += size + spacing;

                    continue;
                }

                if (i < Children.Count - 1) currentValue += elementSize + spacing;

                // test size
                RectangleF testRect = Children[i].CalculateRect();
                float testSize = (axis == Axis.Horizontal ? testRect.Width : testRect.Height);
                if (testSize != elementSize)
                {
                    // add the new size this element should be taking up
                    constrainedIndices.Add(i, testSize);

                    // element had a constraint, grab the amount of size different it's taking up for it's constraint
                    // and adjust the element width
                    float delta = testSize - elementSize;
                    elementSize -= delta / (Children.Count - constrainedIndices.Count);

                    // start over
                    i = -1;
                    currentValue = initialSize;
                }
            }
            
            ChildArea = axis == Axis.Horizontal ? new Vector2(currentValue + Padding.Right, rect.Height) : new Vector2(rect.Width, currentValue + Padding.Bottom);
        }

        protected void SetSizeUpdate(Axis axis, float size, float spacing)
        {
            RectangleF rect = CalculateRect();
            float currentPos = axis == Axis.Horizontal ? Padding.Left : Padding.Top;
            foreach (UIElement child in Children)
            {
                if (!child.Active) continue;

                switch (axis)
                {
                    case Axis.Horizontal: child.Rect.AnchorTopLeft(size, rect.Height - Padding.Top - Padding.Bottom, currentPos, Padding.Top); break;
                    case Axis.Vertical: child.Rect.AnchorTopLeft(rect.Width - Padding.Left - Padding.Right, size, Padding.Left, currentPos); break;
                }

                // test size
                RectangleF testRect = child.CalculateRect();
                float testSize = (axis == Axis.Horizontal ? testRect.Width : testRect.Height);
                if (testSize != size)
                {
                    switch (axis)
                    {
                        case Axis.Horizontal: child.Rect.AnchorTopLeft(testSize, rect.Height - Padding.Top - Padding.Bottom, currentPos, Padding.Top); break;
                        case Axis.Vertical: child.Rect.AnchorTopLeft(rect.Width - Padding.Left - Padding.Right, testSize, Padding.Left, currentPos); break;
                    }
                    
                    currentPos += testSize + spacing;
                }
                else
                {
                    currentPos += size + spacing;
                }
            }

            ChildArea = axis == Axis.Horizontal ? new Vector2(currentPos - spacing + Padding.Right, rect.Height) : new Vector2(rect.Width, currentPos - spacing + Padding.Bottom);
        }

        public override void UpdateChildLayouts()
        {
            switch (_layoutMode)
            {
                case AxisLayoutMode.StretchToFitAllInParentsSize:
                    StretchToFitUpdate(MyAxis, Spacing);
                    break;
                case AxisLayoutMode.SetChildSize:
                    SetSizeUpdate(MyAxis, ChildSize, Spacing);
                    break;
                case AxisLayoutMode.ChildrenControlTheirSize:
                    ChildrenControlTheirSizeUpdate(MyAxis, Spacing);
                    break;
            }

            base.UpdateChildLayouts();
        }
    }
}