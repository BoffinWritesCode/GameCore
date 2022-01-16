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
	public class Vertex
	{
		public VerletSimulation Simulation { get; }
		public Vector2 Position { get; set; }
		public Vector2 LastPosition { get; set; }
		public bool Locked { get; set; }
		public IVertexDrawer VertexDrawer { get; set; }

		public Vertex(VerletSimulation sim, Vector2 position, bool locked = false)
		{
			Simulation = sim;
			Position = position;
			LastPosition = position;
			Locked = locked;
		}

		public Vertex(VerletSimulation sim, Vector2 position, IVertexDrawer drawer, bool locked = false) : this(sim, position, locked)
		{
			VertexDrawer = drawer;
		}

		public Vertex ConnectToNew(Vector2 pos, IVertexDrawer drawer = null, IConnectionDrawer connDrawer = null)
		{
			Vertex newVertex = Simulation.AddVertex(pos, drawer);
			Simulation.ConnectVertices(this, newVertex, connDrawer);
			return newVertex;
		}

		public Vertex ConnectToNew(Vector2 pos, float length, IVertexDrawer drawer = null, IConnectionDrawer connDrawer = null)
		{
			Vertex newVertex = Simulation.AddVertex(pos, drawer);
			Simulation.ConnectVertices(this, newVertex, length, connDrawer);
			return newVertex;
		}

		public Vertex SetLocked(bool locked)
		{
			Locked = locked;
			return this;
		}

		public void Draw(SpriteBatch spriteBatch, int index, VertexType type)
		{
			VertexDrawer?.DrawVertex(spriteBatch, this, index, type);
		}
	}
}
