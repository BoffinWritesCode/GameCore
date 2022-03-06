using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameCore.Maths
{
    public struct RectangleF
    {
        public float X;         
        public float Y;
        public float Width;
        public float Height;

        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;

        public Vector2 Center
        {
            get => new Vector2(X + Width / 2f, Y + Height / 2f);
            set { X = value.X - Width / 2f; Y = value.Y - Height / 2f; }
        }
        
        public Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }

        public Vector2 TopLeft
        {
            get => new Vector2(X, Y);
            set { X = value.X; Y = value.Y; }
        }

        public Vector2 TopRight
        {
            get => new Vector2(Right, Y);
            set { X = value.X - Width; Y = value.Y; }
        }

        public Vector2 BottomLeft
        {
            get => new Vector2(X, Bottom);
            set { X = value.X; Y = value.Y - Height; }
        }

        public Vector2 BottomRight
        {
            get => new Vector2(Right, Bottom);
            set { X = value.X - Width; Y = value.Y - Height; }
        }

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public RectangleF(Vector2 position, Vector2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        public RectangleF(Rectangle rect)
        {
            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }

        public RectangleF RoundToInt()
        {
            return new RectangleF((int)X, (int)Y, (int)Width, (int)Height);
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

        public bool Intersects(RectangleF rect)
        {
            return Left <= rect.Right &&
                rect.Left <= Right &&
                Top <= rect.Bottom &&
                rect.Top <= Bottom;
        }

        public bool IntersectsLineSegment(Vector2 lineStart, Vector2 lineEnd)
        {
            if (Contains(lineStart) || Contains(lineEnd)) return true;

            return MathExtras.LineSegmentsIntersect(lineStart, lineEnd, TopLeft, BottomLeft) ||
                MathExtras.LineSegmentsIntersect(lineStart, lineEnd, TopLeft, TopRight) ||
                MathExtras.LineSegmentsIntersect(lineStart, lineEnd, TopRight, BottomRight) ||
                MathExtras.LineSegmentsIntersect(lineStart, lineEnd, BottomLeft, BottomRight);
        }

        public bool Contains(Vector2 position)
        {
            return position.X >= Left && position.X <= Right && position.Y >= Top && position.Y <= Bottom;
        }

        public RectangleF Scale(float scale) => Scale(new Vector2(scale));

        public RectangleF Scale(Vector2 scale)
        {
            Vector2 extent = Size * 0.5f * scale;
            Vector2 center = Center;
            return FromEdges(center.X - extent.X, center.Y - extent.Y, center.X + extent.X, center.Y + extent.Y);
        }

        public RectangleF Offset(Vector2 amount)
        {
            return new RectangleF(Left + amount.X, Top + amount.Y, Width, Height);
        }

        public RectangleF Expand(Vector2 amount, bool fromCenter = false)
        {
            if (fromCenter) return new RectangleF(Left - amount.X * 0.5f, Top - amount.Y * 0.5f, Width + amount.X, Height + amount.Y);
            return new RectangleF(Left, Top, Width + amount.X, Height + amount.Y);
        }

        public static RectangleF Intersection(RectangleF value1, RectangleF value2)
        {
            if (value1.Intersects(value2))
            {
                float right_side = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
                float left_side = Math.Max(value1.X, value2.X);
                float top_side = Math.Max(value1.Y, value2.Y);
                float bottom_side = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
                return new RectangleF(left_side, top_side, right_side - left_side, bottom_side - top_side);
            }
            else
            {
                return new RectangleF(0, 0, 0, 0);
            }
        }

        public static RectangleF FromEdges(float left, float top, float right, float bottom)
        {
            return new RectangleF(left, top, right - left, bottom - top);
        }

        public static RectangleF FromEdges(Vector2 topLeft, Vector2 bottomRight) => FromEdges(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);

        public override bool Equals(object obj)
        {
            if (!(obj is RectangleF rect)) return false;

            return rect == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"X: {X} Y: {Y} Width: {Width} Height: {Height}";
        }

        public static bool operator ==(RectangleF r1, RectangleF r2)
        {
            return r1.X == r2.X && r1.Y == r2.Y && r1.Width == r2.Width && r1.Height == r2.Height;
        }

        public static bool operator !=(RectangleF r1, RectangleF r2)
        {
            return !(r1 == r2);
        }
    }
}