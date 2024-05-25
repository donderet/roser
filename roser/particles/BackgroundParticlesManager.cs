using JeremyAnsel.DirectX.D2D1;
using roser.animators;

namespace roser.particles
{
	internal class BackgroundParticlesManager
	{
		readonly Random r = new();

		ValueAnimator _valueAnimator;

		const int generateChance = (int)(10000 * (IPhysicsObject.targetTickrate / 128f));
		const int maxParticlesCount = 30;

		public uint Width { get; set; }
		public uint Height { get; set; }

		private double accumulator = 0d;

		public List<CircleParticle> particles = new(maxParticlesCount);

		// Clean up if device is lost
		public void CreateResources(D2D1RenderTarget renderTarget)
		{
			foreach (var particle in particles)
				particle.CreateResources(renderTarget);
		}

		public void OnTick(double dt)
		{
			accumulator += dt;
			while (accumulator >= IPhysicsObject.targetTickTimeMillis)
			{
				accumulator -= IPhysicsObject.targetTickTimeMillis;
				for (int i = 0; i < particles.Count; i++)
				{
					particles[i].OnTick(r, Width, Height);
				}
				if (Height > 90 && (r.Next() & generateChance) == generateChance && particles.Count < maxParticlesCount)
				{
					float x = r.NextSingle() * Width;
					float y = r.NextSingle() * Height;

					double finalSize = r.Next(5, 20);
					int time = r.Next(100, 4000);
					IValueAnimator sizeAnimator = new ValueAnimator(0, finalSize, time);
					IValueAnimator xAnimator = new ValueAnimator(x, x, 100);
					IValueAnimator yAnimator = new ValueAnimator(y, y, 100);

					CircleParticle circleParticle = new((uint)r.Next(), sizeAnimator, xAnimator, yAnimator);
					particles.Add(circleParticle);
				}
			}
		}

		public void Render(D2D1RenderTarget renderTarget)
		{
			foreach (var particle in particles)
				particle.Render(renderTarget);
		}
	}
}
