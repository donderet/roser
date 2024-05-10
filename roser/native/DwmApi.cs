using System.Runtime.InteropServices;

namespace roser.native
{
    internal partial class DwmApi
	{
		[LibraryImport("dwmapi.dll")]
		public static partial int DwmSetWindowAttribute(nint hwnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);
	}
}
