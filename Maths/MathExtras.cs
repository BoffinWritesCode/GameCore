using System;
using System.Collections.Generic;
using GameCore.Maths;
using Microsoft.Xna.Framework;

namespace GameCore
{
    public static class MathExtras
    {
        /// <summary>
        /// Scales "inner" to fit into "outer" while maintaining it's ratio of width to height.
        /// </summary>
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

        /// <summary>
        /// Maps the specified value from it's original minimum and maximum to a new specifed minimum and maximum
        /// </summary>
        /// <param name="baseMin">The original minimum.</param>
        /// <param name="baseMax">The original maximum.</param>
        /// <param name="newMin">The new minimum.</param>
        /// <param name="newMax">The new maximum.</param>
        /// <param name="value">The value to re-map.</param>
        /// <returns>The re-mapped value.</returns>
        public static float Map(float baseMin, float baseMax, float newMin, float newMax, float value)
        {
            return newMin + (value - baseMin) * (newMax - newMin) / (baseMax - baseMin);
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

        //based on https://stackoverflow.com/questions/1073336/circle-line-segment-collision-detection-algorithm
        public static void GetIntersectionPointsLineCircle(Vector2 lineStart, Vector2 lineEnd, Vector2 circleCenter, float circleRadius, out Vector2[] points)
        {
            Vector2 line = lineEnd - lineStart;
            float LAB = line.Length();
            Vector2 D = new Vector2(line.X / LAB, line.Y / LAB);
            float t = D.X * (circleCenter.X - lineStart.X) + D.Y * (circleCenter.Y - lineStart.Y);
            Vector2 E = new Vector2(t * D.X + lineStart.X, t * D.Y + lineStart.X);
            Vector2 EC = E - circleCenter;
            float LEC = EC.Length();

            points = null;
            if (LEC < circleRadius)
            {
                float dt = (float)Math.Sqrt(circleRadius * circleRadius - LEC * LEC);

                points = new Vector2[2];
                points[0] = new Vector2((t - dt) * D.X + lineStart.X, (t - dt) * D.Y + lineStart.Y);
                points[1] = new Vector2((t + dt) * D.X + lineStart.X, (t + dt) * D.Y + lineStart.Y);
            }
            else if (LEC == circleRadius)
            {
                points = new Vector2[1];
                points[0] = E;
            }
        }

        public static bool LineSegmentsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2)
        {
            return LineSegmentsIntersect(p, p2, q, q2, out _);
        }

        // https://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
        public static bool LineSegmentsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2, out Vector2 intersection, 
            bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Vector2();

            Vector2 r = p2 - p;
            Vector2 s = q2 - q;
            float rxs = r.Cross(s);
            float qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero(1e-10f) && qpxr.IsZero(1e-10f))
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                    if ((Vector2.Dot((q - p), r) >= 0 && Vector2.Dot((q - p), r) <= Vector2.Dot(r, r)) || (Vector2.Dot((p - q), s) >= 0 && Vector2.Dot((p - q), s) <= Vector2.Dot(s, s)))
                        return true;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero(1e-10f) && !qpxr.IsZero(1e-10f))
                return false;

            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross(s) / rxs;

            // u = (q - p) x r / (r x s)

            var u = (q - p).Cross(r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero(1e-10f) && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + t * r;

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }
    }
}