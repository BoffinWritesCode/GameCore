using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace GameCore.Maths
{
    public abstract class Easing
    {
        public static readonly Easing Linear = new ActionEase((float x) => { return x; });
        public static readonly Easing ReverseLinear = new ActionEase((float x) => { return 1f - x; });

        public static readonly Easing EaseQuadIn = new ActionEase((float x) => { return x * x; });
        public static readonly Easing EaseQuadOut = new ActionEase((float x) => { return 1f - EaseQuadIn.Ease(1f - x); });
        public static readonly Easing EaseQuadInOut = new ActionEase((float x) => { return (x < 0.5f) ? 2f * x * x : -2f * x * x + 4f * x - 1f; });

        public static readonly Easing EaseCubicIn = new ActionEase((float x) => { return x * x * x; });
        public static readonly Easing EaseCubicOut = new ActionEase((float x) => { return 1f - EaseCubicIn.Ease(1f - x); });
        public static readonly Easing EaseCubicInOut = new ActionEase((float x) => { return (x < 0.5f) ? 4f * x * x * x : 4f * x * x * x - 12f * x * x + 12f * x - 3f; });

        public static readonly Easing EaseQuarticIn = new ActionEase((float x) => { return x * x * x * x; });
        public static readonly Easing EaseQuarticOut = new ActionEase((float x) => { return 1f - EaseQuarticIn.Ease(1f - x); });
        public static readonly Easing EaseQuarticInOut = new ActionEase((float x) => { return (x < 0.5f) ? 8f * x * x * x * x : -8f * x * x * x * x + 32f * x * x * x - 48f * x * x + 32f * x - 7f; });

        public static readonly Easing EaseQuinticIn = new ActionEase((float x) => { return x * x * x * x * x; });
        public static readonly Easing EaseQuinticOut = new ActionEase((float x) => { return 1f - EaseQuinticIn.Ease(1f - x); });
        public static readonly Easing EaseQuinticInOut = new ActionEase((float x) => { return (x < 0.5f) ? 16f * x * x * x * x * x : 16f * x * x * x * x * x - 80f * x * x * x * x + 160f * x * x * x - 160f * x * x + 80f * x - 15f; });

        public static readonly Easing EaseCircularIn = new ActionEase((float x) => { return 1f - (float)Math.Sqrt(1.0 - Math.Pow(x, 2)); });
        public static readonly Easing EaseCircularOut = new ActionEase((float x) => { return (float)Math.Sqrt(1.0 - Math.Pow(x - 1.0, 2)); });
        public static readonly Easing EaseCircularInOut = new ActionEase((float x) => { return (x < 0.5f) ? (1f - (float)Math.Sqrt(1.0 - Math.Pow(x * 2, 2))) * 0.5f : (float)((Math.Sqrt(1.0 - Math.Pow(-2 * x + 2, 2)) + 1) * 0.5); });

        public const float C4 = 2f * MathF.PI / 3f;
        public static readonly Easing EaseElasticOut = new ActionEase((float x) => 
        {
            float newX = MathHelper.Clamp(x, 0f, 1f);
            return MathF.Pow(2f, -10 * newX) * MathF.Sin((newX * 10f - 0.75f) * C4) + 1f; 
        });

        /*
            function easeOutElastic(x: number): number {
            const c4 = (2 * Math.PI) / 3;

            return x === 0
                ? 0
                : x === 1
                ? 1
                : pow(2, -10 * x) * sin((x * 10 - 0.75) * c4) + 1;
            } 
            */

        public virtual float Ease(float time)
        {
            throw new NotImplementedException();
        }

        public float Interpolate(float start, float end, float time)
        {
            return MathHelper.Lerp(start, end, Ease(time));
        }
    }

    public class ActionEase : Easing
    {
        private Func<float, float> _function;

        public ActionEase(Func<float, float> func)
        {
            _function = func;
        }

        public override float Ease(float time)
        {
            return _function(time);
        }
    }

    public class EaseBuilder : Easing
    {
        private List<EasePoint> _points;

        public EaseBuilder()
        {
            _points = new List<EasePoint>();
        }

        public void AddPoint(float x, float y, Easing function) => AddPoint(new Vector2(x, y), function);

        public void AddPoint(Vector2 vector, Easing function)
        {
            if (vector.X > 1f || vector.X < 0f) throw new ArgumentException("X value of point is not in valid range!");

            EasePoint newPoint = new EasePoint(vector, function);
            if (_points.Count == 0)
            {
                _points.Add(newPoint);
                return;
            }

            EasePoint last = _points[^1];
            if (last.Point.X > vector.X) throw new ArgumentException("New point has an x value less than the previous point when it should be greater or equal");

            _points.Add(newPoint);
        }

        public override float Ease(float time)
        {
            Vector2 prevPoint = Vector2.Zero;
            EasePoint usePoint = _points[0];
            for (int i = 0; i < _points.Count; i++)
            {
                usePoint = _points[i];
                if (time <= usePoint.Point.X)
                {
                    break;
                }
                prevPoint = usePoint.Point;
            }
            float dist = usePoint.Point.X - prevPoint.X;
            float progress = (time - prevPoint.X) / dist;
            return MathHelper.Lerp(prevPoint.Y, usePoint.Point.Y, usePoint.Function.Ease(progress));
        }

        private struct EasePoint
        {
            public Vector2 Point;
            public Easing Function;

            public EasePoint(Vector2 p, Easing func)
            {
                Point = p;
                Function = func;
            }
        }
    }
}