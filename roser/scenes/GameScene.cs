using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using roser.animators;
using roser.gameobjects;
using roser.native;

namespace roser.scenes
{
	internal class GameScene : AbstractScene
	{
		private readonly ValueAnimator opacityAnimator = new(1, 0, 100);
		private D2D1SolidColorBrush opacityBrush;

		private D2D1RectF boundsRect;
		private D2D1Brush? boundsBrush;

		private D2D1Matrix3X2F defaultRotation = D2D1Matrix3X2F.Rotation(0);
		private D2D1Matrix3X2F paddleRotation;
		private D2D1RoundedRect paddleRoundedRect;
		private D2D1RectF paddleRect;
		private float paddleWidth;
		private float paddleHeight;

		private D2D1Ellipse ballEllipse;
		private readonly Arena arena = new();

		private D2D1SolidColorBrush? textBrush;
		DWriteTextFormat textFormat;

		protected override void DisposeView()
		{
			boundsBrush?.Dispose();
			textBrush?.Dispose();
			textFormat?.Dispose();
			opacityBrush?.Dispose();
			foreach (BrushWrapper brushWrapper in brushPool)
			{
				brushWrapper.Brush.Dispose();
			}
			brushPool.Clear();
		}

		private Arena.ArenaListener OnFadeEnd;

		public override void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			arena.OnBottomCollision = () =>
			{
				opacityAnimator.To(1, 100);
				OnFadeEnd = WndManager.SetScene<DefeatScene>;
			};
			arena.OnNoBricks = () =>
			{
				opacityAnimator.To(1, 100);
				OnFadeEnd = WndManager.SetScene<VictoryScene>;
				SaveFile.CurrentLevel++;
				SaveFile.Save();
			};
			DisposeView();
			boundsBrush = renderTarget.CreateSolidColorBrush(new(0xff_ff_ff));

			D2D1ColorF color = new(0xaa0000);
			textFormat = dwriteFactory.CreateTextFormat("Arial", dwriteFactory.GetSystemFontCollection(), DWriteFontWeight.Light, DWriteFontStyle.Normal, DWriteFontStretch.Normal, 18, "uk-UA");
			textBrush = renderTarget.CreateSolidColorBrush(color);
			paddleRoundedRect.RadiusX = 3;
			paddleRoundedRect.RadiusY = 3;
			opacityBrush = renderTarget.CreateSolidColorBrush(new(0x0u));
		}

		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
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
			if (vk == VK.KeyA || vk == VK.Left)
			{
				arena.paddle.MovingLeft = true;
				return;
			}
			if (vk == VK.KeyD || vk == VK.Right)
			{
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

		private readonly LinkedList<BrushWrapper> brushPool = new();

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
			renderTarget.DrawText(string.Format("Frame time: {0:n2}\nTick time: {1:n2}\nFrame: {2}", WindowManager.FrameTime, WindowManager.TickTime, frame), textFormat, new(0, 0, Width, Height), textBrush);
			var ghostRot = D2D1Matrix3X2F.Rotation(arena.paddle.Angle, new(paddleRect.Left + (Paddle.Width * arena._realWidthCoef / 2), paddleRect.Top + (Paddle.Height * arena._realHeightCoef / 2)));

			renderTarget.DrawRoundedRectangle(paddleRoundedRect, textBrush);

			renderTarget.Transform = ghostRot;
			renderTarget.DrawEllipse(ballEllipse, textBrush);
#endif
			renderTarget.Transform = paddleRotation;
			renderTarget.DrawRoundedRectangle(paddleRoundedRect, boundsBrush);
			renderTarget.Transform = defaultRotation;
			renderTarget.DrawEllipse(ballEllipse, boundsBrush);

			D2D1RectF brickRect = new();
			D2D1RoundedRect brickRoundedRect = new()
			{
				RadiusX = 3,
				RadiusY = 3
			};
			int len1 = arena.Bricks.GetLength(0);
			int len2 = arena.Bricks.GetLength(1);
			for (int i = 0; i < len1; i++)
			{
				for (int j = 0; j < len2; j++)
				{
					Brick? brick = arena.Bricks[i, j];

					if (brick == null)
						continue;
					BrushWrapper? brushWrapper = brushPool.FirstOrDefault(b => b.Color == brick.Color);
					D2D1Brush brush;
					if (brushWrapper == null)
					{
						LogI("Created brush while drawing UwU");
						brush = renderTarget.CreateSolidColorBrush(new(brick.Color));
						brushPool.AddLast(new BrushWrapper(brush, brick.Color));
					}
					else
						brush = brushWrapper.Brush;
					brickRect.Left = (float)(boundsRect.Left + (brick.X * arena.RealWidthCoef));
					brickRect.Right = (float)(boundsRect.Left + (brick.X * arena.RealWidthCoef) + (brick.Width * arena.RealWidthCoef));
					brickRect.Top = (float)(boundsRect.Top + (brick.Y * arena.RealHeightCoef));
					brickRect.Bottom = (float)(boundsRect.Top + (brick.Y * arena.RealHeightCoef) + (brick.Height * arena.RealHeightCoef));
					brickRoundedRect.Rect = brickRect;
					// bottleneck
					renderTarget.FillRoundedRectangle(brickRoundedRect, brush);
				}
			}
			if (opacityAnimator.Value != 0)
			{
				opacityBrush.Opacity = (float)opacityAnimator.Value;
				renderTarget.FillRectangle(new(0, 0, Width, Height), opacityBrush);
			}
		}

		public override void OnTick(double dt)
		{
			opacityAnimator.OnTick(dt);
			if (!opacityAnimator.IsFinished)
				return;
			else if (opacityAnimator.Value == 1)
			{
				OnFadeEnd();
				return;
			}
			arena.OnTick(dt);
		}
	}
}
