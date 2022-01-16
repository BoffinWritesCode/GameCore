using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;
using Microsoft.Xna.Framework.Input;
using GameCore.Input;

namespace GameCore.UI.Elements
{
    public class UIButton : UIElement
    {
        public UIButton(Action<UIElement, MouseInput> onClick) : base()
        {
            UIUtils.SetupCursorOnHover(this, MouseCursor.Hand);
            
            OnMousePressed += onClick;
        }
    }
}