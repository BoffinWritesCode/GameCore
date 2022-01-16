using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.Graphics
{
    public class LoadableSprite : ISprite
    {
        private Texture2D _texture;
        private Rectangle? _src;

        public LoadableSprite(string name, Rectangle? rect = null)
        {
            _texture = Engine.ContentManager.Load<Texture2D>(name);
            _src = rect;
        }
        
        public TextureInfo GetTextureInfo()
        {
            return new TextureInfo(_texture, _src);
        }
    }
}