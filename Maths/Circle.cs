using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Maths
{
    public struct Circle
    {
        //properties
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; set; }

        public float Diameter
        {
            get => Radius * 2f;
            set => Radius = value * 0.5f;
        }
        public Vector2 Center
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        //constructors
        public Circle(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public Circle(Vector2 center, float radius) : this(center.X, center.Y, radius) { }

        //----------
        //INTERSECTS
        //----------
        public bool Intersects(Circle circle)
        {
            Vector2 between = circle.Center - this.Center;
            return between.Length() < circle.Radius + this.Radius;
        }

        public bool Intersects(Rectangle rectangle) => Intersects(new RectangleF(rectangle));

        public bool Intersects(RectangleF rectangle)
        {
            //get vector between closest rectangle point and circle center
            Vector2 vector = new Vector2(X - Math.Max(rectangle.Left, Math.Min(X, rectangle.Right)), Y - Math.Max(rectangle.Top, Math.Min(Y, rectangle.Bottom)));

            //leave the length squared, as otherwise it needs to do sqrt.
            float length = vector.LengthSquared();

            return length < Radius * Radius;
        }

        public bool Intersects(Rectangle rect, float radians)
        {
            Vector2 center = rect.Center.ToVector2();
            Vector2 thisRotated = Center.RotateMeAround(center, -radians);
            Circle rotated = new Circle(thisRotated, Radius);

            return rotated.Intersects(rect);
        }
    }
}
