namespace roser.animators
{
	internal class LinearValueAnimator(double from, double to, int time) : IValueAnimator
	{
		protected double _time = time;

		protected double _to = to;

		protected double v = (to - from) / time;

		public double Value { get; set; } = from;

		public double TimeLeft { get; private set; } = time;

		public void To(double to, int time)
		{
			v = (to - Value) / time;
			_to = to;
			TimeLeft = time;
			_time = time;
		}

		public bool IsFinished => _time != -1 && TimeLeft <= 0;

		public virtual void OnTick(double dt)
		{
			if (TimeLeft <= 0)
				return;
			TimeLeft -= dt;
			Value += dt * v;
			if (TimeLeft <= 0)
			{
				Value = _to;
			}
		}
	}
}
