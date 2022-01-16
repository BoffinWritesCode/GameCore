using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Maths;
using Microsoft.Xna.Framework;

namespace GameCore.Utility.Camera
{
    public class CameraShake : IValueModifier<CameraValues>
    {
        private ShakeData[] _myData;

        /// <param name="ease">Ease should be an ease out, not an ease in or ease in-out.</param>
        public CameraShake(int maxShakes = 8)
        {
            _myData = new ShakeData[maxShakes];
            for (int i = 0; i < maxShakes; i++) _myData[i] = new ShakeData();
        }

        public bool NewShake(Easing ease, float length, float strength)
        {
            for (int i = 0; i < _myData.Length; i++)
            {
                if (_myData[i].Active) continue;

                _myData[i].Reset(ease, length, strength);

                return true;
            }

            return false;
        }

        public void Modify(ref CameraValues camera)
        {
            Vector2 offset = Vector2.Zero;
            for (int i = 0; i < _myData.Length; i++)
            {
                if (!_myData[i].Active) continue;

                offset += _myData[i].GetOffset();
            }

            camera.Position += offset;
        }

        public class ShakeData
        {
            public bool Active { get; private set; }

            private Easing _ease;
            private float _length;
            private float _strength;
            private float _currentTime;

            public void Reset(Easing ease, float length, float strength)
            {
                _ease = ease;
                _length = length;
                _strength = strength;
                _currentTime = 0f;
                Active = true;
            }

            public Vector2 GetOffset()
            {
                _currentTime += Time.DeltaTime;
                if (_currentTime >= _length)
                {
                    Active = false;
                    return Vector2.Zero;
                }

                float percent = _currentTime / _length;
                float strength = _ease.Interpolate(_strength, 0f, percent);
                float angle = Engine.Random.NextFloat(-MathHelper.Pi, MathHelper.Pi);
                return angle.ToDirection() * strength;
            }
        }
    }
}
