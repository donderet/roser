using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;

namespace roser.scenes
{
	internal class GameScene : AbstractScene
	{
		public override void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{

		}
		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{

		}

		public override void Render(D2D1RenderTarget renderTarget)
		{
			renderTarget.Clear();
		}


		public override void OnTick(int deltaTime)
		{

		}
	}
}
