using JeremyAnsel.DirectX.D2D1;
using roser.animators;

namespace roser.particles
{
	internal class CircleParticle(IValueAnimator sizeAnimator, IValueAnimator xAnimator, IValueAnimator yAnimator, IValueAnimator colorAnimator) : IParticle
	{
		public IValueAnimator SizeAnimator { get; set; } = sizeAnimator;
		public IValueAnimator XAnimator { get; set; } = xAnimator;
		public IValueAnimator YAnimator { get; set; } = yAnimator;
		public IValueAnimator ColorAnimator { get; set; } = colorAnimator;

		public float X { get; set; }
		D2D1Ellipse circle = new(new((float)xAnimator.Value, (float)yAnimator.Value), 0, 0);
		D2D1SolidColorBrush? brush;

		public void CreateResources(D2D1RenderTarget renderTarget)
		{
			if (brush != null)
			{
				brush.Dispose();
				brush = null;
			}
			brush = renderTarget.CreateSolidColorBrush(new((uint)colorAnimator.Value));
		}

		public void Render(D2D1RenderTarget renderTarget)
		{
			if (brush == null)
				CreateResources(renderTarget);
			circle.RadiusX = circle.RadiusY = (float)SizeAnimator.Value;
			D2D1Point2F point = circle.Point;
			point.X = (float)XAnimator.Value;
			point.Y = (float)YAnimator.Value;
			circle.Point = point;
			brush.Color = new((uint)colorAnimator.Value);
			renderTarget.FillEllipse(circle, brush);
		}

		public void OnTick(Random r, uint width, uint height)
		{
			SizeAnimator.OnTick(IPhysicsObject.targetTickTimeMillis);
			XAnimator.OnTick(IPhysicsObject.targetTickTimeMillis);
			YAnimator.OnTick(IPhysicsObject.targetTickTimeMillis);
			ColorAnimator.OnTick(IPhysicsObject.targetTickTimeMillis);

			if (SizeAnimator.IsFinished)
			{
				double finalSize = r.Next(5, 20);
				int time = r.Next(100, 2000);
				SizeAnimator.To(finalSize, time);
			}
			if (XAnimator.IsFinished)
			{
				double finalX = r.NextSingle() * width;
				int time = r.Next(100, 5000);
				XAnimator.To(finalX, time);
			}
			if (YAnimator.IsFinished)
			{
				double finalY = r.NextSingle() * height;
				int time = r.Next(100, 500);
				YAnimator.To(finalY, time);
			}
			if (ColorAnimator.IsFinished)
			{
				int target = r.Next();
				int time = r.Next(10000, 30000);
				ColorAnimator.To(target, time);
			}
		}

		private bool disposedValue;
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				brush?.Dispose();
				brush = null;
				disposedValue = true;
			}
		}

		~CircleParticle()
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
