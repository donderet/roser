using JeremyAnsel.DirectX.D2D1;

namespace roser
{
	internal class BrushWrapper(D2D1Brush brush, uint color)
	{
		public uint Color { get; set; } = color;

		public D2D1Brush Brush { get; set; } = brush;
	}
}