using System.Runtime.InteropServices;

namespace roser.native
{
	internal partial class GDI32
	{
		public const int SRCCOPY = 0xCC0020;

		public const int LOGPIXELSX = 88;
		public const int LOGPIXELSY = 90;
		public const int HORZRES = 8;
		public const int VERTRES = 10;

		[LibraryImport("gdi32.dll")]
		public static partial IntPtr CreateCompatibleDC(IntPtr hdc);

		[LibraryImport("gdi32.dll")]
		public static partial IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int height);

		[LibraryImport("gdi32.dll")]
		public static partial IntPtr GetWindowDC(Int32 ptr);

		[LibraryImport("gdi32.dll")]
		public static partial int DeleteDC(nint hdc);

		[LibraryImport("gdi32.dll")]
		public static partial IntPtr GetStockObject(StockObject obj);

		[LibraryImport("gdi32.dll")]
		public static partial IntPtr SelectObject(IntPtr hdc, IntPtr hgdiObj);

		[LibraryImport("gdi32.dll")]
		public static partial IntPtr DeleteObject(IntPtr handle);

		[LibraryImport("gdi32.dll")]
		public static partial IntPtr BitBlt(
			IntPtr hdcDest,
			int xDest,
			int yDest,
			int width,
			int height,
			IntPtr hdcSrc,
			int xSrc,
			int ySrc,
			int dwRop
		);

		[LibraryImport("gdi32.dll")]
		public static partial int GetDeviceCaps(IntPtr hdc, int nIndex);
	}
}
