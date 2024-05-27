namespace roser
{
	internal static class ConsoleListener
	{
		static bool stop = false;

		public static void Start()
		{
			new Thread(() =>
			{
				while (true)
				{
					if (stop)
						break;
					if (!Console.KeyAvailable)
					{
						Thread.Sleep(100);
						continue;
					}
					ConsoleKey key = Console.ReadKey(true).Key;
					if (Console.ReadKey().Key == ConsoleKey.F2)
					{
						Console.Clear();
					}
				}
			}).Start();
		}

		public static void Stop()
		{
			stop = true;
		}
	}
}
