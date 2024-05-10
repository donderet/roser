using static roser.gameobjects.Arena;

namespace roser.gameobjects
{
	internal class Ball(Arena arena) : IPhysicsObject
	{
		private const float G = 9.80665f * 10 * ArenaUnit;
		// X-axis resistance force
		private const float Rx = 1 * ArenaUnit;
		public const float Radius = 5 * ArenaUnit;

		public double X { get; set; } = Arena.SimulationWidth / 2;
		public double Y { get; set; } = Arena.SimulationHeight / 2;

		private double vx = 10 * ArenaUnit;
		private double vy;

		private double ax = 0 * ArenaUnit;
		private double ay = 0 * ArenaUnit;

		public double accumulator = 0d;

		public void OnTick(double dt)
		{
			accumulator += dt;
			while (accumulator >= IPhysicsObject.targetTickTimeMillis)
			{
				accumulator -= IPhysicsObject.targetTickTimeMillis;
				// a = (v_1 - v_0) / Δt
				// v_1 = Δt * a + v_0
				vx += IPhysicsObject.targetTickTimeMillis * ax / 1000d;
				vy += IPhysicsObject.targetTickTimeMillis * (ay + G) / 1000d;
				// l = vt
				X += IPhysicsObject.targetTickTimeMillis * vx / 1000d;
				Y += IPhysicsObject.targetTickTimeMillis * vy / 1000d;
				// Check if in bounds
				if (Y + Radius > arena.BoundsRect.Bottom)
				{
					OnBallLost();
					Y = arena.BoundsRect.Bottom - Radius;
					vy = -vy;
					//ay -= 4 * ArenaUnit;
					//return;
					OnBoundsCollision();
				}
				else if (Y - Radius < 0)
				{
					Y = Radius;
					vy = -vy;
					vy += 100 * ArenaUnit;
					OnBoundsCollision();
				}
				if (X - Radius < 0)
				{
					X = Radius;
					vx = -vx;
					OnBoundsCollision();
				}
				else if (X + Radius > arena.BoundsRect.Right)
				{
					X = arena.BoundsRect.Right - Radius;
					vx = -vx;
					OnBoundsCollision();
				}
			}
		}

		private void OnBoundsCollision()
		{

		}

		private void OnBallLost()
		{

		}
	}
}
