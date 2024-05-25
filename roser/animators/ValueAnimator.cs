namespace roser.animators
{
	internal class ValueAnimator(double from, double to, int time) : IValueAnimator
	{
		protected double _to = to;

		public void To(double __to, int __time)
		{
			a = 2 * (__to - Value) / (__time * __time);
			v = 0;
			accumulator = 0;
			_to = __to;
			TimeLeft = __time;
		}

		public double TimeLeft { get; set; } = time;

		protected double a = 2 * (to - from) / (time * time);
		protected double v = 0;

		public double Value { get; set; } = from;

		private double accumulator = 0d;

		public bool IsFinished => TimeLeft <= 0;

		public virtual void OnTick(double dt)
		{
			if (TimeLeft <= 0)
				return;

			TimeLeft -= dt;
			accumulator += dt;
			while (accumulator >= IPhysicsObject.targetTickTimeMillis)
			{
				accumulator -= IPhysicsObject.targetTickTimeMillis;
				v += IPhysicsObject.targetTickTimeMillis * a;
				Value += IPhysicsObject.targetTickTimeMillis * v;
			}
			if (TimeLeft <= 0)
			{
				Value = _to;
			}
		}
	}
}
