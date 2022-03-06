using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public interface IScene
    {
        MiscSystemHandler MiscSystemHandler { get; }
        void Load();
        void Unload();
        void Update();
        void Draw();
    }

    public interface ISubScene : IScene
    { 
        Rectangle ScreenArea { get; }
    }
}
