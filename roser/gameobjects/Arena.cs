using JeremyAnsel.DirectX.D2D1;
using roser.generators;

namespace roser.gameobjects
{
	internal class Arena : IPhysicsObject
	{
		public const float G = 9.80665f * 20 * ArenaUnit;
		public const float ArenaAspect = 9f / 16f;

		// Keep simulation bounds separate from the actual bounds on screen in order to have more precision and carefree resizing
		public const float ArenaSize = 1000;
		public const float ArenaUnit = ArenaSize / 100;
		public const float SimulationWidth = ArenaSize * ArenaAspect;
		public const float SimulationHeight = ArenaSize;

		public D2D1RectF BoundsRect { get; set; } = new(0, 0, SimulationWidth,	SimulationHeight);

		public readonly Ball ball;
		public readonly Paddle paddle;

		public double BallX => ball.X;
		public double BallY => ball.Y;

		public Brick?[,] Bricks { get; set; }

		public ILevelGenerator LevelGenerator { get; set; }

		public delegate void ArenaListener();

		public ArenaListener OnBottomCollision { get; set; }

		public ArenaListener OnNoBricks { get; set; }

		public int BricksLeft { get; set; }

		public Arena()
		{
			ball = new(this);
			paddle = new(this);
			if (SaveFile.CurrentLevel == 0)
			{
				LevelGenerator = new SmileyGenerator();
				Bricks = LevelGenerator.Generate();
			}
			else
			{
				Bricks = new Brick[0, 0];
				LogE($"Unknown level {SaveFile.CurrentLevel}");
			}
			BricksLeft = Bricks.Length;
		}

		public void OnTick(double dt)
		{
			paddle.OnTick(dt);
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
			return (float)(Paddle.Y * _realHeightCoef);
		}
	}
}
