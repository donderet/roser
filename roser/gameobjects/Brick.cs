namespace roser.gameobjects
{
	internal class Brick(uint color, float x, float y, float width, float height)
	{
		public uint Color { get; set; } = color;

		public float X { get; set; } = x;

		public float Y { get; set; } = y;

		public float Width { get; set; } = width;

		public float Height { get; set; } = height;
	}
}
