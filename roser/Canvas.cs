using static roser.native.User32;
using JeremyAnsel.DirectX.D3D11;
using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using JeremyAnsel.DirectX.Dxgi;
using roser.native;

namespace roser
{
	internal class Canvas : IDisposable
	{
		private readonly D2D1Factory d2dFactory;
		private readonly DWriteFactory dwritefactory;

		private const DxgiFormat format = DxgiFormat.B8G8R8A8UNorm;

		public nint Hwnd { get; set; }

		public AbstractScene? CurrentScene { get; set; }

		public uint GetWidth()
		{
			return bufferDesc.Width;
		}

		public uint GetHeight()
		{
			return bufferDesc.Height;
		}

		private D3D11Device device;
		private D3D11FeatureLevel featureLevel;
		private D3D11DeviceContext deviceContext;

		private void ReleaseDeviceResources()
		{
			D3D11Utils.DisposeAndNull(ref deviceContext);
			D3D11Utils.DisposeAndNull(ref device);
		}

		public Canvas(nint hwnd)
		{
			Hwnd = hwnd;
			d2dFactory = D2D1Factory.Create(D2D1FactoryType.SingleThreaded, D2D1DebugLevel.Information);
			dwritefactory = DWriteFactory.Create(DWriteFactoryType.Shared);

			CreateDeviceResources();
			CreateSwapChain();
		}

		private void CreateDeviceResources()
		{
			D3D11CreateDeviceOptions creationFlags =
#if DEBUG
				D3D11CreateDeviceOptions.Debug |
#endif
			D3D11CreateDeviceOptions.BgraSupport |
			D3D11CreateDeviceOptions.SingleThreaded;

			D3D11FeatureLevel[] featureLevels =
			{
				D3D11FeatureLevel.FeatureLevel111,
				D3D11FeatureLevel.FeatureLevel110,
				D3D11FeatureLevel.FeatureLevel101,
				D3D11FeatureLevel.FeatureLevel93,
				D3D11FeatureLevel.FeatureLevel92,
				D3D11FeatureLevel.FeatureLevel91,
			};

			try
			{
				D3D11Device.CreateDevice(
					null,
					D3D11DriverType.Hardware,
					creationFlags,
					featureLevels,
					out device,
					out featureLevel,
					out deviceContext
					);
			}
			catch
			{
				// Fallback to WARP
				D3D11Device.CreateDevice(
					null,
					D3D11DriverType.Warp,
					creationFlags,
					featureLevels,
					out device,
					out featureLevel,
					out deviceContext
				);
			}

			PrintAdapterInfo();
		}

		private void PrintAdapterInfo()
		{
			using var dxgiDevice = new DxgiDevice2(device.Handle);
			using var adapeter = dxgiDevice.GetAdapter();
			var desc = adapeter.Description;
			LogI($"Got adapter: {desc.AdapterDescription}\nVideo memory: {desc.DedicatedVideoMemory}\nSystem memory: {desc.DedicatedSystemMemory}");
		}

		private D3D11Viewport viewport;
		private D3D11Texture2DDesc bufferDesc;

		private DxgiSwapChain3 swapChain;
		private D3D11Texture2D backBuffer;
		private D2D1RenderTarget renderTarget;
		private D3D11RenderTargetView renderTargetView;

		private void ReleaseWindowSizeDependentResources()
		{
			if (deviceContext)
			{
				deviceContext.OutputMergerSetRenderTargets([null], null);
				deviceContext.ClearState();
			}

			D3D11Utils.DisposeAndNull(ref backBuffer);
			D3D11Utils.DisposeAndNull(ref renderTargetView);
			D2D1Utils.DisposeAndNull(ref renderTarget);

			//if (swapChain != null && swapChain.GetFullscreenState())
			//{
			//	swapChain.SetFullscreenState(false);
			//}
		}

		private void CreateSwapChain()
		{
			DxgiSwapChainDesc1 swapChainDesc = new()
			{
				Width = 0,
				Height = 0,
				Format = format,
				Stereo = false,
				SampleDescription = new DxgiSampleDesc(1, 0),
				BufferUsage = DxgiUsages.RenderTargetOutput,
				BufferCount = 3,
				Scaling = DxgiScaling.None,
				SwapEffect = DxgiSwapEffect.FlipSequential,
				AlphaMode = DxgiAlphaMode.Ignore,
				Options = (DxgiSwapChainOptions)2048,

			};
			DxgiSwapChainFullscreenDesc? swapChainFullscreenDesc = new()
			{
				IsWindowed = true,
				RefreshRate = new DxgiRational(),
				Scaling = DxgiModeScaling.Unspecified,
				ScanlineOrdering = DxgiModeScanlineOrder.Unspecified
			};
			using var dxgiDevice = new DxgiDevice3(device.Handle);
			using var dxgiAdapter = dxgiDevice.GetAdapter();
			using var dxgiFactory = dxgiAdapter.GetParent();
			try
			{
				swapChain = dxgiFactory.CreateSwapChainForWindowHandle(
					device.Handle,
					Hwnd,
					swapChainDesc,
					swapChainFullscreenDesc,
					null);
			}
			catch (Exception ex)
			{
				if (ex.HResult != DxgiError.InvalidCall)
					throw;
				swapChainDesc.SwapEffect = DxgiSwapEffect.Sequential;

				swapChain = dxgiFactory.CreateSwapChainForWindowHandle(
				device.Handle,
				Hwnd,
				swapChainDesc,
				swapChainFullscreenDesc,
				null);

				dxgiDevice.MaximumFrameLatency = 1;
			}

			dxgiFactory.MakeWindowAssociation(Hwnd, DxgiWindowAssociationOptions.NoAltEnter);
			if (Roser.SaveFile.IsFullscreen)
			{
				swapChain.SetFullscreenState(true);
			}

			backBuffer = swapChain.GetTexture2D(0);

			ResizeWindowSizeDependentResources();
		}

