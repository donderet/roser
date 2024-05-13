namespace roser
{
	internal class ConsoleListener
	{
		public static void Start()
		{
			new Thread(() =>
			{
				while (true)
				{
					ConsoleKey key = Console.ReadKey(true).Key;
					if (Console.ReadKey().Key == ConsoleKey.F2)
					{
						Console.Clear();
					}
				}
			}).Start();
		}
	}
}
