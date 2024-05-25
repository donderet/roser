using JeremyAnsel.DirectX.D2D1;
using roser.animators;
using Windows.UI.Composition;

namespace roser.particles
{
	internal class BackgroundParticlesManager
	{
		public bool AnimateSize { get; set; }
		public bool AnimateMovement { get; set; }
		public bool AnimateColor { get; set; }

		readonly Random r = new();

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

					int time = AnimateSize ? 100 : -1;
					double finalSize = r.Next(5, 20);
					IValueAnimator sizeAnimator = new ValueAnimator(finalSize, finalSize, time);
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
	}
}
