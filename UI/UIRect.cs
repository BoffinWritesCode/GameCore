using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore.UI
{
    public class UIRect
    {
        public AnchorRect AnchorRect { get; set; }

        private List<IConstraint> _constraints;
        public List<IConstraint> Constraints
        {
            get 
            {
                if (_constraints == null) _constraints = new List<IConstraint>();
                return _constraints;
            }
            set
            {
                _constraints = value;
            }
        }

        public event Action OnChangeConstraints;

        public UIRect()
        {
            AnchorRect = new AnchorRect();
        }

        public RectangleF Calculate(RectangleF parent)
        {
            RectangleF current = RectangleF.FromEdges(
                parent.Left + parent.Width * AnchorRect.Left.AxisInterpolation + AnchorRect.Left.PixelOffset,
                parent.Top + parent.Height * AnchorRect.Top.AxisInterpolation + AnchorRect.Top.PixelOffset,
                parent.Left + parent.Width * AnchorRect.Right.AxisInterpolation + AnchorRect.Right.PixelOffset,
                parent.Top + parent.Height * AnchorRect.Bottom.AxisInterpolation + AnchorRect.Bottom.PixelOffset    
            );

            if (_constraints == null) return current;

            // apply constraints
            for (int i = 0; i < _constraints.Count; i++)
            {
                _constraints[i].ApplyConstraint(ref current, parent);
            }

            return current;
        }

        public UIRect AddConstraint(IConstraint constraint)
        {
            Constraints.Add(constraint);
            OnChangeConstraints?.Invoke();
            return this;
        }

        public UIRect RemoveConstraint(IConstraint constraint)
        {
            Constraints.Remove(constraint);
            OnChangeConstraints?.Invoke();
            return this;
        }

        public UIRect FillParent(float leftPadding = 0f, float topPadding = 0f, float rightPadding = 0f, float bottomPadding = 0f)
        {
            AnchorRect.FillParent(leftPadding, topPadding, rightPadding, bottomPadding);
            return this;
        }
        public UIRect AnchorTopLeft(float width, float height, float leftPadding = 0f, float topPadding = 0f)
        {
            AnchorRect.AnchorTopLeft(width,  height, leftPadding, topPadding);
            return this;
        }
        public UIRect AnchorTopMiddle(float width, float height, float topPadding = 0f)
        {
            AnchorRect.AnchorTopMiddle(width, height, topPadding);
            return this;
        }
        public UIRect AnchorTopRight(float width, float height, float rightPadding = 0f, float topPadding = 0f)
        {
            AnchorRect.AnchorTopRight(width, height, rightPadding, topPadding);
            return this;
        }
        public UIRect AnchorMiddleLeft(float width, float height, float leftPadding = 0f)
        {
            AnchorRect.AnchorMiddleLeft(width, height, leftPadding);
            return this;
        }
        public UIRect AnchorMiddleRight(float width, float height, float rightPadding = 0f)
        {
            AnchorRect.AnchorMiddleRight(width, height, rightPadding);
            return this;
        }
        public UIRect AnchorMiddle(float width, float height, float xOffset = 0f, float yOffset = 0f)
        {
            AnchorRect.AnchorMiddle(width, height, xOffset, yOffset);
            return this;
        }
        public UIRect AnchorBottomLeft(float width, float height, float leftPadding = 0f, float bottomPadding = 0f)
        {
            AnchorRect.AnchorBottomLeft(width, height, leftPadding, bottomPadding);
            return this;
        }
        public UIRect AnchorBottomMiddle(float width, float height, float bottomPadding = 0f)
        {
            AnchorRect.AnchorBottomMiddle(width, height, bottomPadding);
            return this;
        }
        public UIRect AnchorBottomRight(float width, float height, float rightPadding = 0f, float bottomPadding = 0f)
        {
            AnchorRect.AnchorBottomRight(width, height, rightPadding, bottomPadding);
            return this;
        }
        public UIRect AnchorTopStretchHorizontally(float height, float topPadding = 0f, float leftPadding = 0f, float rightPadding = 0f)
        {
            AnchorRect.AnchorTopStretchHorizontally(height, topPadding, leftPadding, rightPadding);
            return this;
        }
        public UIRect AnchorBottomStretchHorizontally(float height, float bottomPadding = 0f, float leftPadding = 0f, float rightPadding = 0f)
        {
            AnchorRect.AnchorBottomStretchHorizontally(height, bottomPadding, leftPadding, rightPadding);
            return this;
        }
        public UIRect AnchorLeftStretchVertically(float width, float leftPadding = 0f, float topPadding = 0f, float bottomPadding = 0f)
        {
            AnchorRect.AnchorLeftStretchVertically(width, leftPadding, topPadding, bottomPadding);
            return this;
        }
        public UIRect AnchorRightStretchVertically(float width, float rightPadding = 0f, float topPadding = 0f, float bottomPadding = 0f)
        {
            AnchorRect.AnchorRightStretchVertically(width, rightPadding, topPadding, bottomPadding);
            return this;
        }
    }
}