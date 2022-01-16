using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore.Utility.VerletIntegration
{
	public struct SimulationPhysics
	{
		public float Drag;
		public float GravitationalAcceleration;
		public float TerminalVelocity;

		public SimulationPhysics(float drag, float gravity, float terminalV)
		{
			this.Drag = drag;
			this.GravitationalAcceleration = gravity;
			this.TerminalVelocity = terminalV;
		}

		public static SimulationPhysics Default()
		{
			return new SimulationPhysics(1f, 0.2f, 10f);
		}
	}
}
