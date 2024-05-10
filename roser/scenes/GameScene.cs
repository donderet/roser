using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using roser.gameobjects;

namespace roser.scenes
{
	internal class GameScene : AbstractScene
	{

		private D2D1RectF boundsRect;
		private D2D1Brush? boundsBrush;

		public const uint bricksOnRow = 10;
		private float brickSize;

		private D2D1Ellipse ballEllipse;
		private Arena arena = new();

		private D2D1Brush? textBrush;
		DWriteTextFormat textFormat;

		protected override void DisposeView()
		{
			boundsBrush?.Dispose();
			textBrush?.Dispose();
			textFormat?.Dispose();
		}

		public override void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			LogI("CreateResources called");
			DisposeView();
			boundsBrush = renderTarget.CreateSolidColorBrush(new(0xff_ff_ff));

			D2D1ColorF color = new(0xaa0000);
			textFormat = dwriteFactory.CreateTextFormat("Arial", dwriteFactory.GetSystemFontCollection(), DWriteFontWeight.Light, DWriteFontStyle.Normal, DWriteFontStretch.Normal, 18, "uk-UA");
			textBrush = renderTarget.CreateSolidColorBrush(color);
		}
		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			LogI("CalculateLatyout called");
			boundsRect.Left = (Width - (Height * Arena.ArenaAspect)) / 2;
			boundsRect.Right = Width - boundsRect.Left;
			boundsRect.Top = 0;
			boundsRect.Bottom = Height;
			if (boundsRect.Left <= 0)
			{
				boundsRect.Left = 0;
				boundsRect.Right = Width;
				boundsRect.Top = Math.Abs(Height - (Width / Arena.ArenaAspect)) / 2;
				boundsRect.Bottom = Height - boundsRect.Top;
			}
			arena.SetRealWidth(boundsRect.Right - boundsRect.Left);
			arena.SetRealHeight(boundsRect.Bottom - boundsRect.Top);
			ballEllipse.RadiusX = (float)(arena.RealWidthCoef * Ball.Radius);
			ballEllipse.RadiusY = (float)(arena.RealHeightCoef * Ball.Radius);
		}

		private long frame = 0;

		public override void Render(D2D1RenderTarget renderTarget)
		{
			frame++;
			renderTarget.Clear();
			renderTarget.DrawText(string.Format("Frame time: {0:n2}\nTick time: {1:n2}\nEllapsed ticks: {2}\nFrame: {3}\nBall X: {4:n2}\nBall Y: {5:n2}\nBall accumulator: {6:n2}\nLast dt: {7:n2}", WindowManager.FrameTime, WindowManager.TickTime, WindowManager.stopwatch.ElapsedTicks, frame, arena.BallX, arena.BallY, arena.ball.accumulator, lastDt), textFormat, new(0, 0, Width, Height), textBrush);
			renderTarget.DrawRectangle(boundsRect, boundsBrush);
			ballEllipse.Point = new(boundsRect.Left + arena.GetBallX(), boundsRect.Top + arena.GetBallY());

			renderTarget.DrawEllipse(ballEllipse, boundsBrush);
		}

		private double lastDt = 0;

		public override void OnTick(double dt)
		{
			lastDt = dt;
			arena.OnTick(dt);
		}
	}
}
