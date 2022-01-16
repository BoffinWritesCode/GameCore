using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore.UI
{
    public abstract class AxisAnchoredConstraint : IConstraint
    {
        protected float _axisAnchor;

        public AxisAnchoredConstraint(float anchor = 0.5f) => _axisAnchor = anchor;

        public virtual void ApplyConstraint(ref RectangleF current, RectangleF parent) => throw new NotImplementedException();
    }

    public class MaxWidth : AxisAnchoredConstraint
    {
        private float _maxWidth;

        public MaxWidth(float width, float anchor = 0.5f) : base(anchor) => _maxWidth = width;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            if (current.Width <= _maxWidth) return;

            float leftPush = _axisAnchor;
            float rightPush = 1f - _axisAnchor;
            float amountToPush = current.Width - _maxWidth;
            current = RectangleF.FromEdges(current.Left + amountToPush * leftPush, current.Top, current.Right - amountToPush * rightPush, current.Bottom);
        }
    }

    public class MaxHeight : AxisAnchoredConstraint
    {
        private float _maxHeight;

        public MaxHeight(float height, float anchor = 0.5f) : base(anchor) => _maxHeight = height;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            if (current.Height <= _maxHeight) return;

            float topPush = _axisAnchor;
            float bottomPush = 1f - _axisAnchor;
            float amountToPush = current.Height - _maxHeight;
            current = RectangleF.FromEdges(current.Left, current.Top + amountToPush * topPush, current.Right, current.Bottom - amountToPush * bottomPush);
        }
    }

    public class RelativeMaxWidth : AxisAnchoredConstraint
    {
        private float _maxWidthPercentage;

        public RelativeMaxWidth(float widthPercentage, float anchor = 0.5f) : base(anchor) => _maxWidthPercentage = widthPercentage;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            if (current.Width <= _maxWidthPercentage) return;

            float leftPush = _axisAnchor;
            float rightPush = 1f - _axisAnchor;
            float maxInPixels = parent.Width * _maxWidthPercentage;
            float amountToPush = current.Width - maxInPixels;
            current = RectangleF.FromEdges(current.Left + amountToPush * leftPush, current.Top, current.Right - amountToPush * rightPush, current.Bottom);
        }
    }

    public class RelativeMaxHeight : AxisAnchoredConstraint
    {
        private float _maxHeightPercentage;

        public RelativeMaxHeight(float heightPercentage, float anchor = 0.5f) : base(anchor) => _maxHeightPercentage = heightPercentage;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            if (current.Height <= _maxHeightPercentage) return;

            float topPush = _axisAnchor;
            float bottomPush = 1f - _axisAnchor;
            float maxInPixels = parent.Height * _maxHeightPercentage;
            float amountToPush = current.Height - maxInPixels;
            current = RectangleF.FromEdges(current.Left, current.Top + amountToPush * topPush, current.Right, current.Bottom - amountToPush * bottomPush);
        }
    }

    public class MinWidth : AxisAnchoredConstraint
    {
        private float _minWidth;

        public MinWidth(float width, float anchor = 0.5f) : base(anchor) => _minWidth = width;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            if (current.Width >= _minWidth) return;

            float leftPush = _axisAnchor;
            float rightPush = 1f - _axisAnchor;
            float amountToPush = _minWidth - current.Width;
            current = RectangleF.FromEdges(current.Left - amountToPush * leftPush, current.Top, current.Right + amountToPush * rightPush, current.Bottom);
        }
    }

    public class MinHeight : AxisAnchoredConstraint
    {
        private float _minHeight;

        public MinHeight(float height, float anchor = 0.5f) : base(anchor) => _minHeight = height;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            if (current.Height >= _minHeight) return;

            float topPush = _axisAnchor;
            float bottomPush = 1f - _axisAnchor;
            float amountToPush = _minHeight - current.Height;
            current = RectangleF.FromEdges(current.Left, current.Top - amountToPush * topPush, current.Right, current.Bottom + amountToPush * bottomPush);
        }
    }

    public class FloorToMultipleWidth : AxisAnchoredConstraint
    {
        private float _widthMultiple;

        public FloorToMultipleWidth(float widthMultiple, float anchor = 0.5f) : base(anchor) => _widthMultiple = widthMultiple;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            float divided = MathF.Floor(current.Width / _widthMultiple);
            float desiredWidth = divided * _widthMultiple;
            float leftPush = _axisAnchor;
            float rightPush = 1f - _axisAnchor;
            float amountToPush = current.Width - desiredWidth;
            current = RectangleF.FromEdges(current.Left + amountToPush * leftPush, current.Top, current.Right - amountToPush * rightPush, current.Bottom);
        }
    }

    public class FloorToMultipleHeight : AxisAnchoredConstraint
    {
        private float _heightMultiple;

        public FloorToMultipleHeight(float heightMultiple, float anchor = 0.5f) : base(anchor) => _heightMultiple = heightMultiple;

        public override void ApplyConstraint(ref RectangleF current, RectangleF parent)
        {
            float divided = MathF.Floor(current.Height / _heightMultiple);
            float desiredHeight = divided * _heightMultiple;
            float topPush = _axisAnchor;
            float bottomPush = 1f - _axisAnchor;
            float amountToPush = current.Height - desiredHeight;
            current = RectangleF.FromEdges(current.Left, current.Top + amountToPush * topPush, current.Right, current.Bottom - amountToPush * bottomPush);
        }
    }
}