using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore.UI
{
    public interface IConstraint
    {
        void ApplyConstraint(ref RectangleF current, RectangleF parent);
    }
}