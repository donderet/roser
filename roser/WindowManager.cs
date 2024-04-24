using System.Runtime.InteropServices;
using static roser.native.User32;
using roser.native;
using System.Diagnostics;
using roser.i18n;

namespace roser
{
	class WindowManager : IDisposable
	{
		private const string WindowName = "2k25 game";

		private nint handle;

		private readonly Canvas canvas;

		public WindowManager()
		{
			const string className = "Window";

			wndProcDelegate = WndProc;

			WNDCLASSEX windowClass = new()
			{
				lpszClassName = className,
				lpfnWndProc = Marshal.GetFunctionPointerForDelegate(wndProcDelegate),
				hCursor = LoadCursorW(nint.Zero, IDC.Arrow),
			};

			if (RegisterClassW(in windowClass) == 0)
			{
				throw new Exception("Could not register window class. Error code: " + Marshal.GetLastWin32Error());
			}
			Process.GetCurrentProcess().ProcessorAffinity = 1;

			handle = CreateWindowExW(
				WsEx.OverlappedWindow,
				className,
				WindowName,
				WS.OverlappedWindow | WS.SysMenu | WS.Caption,
				CW_USEDEFAULT,
				CW_USEDEFAULT,
				CW_USEDEFAULT,
				CW_USEDEFAULT,
				nint.Zero,
				nint.Zero,
				nint.Zero,
				nint.Zero
			);
			canvas = new Canvas(handle);
		}

		public void ShowWindow()
		{
			Stopwatch stopwatch = new();
			User32.ShowWindow(handle, SW.Normal);
			//SetWindowDisplayAffinity(handle, 0x00000000);
			MSG msg = new();
			while (msg.message != WM.Paint)
			{
				int result;
				if ((result = GetMessageW(out msg, nint.Zero, 0, 0)) != 0)
				{
					if (result == -1)
						throw new Exception(
							"Failed to get message. Error code " +
								Marshal.GetLastWin32Error()
							);
					TranslateMessage(ref msg);
					DispatchMessageW(ref msg);
				}
			}
			stopwatch.Start();
			while (msg.message != WM.Quit)
			{
				if (PeekMessageW(out msg, nint.Zero, 0, 0, PM.Remove) != 0)
				{
					TranslateMessage(ref msg);
					DispatchMessageW(ref msg);
				}
				else
				{
					const long timerTicksPerMillisecond = 10000;
					// 78125 ticks
					// 7,8125 millis
					const long targetTickRate = 1000 * timerTicksPerMillisecond / 128;
					long ticks = stopwatch.ElapsedTicks;
					if (ticks > targetTickRate)
					{
						canvas.CurrentScene?.OnTick((int)(ticks / timerTicksPerMillisecond));
						stopwatch.Restart();
					}
					canvas.DrawCurrentScene();
				}
			}
		}

		public void SetScene<T>()
			where T : AbstractScene, new()
		{
			T scene = new()
			{
				Language = new UkrainianLanguage(),
				WndManager = this,
			};
			//GetClientRect(handle, out var bounds);
			canvas.CurrentScene?.Dispose();
			canvas.CurrentScene = scene;
		}

		public static WndProcDelegate? wndProcDelegate;

		private nint WndProc(nint hWnd, WM msg, nint wParam, nint lParam)
		{
			//if (msg != WM.NCHITTEST && msg != WM.SETCURSOR && msg != WM.MOUSEFIRST && msg != WM.MOUSEMOVE)
			//Console.WriteLine("Got message: " + msg);
			switch (msg)
			{
				case WM.GetMinMaxInfo:
				case WM.NcCreate:
				case WM.NcCalcSize:
				case WM.ShowWindow:
				case WM.WindowPosChanging:
				case WM.WindowPosChanged:
				case WM.ActivatApp:
				case WM.Activate:
				case WM.NcActivate:
				case WM.GetIcon:
				case WM.ImeSetContext:
				case WM.ImeNotify:
				case WM.SetFocus:
					break;
				case WM.Create:
					SetDisplayInfo();
					break;
				// Handle setup with multiple monitors
				case WM.DpiChanged:
					if (Roser.SaveFile.IsFullscreen)
						break;
					int oldDpi = DisplayInfo.Dpi;
					SetDisplayInfo();
					double scaleFactor = DisplayInfo.Dpi / oldDpi;
					SetWindowPos(
						hWnd,
						nint.Zero,
						0,
						0,
						(int)(canvas.GetWidth() * scaleFactor),
						(int)(canvas.GetHeight() * scaleFactor),
						0x0202
					);
					break;
				// Ignore resize when minimizing window and maximizing previously minimized window
				case WM.Size:
					if (wParam == 1) // SIZE_MINIMIZED
						break;
					int newWidth = (int)lParam & 0xffff;
					int newHeight = (int)(lParam >> 16) & 0xffff;
					// SIZE_MAXIMIZED
					if (wParam == 2 && canvas.GetWidth() == newWidth &&
					canvas.GetHeight() == newHeight)
						break;
					if (canvas.ResizeSwapChain())
						canvas.ResizeWindowSizeDependentResources();
					break;
				case WM.EraseBkgnd:
					return 1;
				case WM.Paint:
					BeginPaint(hWnd, out var paint);
					EndPaint(hWnd, ref paint);
					return nint.Zero;
				case WM.Destroy:
					PostQuitMessage(0);
					return nint.Zero;
				default:
					canvas.OnMessage(msg, wParam, lParam);
					break;
			}

			return DefWindowProcW(hWnd, msg, wParam, lParam);
		}

		private void SetDisplayInfo()
		{
			DisplayInfo.Dpi = GetDpiForWindow(handle);
			DisplayInfo.DipScale = DisplayInfo.Dpi / 96f;
		}

		private bool disposed;

		~WindowManager()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing)
			{
				// Dispose managed state (managed objects)
				canvas.Dispose();
			}

			// Dispose unmanaged resources
			if (handle != nint.Zero)
			{
				DestroyWindow(handle);
				handle = nint.Zero;
			}
			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
