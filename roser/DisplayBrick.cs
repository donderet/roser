using JeremyAnsel.DirectX.D2D1;
using roser.gameobjects;

namespace roser
{
	internal class DisplayBrick(D2D1Brush brush, Arena arena, D2D1RenderTarget renderTarget)
	{
		public D2D1Brush Brush { get; set; } = brush;

		public D2D1RoundedRect RoundedRect { get; set; }

	}
}