		public bool ResizeSwapChain()
		{
			ReleaseWindowSizeDependentResources();

			try
			{
				swapChain.ResizeBuffers(0, 0, 0, DxgiFormat.Unknown, (DxgiSwapChainOptions)2048);
				backBuffer = swapChain.GetTexture2D(0);
			}
			catch (Exception ex)
			{
				if (ex.HResult != DxgiError.DeviceRemoved && ex.HResult != DxgiError.DeviceReset)
					throw;
				OnDeviceLost();
				return false;
			}
			return true;
		}

		// We need to draw something on the buffer in order to actually *free* memory from released resources
		// Great API design by Microsoft, as always
		public void ResizeWindowSizeDependentResources()
		{
			D3D11RenderTargetViewDesc renderTargetViewDesc = new(D3D11RtvDimension.Texture2D);

			renderTargetView = device.CreateRenderTargetView(backBuffer, renderTargetViewDesc);

			bufferDesc = backBuffer.Description;
			var backBufferWidth = bufferDesc.Width;
			var backBufferHeight = bufferDesc.Height;

			viewport = new()
			{
				TopLeftX = 0,
				TopLeftY = 0,
				Width = backBufferWidth,
				Height = backBufferHeight,
				MinDepth = 0.0f,
				MaxDepth = 1.0f
			};
			deviceContext.RasterizerStageSetViewports([viewport]);
			using var surface = new DxgiSurface2(backBuffer.Handle);
			D2D1RenderTargetProperties properties = new(
				D2D1RenderTargetType.Default,
				new(format, D2D1AlphaMode.Premultiplied),
				DisplayInfo.Dpi,
				DisplayInfo.Dpi,
				D2D1RenderTargetUsages.None,
				D2D1FeatureLevel.Default
			);

			renderTarget = d2dFactory.CreateDxgiSurfaceRenderTarget(surface, properties);
			D3D11RasterizerDesc rasterizerStateDesc = new(D3D11FillMode.Solid, D3D11CullMode.Back, false, 0, 0.0f, 0.0f, true, false, true, false);

			using var rasterizerState = device.CreateRasterizerState(rasterizerStateDesc);
			deviceContext.RasterizerStageSetState(rasterizerState);
			if (CurrentScene != null)
			{
				InitScene(CurrentScene);
			}
		}

		public void InitScene(AbstractScene scene)
		{
			GetClientRect(Hwnd, out var bounds);
			scene.Width = (uint)(bounds.Width / DisplayInfo.DipScale);
			scene.Height = (uint)(bounds.Height / DisplayInfo.DipScale);
			scene.SmallerDimension = scene.Width > scene.Height ? scene.Height : scene.Width;
			scene.CreateResources(renderTarget, dwritefactory);
			scene.CalculateLayout(renderTarget, dwritefactory);
		}

		private void OnDeviceLost()
		{
			ReleaseDeviceResources();
			ReleaseWindowSizeDependentResources();

			CreateDeviceResources();
			CreateSwapChain();
		}

		public void DrawCurrentScene()
		{
			DrawScene(CurrentScene);
		}

		public void DrawScene(AbstractScene scene)
		{
			if (bufferDesc.Height == 0)
			{
				LogW("Tried to draw on zero height");
				return;
			}
			if (renderTarget == null)
			{
				LogE("Render target is null, great");
				return;
			}
			renderTarget.BeginDraw();
			scene.Render(renderTarget);
			try
			{
				renderTarget.EndDraw(out var _, out var _);
			}
			catch (Exception e)
			{
				LogW($"Error while calling EndDraw: {e}");
				if (e.HResult == D2D1Error.RecreateTarget)
				{
					OnDeviceLost();
					return;
				}
				throw;
			}
			try
			{
				DxgiPresentOptions options = fullScreen ? DxgiPresentOptions.None : (DxgiPresentOptions)0x00000200UL;
				swapChain.Present(0, options);
			}
			catch (Exception e)
			{
				LogE("Couldn't present. Error code: " + e.HResult);
			}
		}

		private bool fullScreen = false;

		public void SetFullScreen(bool fullScreen)
		{
			this.fullScreen = fullScreen;
			swapChain.SetFullscreenState(fullScreen);
		}

		public void OnMessage(WM msg, nint wParam,
			nint lParam)
		{
			CurrentScene?.OnMessage(msg, wParam, lParam);
		}

		private bool disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;
			if (disposing)
			{
				CurrentScene = null;
			}

			// TODO: implement disposing of unmanaged resources
			disposed = true;
		}

		~Canvas()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
