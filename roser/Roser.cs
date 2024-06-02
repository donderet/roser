using Microsoft.Win32.SafeHandles;
using roser;
using roser.native;
using roser.scenes;
using System.Runtime.InteropServices;
using System.Text;

internal partial class Roser
{
	[LibraryImport("Shcore.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetProcessDpiAwareness(ProcessDpiAwareness awareness);

#if DEBUG
	[LibraryImport("kernel32.dll", SetLastError = true)]
	public static partial IntPtr GetStdHandle(int nStdHandle);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	public static partial int AllocConsole();
#endif

	private static void Main(string[] args)
	{
#if DEBUG
		AllocConsole();
		IntPtr stdHandle = GetStdHandle(-11);
		SafeFileHandle safeFileHandle = new(stdHandle, true);
		FileStream fileStream = new(safeFileHandle, FileAccess.Write);
		StreamWriter standardOutput = new(fileStream)
		{
			AutoFlush = true
		};
		Console.SetOut(standardOutput);

		ConsoleListener.Start();
#endif
		SetProcessDpiAwareness(ProcessDpiAwareness.ProcessPerMonitorDpiAware);
		SaveFile.Init();
		using WindowManager wndManager = new();
		wndManager.SetScene<MenuScene>();
		wndManager.ShowWindow();
#if DEBUG
		ConsoleListener.Stop();
#endif
	}
}