using JeremyAnsel.DirectX.D2D1;

namespace roser
{
	internal class CollisionHelper
	{
		public static bool TestPointRectColliding(float x, float y, D2D1RectF bounds)
		{
			return x >= bounds.Left &&
				x <= bounds.Right &&
				 y >= bounds.Top &&
				y <= bounds.Bottom;

		}

		public static bool TestPointRectColliding(float padding, float x, float y, D2D1RectF bounds)
		{
			return x >= (bounds.Left - padding) &&
				x <= (bounds.Right + padding) &&
				 y >= (bounds.Top - padding) &&
				y <= (bounds.Bottom + padding);

		}
	}
}
