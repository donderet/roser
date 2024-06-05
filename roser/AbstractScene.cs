using roser.native;
using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;

namespace roser
{
	abstract class AbstractScene : IDisposable, IPhysicsObject
	{
		public WindowManager WndManager { get; set; }

		public uint SmallerDimension { get; set; }

		public uint Width { get; set; }

		public uint Height { get; set; }

		public abstract void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory);

		public abstract void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory);

		public abstract void Render(D2D1RenderTarget renderTarget);

		protected float Percent(float percent)
		{
			return percent * SmallerDimension / 100;
		}

		public abstract void OnTick(double dt);

		protected virtual void DisposeScene()
		{

		}

		public virtual void OnMessage(WM msg, nint wParam, nint lParam)
		{
			switch (msg)
			{
				case WM.LButtonDown:
					{
						float x = (float)((lParam & 0xffff) / DisplayInfo.DipScale);
						float y = (float)(((lParam >> 16) & 0xffff) / DisplayInfo.DipScale);
						OnLMBDown(x, y);
						return;
					}
				case WM.LButtonUp:
					{
						float x = (float)((lParam & 0xffff) / DisplayInfo.DipScale);
						float y = (float)(((lParam >> 16) & 0xffff) / DisplayInfo.DipScale);
						OnLMBUp(x, y);
						return;
					}
				case WM.RButtonDown:
					{
						float x = (float)((lParam & 0xffff) / DisplayInfo.DipScale);
						float y = (float)(((lParam >> 16) & 0xffff) / DisplayInfo.DipScale);
						OnRMBDown(x, y);
						return;
					}
				case WM.RButtonUp:
					{
						float x = (float)((lParam & 0xffff) / DisplayInfo.DipScale);
						float y = (float)(((lParam >> 16) & 0xffff) / DisplayInfo.DipScale);
						OnRMBUp(x, y);
						return;
					}
				case WM.KeyDown:
					OnKeyDown((VK)wParam);
					return;
				case WM.KeyUp:
					OnKeyUp((VK)wParam);
					return;
			}
		}

		/**
		 * Called when left button is clicked on the mouse
		 * <param name="x">X position of the mouse, in DIPs</param>
		 * <param name="y">Y position of the mouse, in DIPs</param>
		 */
		public virtual void OnLMBDown(float x, float y)
		{

		}

		/**
		 * Called when left button is released on the mouse
		 * <param name="x">X position of the mouse, in DIPs</param>
		 * <param name="y">Y position of the mouse, in DIPs</param>
		 */
		public virtual void OnLMBUp(float x, float y)
		{

		}

		/**
		 * Called when right button is clicked on the mouse
		 * <param name="x">X position of the mouse, in DIPs</param>
		 * <param name="y">Y position of the mouse, in DIPs</param>
		 */
		public virtual void OnRMBDown(float x, float y)
		{

		}

		/**
		 * Called when right button is released on the mouse
		 * <param name="x">X position of the mouse, in DIPs</param>
		 * <param name="y">Y position of the mouse, in DIPs</param>
		 */
		public virtual void OnRMBUp(float x, float y)
		{

		}

		/**
		 * Called when button is pressed on the keyboard
		 * <param name="vk">virtual key code</param>
		 */
		public virtual void OnKeyDown(VK vk)
		{

		}

		/**
		 * Called when button is released on the keyboard
		 * <param name="vk">virtual key code</param>
		 */
		public virtual void OnKeyUp(VK vk)
		{

		}

		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					DisposeScene();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~AbstractScene()
		{
			Dispose(false);
		}
	}
}
