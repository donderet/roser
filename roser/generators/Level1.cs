using roser.gameobjects;

namespace roser.generators
{
	internal class Level1 : ILevelGenerator
	{
		const uint art = 0x0011ee;

		const int radius = 5;
		const float brickSize = Arena.SimulationWidth / (radius * 2 + 1);

		int brickCount;

		public Brick?[,] Generate()
		{
			Brick?[,] bricks = new Brick?[radius * 2 + 1, radius * 2 + 1];

			int currX = 0;
			int currY = 0;

			for (int row = radius; row >= -radius; row--)
			{
				for (int column = -radius; column <= radius; column++)
				{
					if (Math.Sqrt(row * row + column * column) <= radius)
					{
						bricks[currX, currY] = new(art, currX * brickSize, currY * brickSize, brickSize, brickSize);
						brickCount++;
					}
					currX++;
				}
				currX = 0;
				currY++;
			}
			return bricks;
		}

		public float GetBrickSize()
		{
			return brickSize;
		}

		public int GetTotalBricks()
		{
			return brickCount;
		}
	}
}
