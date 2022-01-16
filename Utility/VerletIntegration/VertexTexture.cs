using GameCore.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Utility.VerletIntegration
{
    public class VertexTexture : IVertexDrawer
    {
        protected Texture2D _texture;

        public Color Color { get; set; }
        public bool WorldLit { get; set; } = true;

        public VertexTexture(Texture2D texture, Color color)
        {
            _texture = texture;
            Color = color;
        }

        public void DrawVertex(SpriteBatch spriteBatch, Vertex vertex, int index, VertexType type)
        {
            spriteBatch.Draw(_texture, vertex.Position, null, Color, 0f, _texture.Bounds.Size() * 0.5f, 1f, SpriteEffects.None, 0);
        }
    }
}
