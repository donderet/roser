using roser.gameobjects;

namespace roser.generators
{
	internal class Level4 : ILevelGenerator
	{
		const uint art = 0xaa0000;
		const uint bg = 0x112356;

		const int radius = 10;
		const int eyesRadius = radius / 10;
		const float brickSize = (Arena.SimulationWidth / (radius * 2 + 1));

		public float GetBrickSize()
		{
			return brickSize;
		}

		public Brick?[,] Generate()
		{
			Brick?[,] bricks = new Brick?[radius * 2 + 1, radius * 2 + 1];
			int currX = 0;
			int currY = 0;


			// smiley base (circle): x^2 + y^2 = radius^2
			// eyes: (x - |r/2|)^2 + (y + r/2)^2 = eyesRadius^2
			// smile:
			// (x - h)^2/a^2 + (y - k)^2/b^2 = 1
			// (x - h)^2*b^2 + (y - k)^2*a^2 = a^2*b^2
			// y <= k

			for (int row = radius; row >= -radius; row--)
			{
				for (int column = -radius; column <= radius; column++)
				{

					int eye1X = column + (radius >> 1);
					int eye2X = column - (radius >> 1);
					int eyesY = row - (radius >> 1);

					const int smileOffset = -radius * 3 / 8;
					int smileY = row - smileOffset;
					const int smileWidth = radius * 3 / 4;
					const int smileHeight = radius >> 1;

					uint color =
						   (int)Math.Sqrt(row * row + column * column) <= radius &&
								   !(eye1X * eye1X + eyesY * eyesY <= eyesRadius * eyesRadius ||
										   eye2X * eye2X + eyesY * eyesY <= eyesRadius * eyesRadius ||
										   (row <= smileOffset && column * column * smileHeight * smileHeight + smileY * smileY * smileWidth * smileWidth <= smileWidth * smileWidth * smileHeight * smileHeight))
								   ?
								   art :
								   bg;
					bricks[currX, currY] = new(color, currX * brickSize, currY * brickSize, brickSize, brickSize);
					currX++;
				}
				currX = 0;
				currY++;
			}
			return bricks;
		}

		public int GetTotalBricks()
		{
			return (radius * 2 + 1) * (radius * 2 + 1);
		}
	}
}
