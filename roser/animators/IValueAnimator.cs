namespace roser.animators
{
	internal interface IValueAnimator
	{
		void To(double to, int time);

		double Value { get; set; }

		bool IsFinished { get; }

		void OnTick(double dt);
	}
}
