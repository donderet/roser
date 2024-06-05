using System.Drawing;
using System.Runtime.InteropServices;

namespace roser.native
{
	internal partial class User32
	{
		public const int CwUseDefault = unchecked((int)0x80000000);

		public const int ErrorClassAlreadyExists = 1410;

		public struct WNDCLASSEX
		{
			public uint cbSize;
			public uint style;
			public nint lpfnWndProc;
			public int cbClsExtra;
			public int cbWndExtra;
			public nint hInstance;
			public nint hIcon;
			public nint hCursor;
			public nint hbrBackground;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszMenuName;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszClassName;
			public nint hIconSm;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PAINTSTRUCT
		{
			public nint hdc;
			public bool fErase;
			public Rectangle rcPaint;
			public bool fRestore;
			public bool fIncUpdate;
			public uint color;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct MSG
		{
			public nint hwnd;
			public WM message;
			public UIntPtr wParam;
			public UIntPtr lParam;
			public uint time;
			public Point pt;
		}

		public delegate nint WndProcDelegate(nint hWnd, WM msg, nint wParam, nint lParam);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool SetWindowLongPtrW(nint hwnd, int nIndex, nint dwNewLong);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.U8)]
		public static partial ulong GetWindowLongPtrW(nint hwnd, int nIndex);

		[LibraryImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool ShowWindow(nint hWnd, SW nCmdShow);

		[LibraryImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static partial bool DestroyWindow(nint hWnd);

		[LibraryImport("user32.dll", SetLastError = true)]
		public static partial nint CreateWindowExW(
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
			nint hWndParent,
			nint hMenu,
			nint hInstance,
			nint lpParam
		);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern ushort RegisterClassW(in WNDCLASSEX lpWndClass);

		[LibraryImport("user32.dll")]
		public static partial nint DefWindowProcW(nint hWnd, WM uMsg, nint wParam, nint lParam);

		[DllImport("user32.dll")]
		public static extern nint BeginPaint(nint hwnd, out PAINTSTRUCT lpPaint);

		[DllImport("user32.dll")]
		public static extern bool GetClientRect(nint hWnd, out Rectangle lpRect);

		[DllImport("user32.dll")]
		public static extern bool EndPaint(nint hWnd, ref PAINTSTRUCT lpPaint);

		[LibraryImport("user32.dll")]
		public static partial void PostQuitMessage(int nExitCode);

		[LibraryImport("user32.dll")]
		public static partial void SendMessageW(nint hwnd, WM msg, nint wParam, nint lParam);

		[LibraryImport("user32.dll")]
		public static partial void PostMessageW(nint hwnd, WM msg, nint wParam, nint lParam);

		[LibraryImport("user32.dll")]
		public static partial nint LoadIconW(
			nint hInstance,
			[MarshalAs(UnmanagedType.LPWStr)]
			string lpIconName
		);

		[DllImport("user32.dll")]
		public static extern nint DispatchMessageW(ref MSG lpmsg);

		[DllImport("user32.dll")]
		public static extern bool TranslateMessage(ref MSG lpMsg);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetMessageW(
			out MSG lpMsg,
			nint hWnd,
			uint wMsgFilterMin,
			uint wMsgFilterMax
		);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern int PeekMessageW(
			out MSG lpMsg,
			nint hWnd,
			uint wMsgFilterMin,
			uint wMsgFilterMax,
			PM wRemoveMsg
		);

		[LibraryImport("user32.dll")]
		public static partial nint LoadCursorW(
			nint hInstance,
			IDC lpCursorName
		);

		[LibraryImport("User32", SetLastError = true)]
		public static partial int SetWindowPos(nint hwnd, nint hwndInsertAfter, int x, int y, int cx, int cy, uint flags);

		[LibraryImport("User32", SetLastError = true)]
		public static partial int GetDpiForWindow(nint hwnd);
	}
}
