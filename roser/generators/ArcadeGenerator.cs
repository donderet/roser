using roser.gameobjects;
using Windows.Devices.Radios;

namespace roser.generators
{
	internal class ArcadeGenerator : ILevelGenerator
	{
		float brickSize;

		int brickCount;

		public Brick?[,] Generate()
		{
			Random r = new((int)SaveFile.CurrentLevel);
			int len = r.Next(5, 12);
			brickSize = Arena.SimulationWidth / len;

			Brick?[,] bricks = new Brick?[len, len];

			if ((r.Next() & 0x1) == 1)
			{
				int currX = 0;
				int currY = 0;
				int radius = len / 2;
				for (int row = radius; row >= -radius; row--)
				{
					for (int column = -radius; column <= radius; column++)
					{
						if (Math.Sqrt(row * row + column * column) <= radius)
						{
							uint color = (uint)r.Next();
							bricks[currX, currY] = new(color, currX * brickSize, currY * brickSize, brickSize, brickSize);
							brickCount++;
						}
						currX++;
					}
					currX = 0;
					currY++;
				}
			}
			else
			{
				brickCount = len * len;
				for (int row = 0; row < len; row++)
				{
					for (int column = 0; column < len; column++)
					{
						uint color = (uint)r.Next();
						bricks[row, column] = new(color, row * brickSize, column * brickSize, brickSize, brickSize);
					}
				}
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
