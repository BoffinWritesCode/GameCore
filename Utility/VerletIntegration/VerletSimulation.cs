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
	public class VerletSimulation
	{
		public int IterationCount { get; set; } = 6;

		public List<Connection> Connections { get; set; }
		public List<Vertex> Vertices { get; set; }
		public List<IClassModifier<Vertex>> VertexModifiers { get; }

		public Vertex FirstVertex => Vertices[0];
		public Vertex LastVertex => Vertices[Vertices.Count - 1];

		public VerletSimulation(SimulationPhysics? physics = null)
		{
			Connections = new List<Connection>();
			Vertices = new List<Vertex>();
			VertexModifiers = new List<IClassModifier<Vertex>>();

			SimulationPhysics coefficients = physics ?? SimulationPhysics.Default();

			VertexModifiers.Add(new VertexVelocity(coefficients.Drag));
			VertexModifiers.Add(new VertexGravity(coefficients.GravitationalAcceleration, coefficients.TerminalVelocity));
		}

		public virtual void Update()
		{
			foreach (Vertex v in Vertices)
			{
				if (v.Locked) continue;

				Vector2 before = v.Position;
				foreach (IClassModifier<Vertex> modifier in VertexModifiers)
				{
					modifier.Modify(v);
				}
				v.LastPosition = before;
			}

			for (int i = 0; i < IterationCount; i++)
			{
				foreach (Connection c in Connections)
				{
					/*
					Vector2 center = (c.Vertex1.Position + c.Vertex2.Position) * 0.5f;
					Vector2 dir = Vector2.Normalize(c.Vertex1.Position - c.Vertex2.Position);
					if (!c.Vertex1.Locked)
						c.Vertex1.Position = center + dir * c.Length * 0.5f;
					else
						c.Vertex2.Position = c.Vertex1.Position - dir * c.Length;

					if (!c.Vertex2.Locked)
						c.Vertex2.Position = center - dir * c.Length * 0.5f;
					else
						c.Vertex1.Position = c.Vertex2.Position + dir * c.Length;
					*/

					Vector2 delta = c.Vertex1.Position - c.Vertex2.Position;
					float d2 = delta.Length();
					float d1 = c.Length;
					float diff = d2 - d1;
					float percent = (diff / d2) * 0.5f;
					Vector2 offset = delta * percent;
					if (!c.Vertex1.Locked) c.Vertex1.Position -= offset;
					if (!c.Vertex2.Locked) c.Vertex2.Position += offset;
				}
			}
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			// draw segments
			ConnectionType type = ConnectionType.Head;
			for (int i = 0; i < Connections.Count; i++)
			{
				var segment = Connections[i];
				segment.Draw(spriteBatch, i, type);

				// modify the segment type
				type = ConnectionType.Body;
				if (type == ConnectionType.Body && i == Connections.Count - 2) type = ConnectionType.Tail;
			}

			// draw vertices
			VertexType vertexType = VertexType.Head;
			for (int i = 0; i < Vertices.Count; i++)
			{
				var vertex = Vertices[i];
				vertex.Draw(spriteBatch, i, vertexType);

				vertexType = VertexType.Body;
				if (vertexType == VertexType.Body && i == Vertices.Count - 2) vertexType = VertexType.Tail;
			}
		}

		public Vertex AddVertex(Vector2 pos, IVertexDrawer drawer = null)
		{
			Vertex v = new Vertex(this, pos, drawer);
			Vertices.Add(v);
			return v;
		}

		public Connection ConnectVertices(Vertex v1, Vertex v2, IConnectionDrawer drawer = null)
		{
			Connection c = new Connection(v1, v2, drawer, Vector2.Distance(v1.Position, v2.Position));
			Connections.Add(c);
			return c;
		}

		public Connection ConnectVertices(Vertex v1, Vertex v2, float length, IConnectionDrawer drawer = null)
		{
			Connection c = new Connection(v1, v2, drawer, length);
			Connections.Add(c);
			return c;
		}

		/// <param name="slackLength">The extra length given to each segment.</param>
		public static VerletSimulation CreateRope(Vector2 start, Vector2 end, float segmentLength, float slackLength, SimulationPhysics? physics = null, IVertexDrawer vertDrawer = null, IConnectionDrawer connectionDrawer = null)
		{
			VerletSimulation sim = new VerletSimulation(physics);
			Vector2 pos = start;
			Vertex current = sim.AddVertex(pos);
			current.Locked = true;
			float dist = Vector2.Distance(start, end);
			int connectionCount = (int)Math.Ceiling(dist / segmentLength);
			float actualLength = dist / connectionCount;
			Vector2 dir = (end - start) / connectionCount;
			for (int i = 0; i < connectionCount; i++)
			{
				pos += dir;
				current = current.ConnectToNew(pos, actualLength + slackLength, vertDrawer, connectionDrawer);
			}
			current.Locked = true;
			return sim;
		}
	}
}
