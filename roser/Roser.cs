using roser;
using roser.native;
using roser.scenes;
using System.Runtime.InteropServices;

internal partial class Roser
{
	private static readonly SaveFile _saveFile = new();

	public static SaveFile SaveFile => _saveFile;

	[LibraryImport("Shcore.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetProcessDpiAwareness(ProcessDpiAwareness awareness);

	private static void Main(string[] args)
	{
		SetProcessDpiAwareness(ProcessDpiAwareness.ProcessPerMonitorDpiAware);
		using WindowManager wndManager = new();
		wndManager.SetScene<MenuScene>();
		wndManager.ShowWindow();
	}
}