using System.Runtime.InteropServices;
using static roser.native.User32;
using static roser.native.DwmApi;
using roser.native;
using System.Diagnostics;
using roser.i18n;

namespace roser
{
	class WindowManager : IDisposable
	{
		public static double FrameTime { get; private set; }
		public static double TickTime { get; private set; }

		private const string WindowName = "2k25 game";

		private nint handle;

		private readonly Canvas canvas;

		private bool fullScreen = false;

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

			handle = CreateWindowExW(
				WsEx.OverlappedWindow,
				className,
				WindowName,
				WS.OverlappedWindow | WS.SysMenu | WS.Caption,
				CW_USEDEFAULT,
				CW_USEDEFAULT,
				1500,
				1500,
				nint.Zero,
				nint.Zero,
				nint.Zero,
				nint.Zero
			);
			int darkMode = 1;
			// Undocumented support since W10 20H1
			int hres = DwmSetWindowAttribute(handle, DwmWindowAttribute.UseImmersiveDarkMode, ref darkMode, 4);
			if (hres != 0)
				LogI(string.Format("DwmSetWindowAttribute failed: 0X{0:x}", hres));
			SetDisplayInfo();
			canvas = new Canvas(handle);
		}

		public void ShowWindow()
		{
			User32.ShowWindow(handle, SW.Normal);
			//SetWindowDisplayAffinity(handle, 0x00000000);
			MSG msg = new();
			while (msg.message != WM.Paint)
			{
				if (GetMessageW(out msg, nint.Zero, 0, 0) != 0)
				{
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
				Tick();
			}
		}

		public static readonly Stopwatch stopwatch = new();

		public void Tick()
		{
			long ticks = stopwatch.ElapsedTicks;
			if (ticks >= IPhysicsObject.targetTickTime)
			{
				canvas.CurrentScene?.OnTick(ticks / 10_000d);
				TickTime = (stopwatch.ElapsedTicks - ticks) / 10_000d;
				stopwatch.Restart();
				ticks = 0;
			}
			canvas.DrawCurrentScene();
			FrameTime = (stopwatch.ElapsedTicks - ticks) / 10_000d;
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
			canvas.InitScene(scene);
			canvas.CurrentScene?.Dispose();
			canvas.CurrentScene = scene;
		}

		public static WndProcDelegate? wndProcDelegate;

		private nint WndProc(nint hWnd, WM msg, nint wParam, nint lParam)
		{
			if (msg != WM.SetCursor && msg != WM.MouseFirst && msg != WM.MouseMove)
				LogI("" + msg);
			//if (msg == WM.SysCommand)
			//{
			//	LogI("Recieved syscom");
			//	LogI("wParam: " + wParam);
			//	LogI("lParam: " + lParam);
			//}
			switch (msg)
			{
				// Any titlebar click blocks the thread until the mouse button is released
				// EnterSizeMove has a huge delay, using SysCommand is a better option
				// Great API design by Microsoft, as always
				case WM.SysCommand:
					// Undocumented values (thanks, Microsoft):
					// 0xF012 - just title bar click
					// 0xF000 - right corner resize, but documented as resize ???
					// 0xF001 - left corner resize
					// 0xF006 - bottom corner resize
					//if (wParam != 0xF012 && wParam != 0xF010 && wParam != 0xF002 && wParam != 0xF000 && wParam != 0xF001 && wParam != 0xF006)
					//	break;
					break;
				case WM.Activate:
					canvas.SetFullScreen(fullScreen);
					break;
				case WM.ExitSizeMove:

					break;
				case WM.GetMinMaxInfo:
				case WM.NcCreate:
				case WM.NcCalcSize:
				case WM.ShowWindow:
				case WM.WindowPosChanging:
				case WM.WindowPosChanged:
				case WM.GetIcon:
				case WM.ImeSetContext:
				case WM.ImeNotify:
				case WM.SetFocus:
					break;
				case WM.Create:
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
					if (canvas == null || (canvas.GetWidth() == newWidth &&
						canvas.GetHeight() == newHeight))
						break;
					if (canvas.ResizeSwapChain())
						canvas.ResizeWindowSizeDependentResources();
					// Needed to free resources and keep the game updated
					Tick();
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
					if (msg == WM.KeyUp && wParam == 0x7A)
					{
						canvas.SetFullScreen(fullScreen = !fullScreen);
						return 0;
					}
					canvas?.OnMessage(msg, wParam, lParam);
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
