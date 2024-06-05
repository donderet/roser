using JeremyAnsel.DirectX.D2D1;
using static roser.gameobjects.Arena;

namespace roser.gameobjects
{
	internal class Paddle(Arena arena) : IPhysicsObject
	{
		public const float Width = SimulationWidth / 4;
		public const float Height = SimulationWidth / 20;

		public double X { get; set; } = SimulationWidth / 2 - (Width / 2);
		public const double Y = SimulationHeight * 0.95 - Height;

		public bool MovingRight { get; set; } = false;
		public bool MovingLeft { get; set; } = false;
		public D2D1Matrix3X2F PaddleRotation { get; set; }

		public float Angle { get; set; }

		private double vx = 0 * ArenaUnit;

		private double ax = 0 * ArenaUnit;

		private double ac = 0 * ArenaUnit;
		private double vc = 0 * ArenaUnit;

		private double accumulator = 0d;

		public void OnTick(double dt)
		{
			const double maxAngle = 20;
			if (MovingRight)
			{
				ax = 100 * ArenaUnit;
				ac = 0.1 * ArenaUnit;
			}
			else if (MovingLeft)
			{
				ax = -100 * ArenaUnit;
				ac += -0.1 * ArenaUnit;
			}
			else
			{
				ax = 0;
				ac = 0;
			}
			accumulator += dt;
			while (accumulator >= IPhysicsObject.targetTickTimeMillis)
			{
				accumulator -= IPhysicsObject.targetTickTimeMillis;
				vx += IPhysicsObject.targetTickTimeMillis * ax / 1000d;
				// resistance
				if (vx > 0)
					vx -= 0.001;
				else if (vx < 0)
					vx += 0.001;
				X += IPhysicsObject.targetTickTimeMillis * vx / 1000d;
				if (X <= 0)
				{
					X = 0;
					vx = -vx * 0.7;
				}
				else if (X + Width >= arena.BoundsRect.Right)
				{
					X = arena.BoundsRect.Right - Width;
					vx = -vx * 0.7;
				}
			}
			// a_c = dv/dt = dθ*v/dt = v^2/r
			// v = √(ar)
			// dθ = a_c*dt/v
			if (ac != 0)
			{
				vc = Math.Sqrt(Math.Abs(ac * Width));
				Angle += (float)(ac * dt / vc);
			}
			else if (Angle != 0)
			{
				vc = Math.Sqrt(Math.Abs(Width));
				Angle -= (float)Math.Round(Math.CopySign(dt, Angle) / vc);
			}
			if (Angle > maxAngle)
				Angle = (float)maxAngle;
			else if (Angle < -maxAngle)
				Angle = (float)-maxAngle;

			PaddleRotation = D2D1Matrix3X2F.Rotation(Angle, new((float)X + (Width / 2), (float)Y + (Height / 2)));
			D2D1Point2F point = D2D1Matrix3X2F.Rotation(-Angle, new((float)X + (Width / 2), (float)Y + (Height / 2))).TranformPoint(new((float)arena.ball.X, (float)arena.ball.Y));

			var dx = point.X - Math.Max(X, Math.Min(point.X, X + Width));
			var dy = point.Y - Math.Max(Y, Math.Min(point.Y, Y + Height));
			if ((dx * dx + dy * dy) < (Ball.Radius * Ball.Radius)) {
				point.Y = (float)(Y - Height);
				arena.ball.Y = PaddleRotation.TranformPoint(point).Y;
				const float vBoost = 15 * ArenaUnit;
				arena.ball.vy = -arena.ball.vy - (Math.Cos(Angle) * vBoost);
				arena.ball.vx += Math.Sin(Angle) * vBoost;
			}

		}
	}
}
