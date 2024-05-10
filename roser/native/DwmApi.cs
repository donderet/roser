using System.Runtime.InteropServices;

namespace roser.native
{
    internal class DwmApi
	{
		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);

	}
}
