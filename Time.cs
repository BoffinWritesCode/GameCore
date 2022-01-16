using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameCore
{
    /// <summary>
    /// A static class exposing values related to the amount of time between frames or the application's lifetime.
    /// <br>Game speed is also modifiable via this class.</br>
    /// </summary>
    public static class Time
    {
        public static float GameSpeed { get; set; }
        public static float DeltaTime { get; private set; }
        public static float RawDeltaTime { get; private set; }
        public static float DrawDeltaTime { get; private set; }
        public static float RawTotalTime { get; private set; }
        public static float TotalTime { get; private set; }

        static Time()
        {
            GameSpeed = 1f;
        }

        public static void Update(GameTime gameTime)
        {
            RawTotalTime = (float)gameTime.TotalGameTime.TotalSeconds;
            RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            DeltaTime = RawDeltaTime * GameSpeed;
            TotalTime += DeltaTime;
        }

        public static void UpdateDraw(GameTime gameTime)
        {
            DrawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
