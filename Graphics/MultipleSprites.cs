using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.Graphics
{
    public class MultipleSprites : ISprite
    {
        protected List<ISprite> _sprites;
        protected int _currentFrame;

        public MultipleSprites()
        {
            _sprites = new List<ISprite>();
        }

        public int AddSprite(ISprite sprite)
        {
            _sprites.Add(sprite);
            return _sprites.Count - 1;
        }

        public MultipleSprites SetFrame(int index) 
        {
            _currentFrame = index;
            return this;
        }
        
        public TextureInfo GetTextureInfo()
        {
            return _sprites[_currentFrame].GetTextureInfo();
        }
    }
}