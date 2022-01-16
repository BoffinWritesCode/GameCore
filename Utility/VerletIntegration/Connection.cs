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
	public class Connection
	{
		public Vertex Vertex1 { get; set; }
		public Vertex Vertex2 { get; set; }
		public float Length { get; set; }
		public IConnectionDrawer SegmentDrawer { get; set; }

		public Connection(Vertex vertex1, Vertex vertex2, float length)
		{
			Vertex1 = vertex1;
			Vertex2 = vertex2;
			Length = length;
		}

		public Connection(Vertex vertex1, Vertex vertex2, IConnectionDrawer drawer, float length) : this(vertex1, vertex2, length)
		{
			SegmentDrawer = drawer;
		}

		public void Draw(SpriteBatch sB, int index, ConnectionType type)
		{
			SegmentDrawer?.DrawConnection(sB, this, index, type);
		}
	}
}
