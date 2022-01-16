using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.Graphics
{
    public interface IColorable
    {
        Color Color { get; set; }
        Vector4 ColorMultiplier { get; set; }
    }
}