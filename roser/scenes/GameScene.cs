using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using roser.gameobjects;
using roser.native;

namespace roser.scenes
{
	internal class GameScene : AbstractScene
	{

		private D2D1RectF boundsRect;
		private D2D1Brush? boundsBrush;

		public const uint bricksOnRow = 10;
		private float brickSize;

		private D2D1Matrix3X2F defaultRotation = D2D1Matrix3X2F.Rotation(0);
		private D2D1Matrix3X2F paddleRotation;
		private D2D1RoundedRect paddleRoundedRect;
		private D2D1RectF paddleRect;
		private float paddleWidth;
		private float paddleHeight;

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
			paddleRoundedRect.RadiusX = 3;
			paddleRoundedRect.RadiusY = 3;
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
			paddleWidth = (float)(arena.RealWidthCoef * Paddle.Width);
			paddleHeight = (float)(arena.RealHeightCoef * Paddle.Height);
			paddleRect.Top = boundsRect.Top + arena.GetPaddleY();
		}

		public override void OnKeyDown(VK vk)
		{
			if (vk == VK.KeyA || vk == VK.Left) {
				arena.paddle.MovingLeft = true;
				return;
			}
			if (vk == VK.KeyD || vk == VK.Right) {
				arena.paddle.MovingRight = true;
				return;
			}
		}

		public override void OnKeyUp(VK vk)
		{
			if (vk == VK.KeyA || vk == VK.Left)
			{
				arena.paddle.MovingLeft = false;
				return;
			}
			if (vk == VK.KeyD || vk == VK.Right)
			{
				arena.paddle.MovingRight = false;
				return;
			}
		}

		private long frame = 0;

		public override void Render(D2D1RenderTarget renderTarget)
		{
			frame++;
			renderTarget.Clear();
			renderTarget.DrawRectangle(boundsRect, boundsBrush);
			ballEllipse.Point = new(boundsRect.Left + arena.GetBallX(), boundsRect.Top + arena.GetBallY());

			paddleRect.Left = boundsRect.Left + arena.GetPaddleX();
			paddleRect.Right = paddleRect.Left + paddleWidth;
			paddleRect.Bottom = paddleRect.Top + paddleHeight;
			paddleRoundedRect.Rect = paddleRect;

			paddleRotation = D2D1Matrix3X2F.Rotation(arena.paddle.Angle, new(paddleRect.Left + (Paddle.Width * arena._realWidthCoef / 2), paddleRect.Top + (Paddle.Height * arena._realHeightCoef / 2)));

#if DEBUG
			renderTarget.DrawText(string.Format("Frame time: {0:n2}\nTick time: {1:n2}\nEllapsed ticks: {2}\nFrame: {3}\nBall X: {4:n2}\nBall Y: {5:n2}\nBall accumulator: {6:n2}\nPaddle accumulator: {7:n2}\nPaddle vx: {8:n15}\nPaddle angle: {9:n2}\nLast dt: {10:n2}", WindowManager.FrameTime, WindowManager.TickTime, WindowManager.stopwatch.ElapsedTicks, frame, arena.BallX, arena.BallY, arena.ball.accumulator, arena.paddle.accumulator, arena.paddle.vx, arena.paddle.Angle, lastDt), textFormat, new(0, 0, Width, Height), textBrush);
			var ghostRot = D2D1Matrix3X2F.Rotation(arena.paddle.Angle, new(paddleRect.Left + (Paddle.Width * arena._realWidthCoef / 2), paddleRect.Top + (Paddle.Height * arena._realHeightCoef / 2)));

			renderTarget.DrawRoundedRectangle(paddleRoundedRect, textBrush);

			renderTarget.Transform = ghostRot;
			renderTarget.DrawEllipse(ballEllipse, textBrush);
#endif
			renderTarget.Transform = paddleRotation;
			renderTarget.DrawRoundedRectangle(paddleRoundedRect, boundsBrush);
			renderTarget.Transform = defaultRotation;
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
