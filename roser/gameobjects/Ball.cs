using static roser.gameobjects.Arena;

namespace roser.gameobjects
{
	internal class Ball(Arena arena) : IPhysicsObject
	{
		public const float Radius = 3 * ArenaUnit;

		// Center of the ball
		public double X { get; set; } = SimulationWidth / 2;
		public double Y { get; set; } = SimulationHeight * 3 / 4;

		public double vx = 5 * ArenaUnit;
		public double vy;

		private double ax = 0 * ArenaUnit;
		private double ay = 0 * ArenaUnit;

		private double accumulator = 0d;

		public void OnTick(double dt)
		{
			accumulator += dt;
			while (accumulator >= IPhysicsObject.targetTickTimeMillis)
			{
				accumulator -= IPhysicsObject.targetTickTimeMillis;
				// dv = dt * a
				vx += IPhysicsObject.targetTickTimeMillis * ax * 0.6 / 1000d;
				vy += IPhysicsObject.targetTickTimeMillis * (ay + G) * 0.6 / 1000d;
				X += IPhysicsObject.targetTickTimeMillis * vx / 1000d;
				Y += IPhysicsObject.targetTickTimeMillis * vy / 1000d;

				Brick?[,] bricks = arena.Bricks;
				// Since we have constant brick size, we can bound check bricks faster
				// This won't work with variable brick size
				float brickSize = arena.LevelGenerator.GetBrickSize();
				const float d = Radius * 2;
				// Neighbourhood 
				int n = (int)(d / brickSize) + 1;

				int bX = (int)((X - Radius) / brickSize);
				int bY = (int)((Y - Radius) / brickSize);
				if (bX < 0)
					bX = 0;
				if (bY < 0)
					bY = 0;

				int widthL = bricks.GetLength(0);
				int heightL = bricks.GetLength(1);

				if (bY < heightL)
				{
					for (int i = 0; i < n; i++)
					{
						int xI = bX + i;
						if (xI < 0)
							continue;
						if (xI >= widthL)
							break;
						for (int j = 0; j < n; j++)
						{
							int yI = bY + j;
							if (yI < 0)
								continue;
							if (yI >= heightL)
								break;
							if (bricks[xI, yI] != null)
							{
								bricks[xI, yI] = null;
								arena.BricksLeft--;
							}
						}
					}
					if (arena.BricksLeft == 0)
						arena.OnNoBricks();
				}

				// Check if in bounds
				if (Y + Radius >= arena.BoundsRect.Bottom)
				{
					arena.OnBottomCollision();
					return;
					//BounceFromBottom();
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
