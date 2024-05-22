using static roser.gameobjects.Arena;

namespace roser.gameobjects
{
	internal class Ball(Arena arena) : IPhysicsObject
	{
		// X-axis resistance force
		private const float Rx = 1f * ArenaUnit;
		public const float Radius = 3 * ArenaUnit;

		public double X { get; set; } = SimulationWidth / 2;
		public double Y { get; set; } = SimulationHeight / 2;

		public double vx = 5 * ArenaUnit;
		public double vy;

		public double ax = 0 * ArenaUnit;
		public double ay = 0 * ArenaUnit;

		public double accumulator = 0d;

		public void OnTick(double dt)
		{
			accumulator += dt;
			while (accumulator >= IPhysicsObject.targetTickTimeMillis)
			{
				accumulator -= IPhysicsObject.targetTickTimeMillis;
				// a = (v_1 - v_0) / dt
				// v_1 = dt * a + v_0
				vx += IPhysicsObject.targetTickTimeMillis * ax * 0.6 / 1000d;
				vy += IPhysicsObject.targetTickTimeMillis * (ay + G) * 0.6 / 1000d;
				// l = vt
				X += IPhysicsObject.targetTickTimeMillis * vx / 1000d;
				Y += IPhysicsObject.targetTickTimeMillis * vy / 1000d;
				// Check if in bounds
				if (Y + Radius >= arena.BoundsRect.Bottom)
				{
					BounceFromBottom();
				}
				else if (Y - Radius <= 0)
				{
					BounceFromTop();
				}
				if (X - Radius <= 0)
				{
					BounceFromLeft();
				}
				else if (X + Radius >= arena.BoundsRect.Right)
				{
					BounceFromRight();
				}

			}
		}

		public void BounceFromBottom()
		{
			Y = arena.BoundsRect.Bottom - Radius;
			vy = -vy;
		}

		public void BounceFromTop()
		{
			Y = Radius;
			vy = 0;
		}

		public void BounceFromRight()
		{
			X = arena.BoundsRect.Right - Radius;
			vx = -vx;
		}

		public void BounceFromLeft()
		{
			X = Radius;
			vx = -vx;
		}

		private void OnBallLost()
		{

		}
	}
}
