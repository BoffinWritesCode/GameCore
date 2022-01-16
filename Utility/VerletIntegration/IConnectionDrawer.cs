using Microsoft.Xna.Framework.Graphics;
using GameCore.Utility.VerletIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Graphics;

namespace GameCore.Utility.VerletIntegration
{
    public interface IConnectionDrawer
    {
        void DrawConnection(SpriteBatch spriteBatch, Connection connection, int index, ConnectionType type);
    }
}
