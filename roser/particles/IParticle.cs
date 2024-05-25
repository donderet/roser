using JeremyAnsel.DirectX.D2D1;

namespace roser.particles
{
	internal interface IParticle : IDisposable
	{
		void Render(D2D1RenderTarget renderTarget);

		void CreateResources(D2D1RenderTarget renderTarget);

		// It is on the manager to time calls of this function
		void OnTick(Random r, uint width, uint height);
	}
}
