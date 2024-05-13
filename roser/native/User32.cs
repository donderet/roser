using System.Drawing;
using System.Runtime.InteropServices;

namespace roser.native
{
	internal partial class User32
	{
		public const int CW_USEDEFAULT = unchecked((int)0x80000000);

		public const int ErrorClassAlreadyExists = 1410;

		public struct WNDCLASSEX
		{
			public uint cbSize;
			public uint style;
			public IntPtr lpfnWndProc;
			public int cbClsExtra;
			public int cbWndExtra;
			public IntPtr hInstance;
			public IntPtr hIcon;
			public IntPtr hCursor;
			public IntPtr hbrBackground;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszMenuName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszClassName;
			public IntPtr hIconSm;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PAINTSTRUCT
		{
			public IntPtr hdc;
			public bool fErase;
			public Rectangle rcPaint;
			public bool fRestore;
			public bool fIncUpdate;
			public uint color;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct MSG
		{
			public IntPtr hwnd;
			public WM message;
			public UIntPtr wParam;
			public UIntPtr lParam;
			public uint time;
			public Point pt;
		}

		public delegate IntPtr WndProcDelegate(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool InvalidateRect(IntPtr hWnd, IntPtr rect, [MarshalAs(UnmanagedType.Bool)] bool bErase);

		[LibraryImport("usеr32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool SetWindowDisplayAffinity(nint hwnd, uint dwAffinity);	

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool SetWindowLongPtrW(nint hwnd, int nIndex, nint dwNewLong);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct MonitorInfoExW
		{
			public int cbSize;
			public Rectangle rcMonitor;
			public Rectangle rcWork;
			public int dwFlags;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string szDevice;
		}

		[DllImport("user32.dll")]
		public static extern int GetMonitorInfoW(nint hMonitor, out MonitorInfoExW lpmi);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.U8)]
		public static partial ulong MonitorFromWindow(nint hwnd, int dwFlags);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.U8)]
		public static partial ulong GetWindowLongPtrW(nint hwnd, int nIndex);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool UpdateWindow(IntPtr hWnd);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool ShowWindow(IntPtr hWnd, SW nCmdShow);

		[LibraryImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool DestroyWindow(IntPtr hWnd);

		[LibraryImport("user32.dll", SetLastError = true)]
		public static partial IntPtr CreateWindowExW(
			WsEx dwExStyle,
			[MarshalAs(UnmanagedType.LPWStr)]
			string lpClassName,
			[MarshalAs(UnmanagedType.LPWStr)]
			string lpWindowName,
			WS dwStyle,
			int x,
			int y,
			int nWidth,
			int nHeight,
			IntPtr hWndParent,
			IntPtr hMenu,
			IntPtr hInstance,
			IntPtr lpParam
		);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern UInt16 RegisterClassW(in WNDCLASSEX lpWndClass);

		[LibraryImport("user32.dll")]
		public static partial IntPtr DefWindowProcW(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

		[DllImport("user32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

		[DllImport("user32.dll")]
		public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

		[LibraryImport("user32.dll")]
		public static partial void PostQuitMessage(int nExitCode);

		[LibraryImport("user32.dll")]
		public static partial void SendMessageW(nint hwnd, WM msg, nint wParam, nint lParam);

		[LibraryImport("user32.dll")]
		public static partial void PostMessageW(nint hwnd, WM msg, nint wParam, nint lParam);

		[LibraryImport("user32.dll")]
		public static partial IntPtr LoadIconW(
			IntPtr hInstance,
			[MarshalAs(UnmanagedType.LPWStr)]
			string lpIconName
		);

		[DllImport("user32.dll")]
		public static extern IntPtr DispatchMessageW(ref MSG lpmsg);

		[DllImport("user32.dll")]
		public static extern bool TranslateMessage(ref MSG lpMsg);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetMessageW(
			out MSG lpMsg,
			IntPtr hWnd,
			uint wMsgFilterMin,
			uint wMsgFilterMax
		);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int PeekMessageW(
			out MSG lpMsg,
			IntPtr hWnd,
			uint wMsgFilterMin,
			uint wMsgFilterMax,
			PM wRemoveMsg
		);

		[LibraryImport("user32.dll")]
		public static partial IntPtr GetDC(IntPtr ptr);

		[LibraryImport("user32.dll")]
		public static partial IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

		[LibraryImport("user32.dll")]
		public static partial IntPtr LoadCursorW(
			IntPtr hInstance,
			IDC lpCursorName
		);

		[LibraryImport("User32", SetLastError = true)]
		public static partial int GetGuiResources(IntPtr hProcess, int uiFlags);

		[LibraryImport("User32", SetLastError = true)]
		public static partial int SetWindowPos(nint hwnd, nint hwndInsertAfter, int x, int y, int cx, int cy, uint flags);

		[LibraryImport("User32", SetLastError = true)]
		public static partial int GetDpiForWindow(nint hwnd);
	}
}
