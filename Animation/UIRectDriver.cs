using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GameCore.UI;

namespace GameCore.Animation
{
    public class AnchorRectDriver : IValueDriver
    {
        protected AnchorRect _rect;
        protected int _side;
        protected List<AnchorRect> _states;
        
        public AnchorRectDriver(AnchorRect rect)
        {
            _rect = rect;
            _states = new List<AnchorRect>();

            // add initial state
            _states.Add(new AnchorRect(rect));
        }

        public void Drive(int from, int to, float interpolation)
        {
            if (from < 0 || to < 0 || from >= _states.Count || to >= _states.Count) 
                throw new ArgumentException("State Index is out of range!");

            AnchorRect p1 = _states[from];
            AnchorRect p2 = _states[to];

            _rect.Lerp(p1, p2, interpolation);
        }

        public void AddState(AnchorRect rect)
        {
            _states.Add(rect);
        }
    }
}