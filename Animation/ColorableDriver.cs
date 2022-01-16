using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GameCore.Graphics;

namespace GameCore.Animation
{
    public class ColorableDriver : IValueDriver
    {
        protected IColorable _colorable;
        protected List<ColorableState> _states;

        public ColorableDriver(IColorable colorable, bool getInitialState = false)
        {
            _colorable = colorable;
            _states = new List<ColorableState>();
            
            // add initial state
            if (getInitialState)
                _states.Add(new ColorableState() { Color = colorable.Color, Multiplier = colorable.ColorMultiplier });
        }

        public void Drive(int from, int to, float interpolation)
        {
            if (from < 0 || to < 0 || from >= _states.Count || to >= _states.Count) 
                throw new ArgumentException("State Index is out of range!");

            ColorableState p1 = _states[from];
            ColorableState p2 = _states[to];

            // only lerp if non-null
            if (p1.Color.HasValue)
                _colorable.Color = Color.Lerp(p1.Color.Value, p2.Color.Value, interpolation);

            _colorable.ColorMultiplier = Vector4.Lerp(p1.Multiplier, p2.Multiplier, interpolation);
        }

        public void AddState(Vector4 multiplier)
        {
            _states.Add(new ColorableState() { Color = null, Multiplier = multiplier });
        }

        public void AddState(Color colour)
        {
            _states.Add(new ColorableState() { Color = colour, Multiplier = Vector4.One });
        }

        public void AddState(Color colour, Vector4 multiplier)
        {
            _states.Add(new ColorableState() { Color = colour, Multiplier = multiplier });
        }

        public void AddState(int r, int g, int b, int a)
        {
            _states.Add(new ColorableState() { Color = Color.FromNonPremultiplied(r, g, b, a), Multiplier = Vector4.One });
        }

        protected struct ColorableState
        {
            public Color? Color { get; set; }
            public Vector4 Multiplier { get; set; }
        }
    }
}