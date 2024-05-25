using roser;
using roser.native;
using roser.scenes;
using System.Runtime.InteropServices;

internal partial class Roser
{
	[LibraryImport("Shcore.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetProcessDpiAwareness(ProcessDpiAwareness awareness);

	private static void Main(string[] args)
	{
#if DEBUG
		ConsoleListener.Start();
#endif
		SetProcessDpiAwareness(ProcessDpiAwareness.ProcessPerMonitorDpiAware);
		SaveFile.Init();
		using WindowManager wndManager = new();
		wndManager.SetScene<MenuScene>();
		wndManager.ShowWindow();
	}
}