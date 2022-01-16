using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Animation
{
    public class ExpressionDriver<T> : IValueDriver
    {
        private List<T> _states;
        private Action<T, T, float> _drive;

        public ExpressionDriver(Action<T, T, float> driveMethod)
        {
            _states = new List<T>();
            _drive = driveMethod;
        }

        public void Drive(int from, int to, float interpolation)
        {
            if (from < 0 || to < 0 || from >= _states.Count || to >= _states.Count)
                throw new ArgumentException("State Index is out of range!");

            T p1 = _states[from];
            T p2 = _states[to];

            _drive?.Invoke(p1, p2, interpolation);
        }

        public void AddState(T value)
        {
            _states.Add(value);
        }
    }
}
