using JeremyAnsel.DirectX.D2D1;
using roser.animators;

namespace roser.particles
{
	internal class BackgroundParticlesManager : IDisposable
	{
		public bool AnimateSize { get; set; }
		public bool AnimateMovement { get; set; }
		public bool AnimateColor { get; set; }

		readonly Random r = new();

		const int genParticleTime = 500;
		const int maxParticlesCount = 30;

		public uint Width { get; set; }
		public uint Height { get; set; }

		private double accumulator = 0d;

		public List<CircleParticle> particles = new(maxParticlesCount);
		private bool disposedValue;

		// Clean up if device is lost
		public void CreateResources(D2D1RenderTarget renderTarget)
		{
			foreach (var particle in particles)
				particle.CreateResources(renderTarget);
		}

		double lastParticleTime;

		public void OnTick(double dt)
		{
			accumulator += dt;
			lastParticleTime += dt;
			while (accumulator >= IPhysicsObject.targetTickTimeMillis)
			{
				accumulator -= IPhysicsObject.targetTickTimeMillis;
				for (int i = 0; i < particles.Count; i++)
				{
					particles[i].OnTick(r, Width, Height);
				}
				if (Height > 90 && (r.Next() & 0b1111) == 0b1111 && lastParticleTime >= genParticleTime && particles.Count < maxParticlesCount)
				{
					lastParticleTime -= genParticleTime;
					float x = r.NextSingle() * Width;
					float y = r.NextSingle() * Height;

					int time = AnimateSize ? 200 : -1;
					double finalSize = r.Next(5, 20);
					double startSize = AnimateSize ? 0 : finalSize;
					IValueAnimator sizeAnimator = new ValueAnimator(startSize, finalSize, time);
					time = AnimateMovement ? 0 : -1;
					IValueAnimator xAnimator = new ValueAnimator(x, x, time);
					IValueAnimator yAnimator = new ValueAnimator(y, y, time);
					time = AnimateColor ? 0 : -1;
					IValueAnimator colorAnimator = new LinearValueAnimator((uint)r.Next(), (uint)r.Next(), time);

					CircleParticle circleParticle = new(sizeAnimator, xAnimator, yAnimator, colorAnimator);
					particles.Add(circleParticle);
				}
			}
		}

		public void Render(D2D1RenderTarget renderTarget)
		{
			foreach (var particle in particles)
				particle.Render(renderTarget);
		}

		public void ReleaseParticles()
		{
			foreach (var particle in particles)
				particle.Dispose();
			particles.Clear();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					ReleaseParticles();
				}
				disposedValue = true;
			}
		}

		~BackgroundParticlesManager()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
