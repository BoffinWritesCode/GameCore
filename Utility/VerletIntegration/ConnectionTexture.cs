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
    public class ConnectionTexture : IConnectionDrawer
    {
        protected Texture2D _texture;

        public Color Color { get; set; }
        public float Thickness { get; set; }
        public bool WorldLit { get; set; } = true;
        public bool TextureWrap { get; set; } = false;

        public ConnectionTexture(Texture2D texture, Color color)
        {
            _texture = texture;
            Color = color;

            Thickness = texture.Height;
        }

        public void DrawConnection(SpriteBatch spriteBatch, Connection connection, int index, ConnectionType type)
        {
            Vector2 delta = (connection.Vertex1.Position - connection.Vertex2.Position);
            float rotation = (float)Math.Atan2(delta.Y, delta.X);

            float length = delta.Length();
            Rectangle source =
                TextureWrap ?
                    new Rectangle(0, 0, (int)length, _texture.Height) :
                    new Rectangle(0, 0, (int)Math.Ceiling(MathHelper.Clamp(length, 0f, _texture.Width)), _texture.Height);
            Vector2 scale = new Vector2(length / source.Width, Thickness / source.Height);

            spriteBatch.Draw(_texture, connection.Vertex2.Position, source, Color, rotation, new Vector2(0f, _texture.Height * 0.5f), scale, SpriteEffects.None, 0);
        }
    }
}
