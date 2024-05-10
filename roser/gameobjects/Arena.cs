using JeremyAnsel.DirectX.D2D1;

namespace roser.gameobjects
{
	internal class Arena : IPhysicsObject
	{
		public const float ArenaAspect = 9f / 16f;

		// Keep simulation bounds separate from the actual bounds on screen in order to have more precision and carefree resizing
		public const float ArenaSize = 1000;
		public const float ArenaUnit = Arena.ArenaSize / 100;
		public const float SimulationWidth = ArenaSize * ArenaAspect;
		public const float SimulationHeight = ArenaSize;

		public D2D1RectF BoundsRect { get; set; } = new(0, 0, SimulationWidth,	SimulationHeight);

		public readonly Ball ball;
		private readonly Paddle paddle;

		public double BallX => ball.X;
		public double BallY => ball.Y;

		public Arena()
		{
			ball = new(this);
			paddle = new(this);
		}

		public void OnTick(double dt)
		{
			ball.OnTick(dt);
		}

		public float _realWidthCoef;
		public float _realHeightCoef;

		public double RealWidthCoef => _realWidthCoef;
		public double RealHeightCoef => _realHeightCoef;

		public void SetRealWidth(float width)
		{
			_realWidthCoef = width / SimulationWidth;
		}

		public void SetRealHeight(float height)
		{
			_realHeightCoef = height / SimulationHeight;
		}

		public float GetBallX()
		{
			return (float)(ball.X * _realWidthCoef);
		}

		public float GetBallY()
		{
			return (float)(ball.Y * _realHeightCoef);
		}

		public float GetPaddleX()
		{
			return (float)(paddle.X * _realWidthCoef);
		}

		public float GetPaddleY()
		{
			return (float)(paddle.Y * _realHeightCoef);
		}
	}
}
