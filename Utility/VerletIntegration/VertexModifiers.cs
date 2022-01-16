using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Utility.VerletIntegration
{
    public class VertexVelocity : IClassModifier<Vertex>
    {
        private float _mult;

        public VertexVelocity(float multiplier)
        {
            _mult = multiplier;
        }

        public void Modify(Vertex vertex)
        {
            vertex.Position += (vertex.Position - vertex.LastPosition) * _mult;
        }
    }

    public class VertexGravity : IClassModifier<Vertex>
    {
        private float _grav;
        private Vector2 _gravityDir;
        private float _terminalVelocity;

        public VertexGravity(float gravity, float maxVelocityY)
        {
            _grav = gravity;
            _gravityDir = Vector2.UnitY;
            _terminalVelocity = maxVelocityY;
        }

        public VertexGravity(float gravity, Vector2 dir)
        {
            _grav = gravity;
            _gravityDir = Vector2.Normalize(dir);
        }

        public void Modify(Vertex vertex)
        {
            vertex.Position += _gravityDir * _grav * Time.DeltaTime;
            float deltaY = vertex.Position.Y - vertex.LastPosition.Y;
            if (deltaY > _terminalVelocity) vertex.Position = new Vector2(vertex.Position.X, vertex.LastPosition.Y + _terminalVelocity);
        }
    }

    /*
    public class VertexNoiseTurbulence : IVertexModifier
    {
        private static readonly PerlinNoise X_NOISE = new PerlinNoise(69696969);
        private static readonly PerlinNoise Y_NOISE = new PerlinNoise(696969);
        private static readonly PerlinNoise STRENGTH_NOISE = new PerlinNoise(420420);

        private float _mult;
        private float _spaceMult;

        public VertexNoiseTurbulence(float multiplier = 0.1f, float spaceMultiplier = 0.1f)
        {
            _spaceMult = spaceMultiplier;
            _mult = multiplier;
        }

        public void ModifyVertex(Vertex vertex)
        {
            float time = (float)Main._drawInterfaceGameTime.TotalGameTime.TotalSeconds * 0.2f;
            Vector2 pos = vertex.Position * _spaceMult;

            Vector2 dir = Vector2.Normalize(new Vector2(X_NOISE.Noise3D(pos.X, pos.Y, time) * 2f - 1f, Y_NOISE.Noise3D(pos.X, pos.Y, time) * 2f - 1f));

            vertex.Position += dir * STRENGTH_NOISE.Noise3D(pos.X, pos.Y, time) * _mult;
        }
    }
    */
}
