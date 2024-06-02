using roser.gameobjects;

namespace roser.generators
{
	internal class Level2 : ILevelGenerator
	{
		const uint art = 0xee00ee;
		const uint bg = 0xee0000;

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
						uint color = row == column ? bg : art;
						bricks[currX, currY] = new(color, currX * brickSize, currY * brickSize, brickSize, brickSize);
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
