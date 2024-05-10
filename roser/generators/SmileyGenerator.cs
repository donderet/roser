//namespace roser.generators
//{
//	internal class SmileyGenerator : ILevelGenerator
//	{
//		public bool[,] Generate()
//		{
//			int currX = 0;
//			int currY = 0;

//			int Radius = Dimensions.SMILEY_RADIUS;
//			int eyesRadius = Radius / 10;

//			int it = 0;

//			// smiley base (circle): x^2 + y^2 = Radius^2
//			// eyes: (x - |r/2|)^2 + (y + r/2)^2 = eyesRadius^2
//			// smile:
//			// (x - h)^2/a^2 + (y - k)^2/b^2 = 1
//			// to avoid division to calculate more precisely, we should multiply formula by a^2*b^2:
//			// (x - h)^2*b^2 + (y - k)^2*a^2 = a^2*b^2
//			// y <= k

//			for (int row = Radius; row >= -Radius; row--)
//			{
//				for (int column = -Radius; column <= Radius; column++)
//				{

//					int eye1X = column + (Radius >> 1);
//					int eye2X = column - (Radius >> 1);
//					int eyesY = row - (Radius >> 1);

//					int smileOffset = -Radius * 3 / 8;
//					int smileY = row - smileOffset;
//					int smileWidth = Radius * 3 / 4;
//					int smileHeight = Radius >> 1;

//					Color color =
//							(int)Math.sqrt(row * row + column * column) <= Radius &&
//									!(eye1X * eye1X + eyesY * eyesY <= eyesRadius * eyesRadius ||
//											eye2X * eye2X + eyesY * eyesY <= eyesRadius * eyesRadius ||
//											(row <= smileOffset && column * column * smileHeight * smileHeight + smileY * smileY * smileWidth * smileWidth <= smileWidth * smileWidth * smileHeight * smileHeight))
//									?
//									Colors.COLOR_ART :
//									Colors.COLOR_NON_ART;
//					bricks[it++] = new Brick(color, currX, currY);
//					currX += (int)Dimensions.BRICK_WIDTH;
//				}
//				currX = 0;
//				currY += (int)Dimensions.BRICK_HEIGHT;
//			}
//	}
//}
