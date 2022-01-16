using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GameCore.UI;

namespace GameCore.Animation
{
    public class AnchorPointDriver : IValueDriver
    {
        protected AnchorRect _rect;
        protected int _side;
        protected List<AnchorPoint> _states;
        
        /// <summary>
        /// Drives one anchor point of a UIRect.
        /// <br>0 = left, 1 = top, 2 = right, 3 = bottom</br>
        /// </summary>
        public AnchorPointDriver(AnchorRect rect, int side)
        {
            _rect = rect;
            _side = side;
            _states = new List<AnchorPoint>();

            // add initial state
            _states.Add(GetAnchorPoint());
        }

        public void Drive(int from, int to, float interpolation)
        {
            if (from < 0 || to < 0 || from >= _states.Count || to >= _states.Count) 
                throw new ArgumentException("State Index is out of range!");

            AnchorPoint p1 = _states[from];
            AnchorPoint p2 = _states[to];

            SetAnchorPoint(AnchorPoint.Lerp(p1, p2, interpolation));
        }

        public void AddState(AnchorPoint point)
        {
            _states.Add(point);
        }

        public void AddState(float axisInterp, float pixelOffset)
        {
            _states.Add(new AnchorPoint(axisInterp, pixelOffset));
        }

        protected AnchorPoint GetAnchorPoint()
        {
            return _side switch
            {
                1 => _rect.Top,
                2 => _rect.Right,
                3 => _rect.Bottom,
                _ => _rect.Left,
            };
        }

        protected void SetAnchorPoint(AnchorPoint point)
        {
            switch (_side)
            {
                default:
                case 0:
                    _rect.Left = point;
                    break;
                case 1:
                    _rect.Top = point;
                    break;
                case 2:
                    _rect.Right = point;
                    break;
                case 3:
                    _rect.Bottom = point;
                    break;
            }
        }
    }
}