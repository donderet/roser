namespace roser
{
    internal interface IPhysicsObject
    {
		const short targetTickrate = 128;
		const double targetTickTime = 10000_000d / targetTickrate;
		const double targetTickTimeMillis = 1d / targetTickrate;

		void OnTick(double dt);
    }
}
