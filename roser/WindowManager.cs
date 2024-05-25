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
		public static ILanguage Language { get; private set; }

		public static double FrameTime { get; private set; }
		public static double TickTime { get; private set; }

		private const string WindowName = "2k25 game";

		private nint handle;

		private readonly Canvas canvas;

		private bool fullScreen = false;

		private bool minimized = false;

		private const WS defaultWindowStyle = WS.OverlappedWindow | WS.SysMenu | WS.Caption;

		public WindowManager()
		{
			Language = new UkrainianLanguage();

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
				defaultWindowStyle,
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
				LogI(string.Format("DwmSetWindowAttribute failed: 0x{0:x}", hres));
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
			if (minimized)
				return;
			canvas.DrawCurrentScene();
			FrameTime = (stopwatch.ElapsedTicks - ticks) / 10_000d;
		}

		public void SetScene<T>()
			where T : AbstractScene, new()
		{
			T scene = new()
			{
				WndManager = this,
			};
			canvas.InitScene(scene);
			canvas.CurrentScene?.Dispose();
			canvas.CurrentScene = scene;
		}

		public void SetScene(AbstractScene scene)
		{
			scene.WndManager = this;

			canvas.InitScene(scene);
			canvas.CurrentScene?.Dispose();
			canvas.CurrentScene = scene;
		}

		public static WndProcDelegate? wndProcDelegate;

		private nint WndProc(nint hWnd, WM msg, nint wParam, nint lParam)
		{
			//if (msg != WM.SetCursor && msg != WM.MouseFirst && msg != WM.MouseMove && msg != WM.NcHitTest && msg != WM.NcMouseMove)
			//	LogI("" + msg);
			//if (msg == WM.SysCommand)
			//{
			//	LogI("Recieved syscom");
			//	LogI("wParam: " + wParam);
			//	LogI("lParam: " + lParam);
			//}
			switch (msg)
			{
				// Any titlebar click blocks the thread until the mouse button is released
				// EnterSizeMove has a huge delay, using SysCommand to alert app that it is moving is a better option but anyway two rendering threads cause lag
				// Great API design by Microsoft, as always
				// TODO: just pause the game when resizing/moving
				case WM.SysCommand:
					// Undocumented values (thanks, Microsoft):
					// 0xF012 - just title bar click
					// 0xF000 - right corner resize, but documented as resize ???
					// 0xF001 - left corner resize
					// 0xF006 - bottom corner resize
					//if (wParam != 0xF012 && wParam != 0xF010 && wParam != 0xF002 && wParam != 0xF000 && wParam != 0xF001 && wParam != 0xF006)
					//	break;
					if (wParam == 0xF020)
					{
						minimized = true;
						LogI("Window is minimized");
					}
					//else if (wParam == 0xF120)
					//{
					//	LogI("Window is restored");
					//	minimized = false;
					//}
					break;
				case WM.KillFocus:
					if (fullScreen)
						SendMessageW(hWnd, WM.SysCommand, 0xF020, 0);
					canvas?.OnMessage(msg, wParam, lParam);
					break;
				case WM.Activate:
					// if deactivated
					if ((wParam & 0xFFFF) == 0)
					{
						minimized = true;
						break;
					}
					minimized = false;
					// Doesn't work in some cases 😭
					canvas.SetFullScreen(fullScreen);
					// The game refuses to draw frames without resizing swapchain and resources in full-screen mode
					// ¯\_(ツ)_/¯
					if (canvas.ResizeSwapChain())
						canvas.ResizeWindowSizeDependentResources();
					break;
				// Handle setup with multiple monitors
				case WM.DpiChanged:
					int oldDpi = DisplayInfo.Dpi;
					SetDisplayInfo();
					if (SaveFile.IsFullscreen)
						break;
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
					// SIZE_MINIMIZED
					if (wParam == 1)
						break;
					int newWidth = (int)lParam & 0xffff;
					int newHeight = (int)(lParam >> 16) & 0xffff;
					if (canvas == null || (canvas.GetWidth() == newWidth &&
						canvas.GetHeight() == newHeight))
						break;
					if (canvas.ResizeSwapChain())
						canvas.ResizeWindowSizeDependentResources();
					// Needed to free resources
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
						fullScreen = !fullScreen;
						canvas.SetFullScreen(fullScreen);
						// For some reason window won't have caption back when switching to windowed mode ???
						if (!fullScreen)
						{
							ulong dwStyle = GetWindowLongPtrW(hWnd, -16);
							SetWindowLongPtrW(hWnd, -16, (nint)(dwStyle | (ulong)defaultWindowStyle));
							const uint applyStyleFlags = 0x0002 | 0x0004 | 0x0200 | 0x0020 | 0x0001;
							SetWindowPos(handle, 0, 0, 0, 0, 0, applyStyleFlags);
						}
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
