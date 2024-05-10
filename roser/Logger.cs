global using static roser.Logger;

namespace roser
{
	internal class Logger
	{
		//public static void LogF(string s)
		//{
		//	Console.ForegroundColor = ConsoleColor.Magenta;
		//	Console.Write(s);
		//	User32.PostQuitMessage(0);
		//}

		//public static void LogF(string s, int hres)
		//{
		//	Console.ForegroundColor = ConsoleColor.Magenta;
		//	Console.Write(s);
		//	Console.Write("\nHRES: ");
		//	Console.WriteLine(hres);
		//	Console.ResetColor();
		//	User32.PostQuitMessage(hres);
		//}

		public static void LogE(string s, int hres)
		{
			LogE(s, hres);
		}

		public static void LogE(string s)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(s);
			Console.ResetColor();
		}

		public static void LogW(string s)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(s);
			Console.ResetColor();
		}

		public static void LogI(string s)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(s);
			Console.ResetColor();
		}
	}
}
