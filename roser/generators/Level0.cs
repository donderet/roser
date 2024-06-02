using roser.gameobjects;

namespace roser.generators
{
	internal class Level0 : ILevelGenerator
	{
		const uint art = 0x0011ee;

		const int len = 10;
		const float brickSize = Arena.SimulationWidth / len;

		public Brick?[,] Generate()
		{
			Brick?[,] bricks = new Brick?[len, len];

			for (int row = 0; row < len; row++)
			{
				for (int column = 0; column < len; column++)
				{
					bricks[row, column] = new(art, row * brickSize, column * brickSize, brickSize, brickSize);
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
			return len * len;
		}
	}
}
