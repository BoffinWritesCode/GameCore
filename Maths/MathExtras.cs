using System;
using System.Collections.Generic;
using GameCore.Maths;
using Microsoft.Xna.Framework;

namespace GameCore
{
    public static class MathExtras
    {
        public static RectangleF ScaleToFitInto(RectangleF inner, RectangleF outer)
        {
            float resizeFactor = ScaleToFitIntoFactor(inner, outer);

            float newWidth = inner.Width * resizeFactor;
            float newHeight = inner.Height * resizeFactor;

            return new RectangleF(inner.X, inner.Y, newWidth, newHeight);
        }

        public static float ScaleToFitIntoFactor(RectangleF inner, RectangleF outer)
        {
            float innerRatio = inner.Width / (float) inner.Height;
            float outerRatio = outer.Width / (float) outer.Height;

            return (innerRatio >= outerRatio) ? (outer.Width / (float) inner.Width) : (outer.Height / (float) inner.Height);
        }

        public static Vector2 ClosestPointOnLineToPoint(Vector2 point, Vector2 lineStart, Vector2 lineEnd, bool isLineInfinite = false)
        {
            Vector2 delta = Vector2.Normalize(lineEnd - lineStart);
            Vector2 pointToStart = point - lineStart;
            float dot = Vector2.Dot(pointToStart, delta);
            Vector2 returnPoint = lineStart + delta * dot;

            if (!isLineInfinite)
            {
                //clamp the return point so that it cannot be a point that doesn't exist on the line defined by start and end points.
                returnPoint.X = MathHelper.Clamp(returnPoint.X, Math.Min(lineStart.X, lineEnd.X), Math.Max(lineStart.X, lineEnd.X));
                returnPoint.Y = MathHelper.Clamp(returnPoint.Y, Math.Min(lineStart.Y, lineEnd.Y), Math.Max(lineStart.Y, lineEnd.Y));
            }

            return returnPoint;
        }
    }
}