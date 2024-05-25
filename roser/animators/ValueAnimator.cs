namespace roser.animators
{
	internal class ValueAnimator(double from, double to, int time) : IValueAnimator
	{
		protected double _time = time;

		protected double _to = to;

		public void To(double to, int time)
		{
			a = 2 * (to - Value) / (time * time);
			v = 0;
			accumulator = 0;
			_to = to;
			TimeLeft = time;
			_time = time;
		}

		public double TimeLeft { get; set; } = time;

		protected double a = 2 * (to - from) / (time * time);
		protected double v = 0;

		public double Value { get; set; } = from;

		private double accumulator = 0d;

		public bool IsFinished => _time != -1 && TimeLeft <= 0;

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
