using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using roser.animators;
using static roser.i18n.Language;

namespace roser.scenes
{
	internal class MenuScene : TextScene
	{
		protected bool shouldRecreate = false;

		protected readonly ValueAnimator textTransparencyAnimator = new(0, 0, 100);

		protected D2D1RectF globeBounds = new();
		protected DWriteTextLayout globeTextLayout;
		protected D2D1Brush globeBrush;

		public MenuScene() : base(WindowManager.Language.GetString(i18n.StringId.StartGame), 0xeeeeee)
		{
			particlesManager.AnimateMovement = true;
			particlesManager.AnimateSize = true;
		}

		protected override void Continue()
		{
			WndManager.SetScene<GameScene>();
		}

		public override void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			base.CreateResources(renderTarget, dwriteFactory);
			using DWriteTextFormat textFormat = dwriteFactory.CreateTextFormat("Segoe UI Emoji", dwriteFactory.GetSystemFontCollection(), DWriteFontWeight.Light, DWriteFontStyle.Normal, DWriteFontStretch.Normal, 96, WindowManager.Language.GetString(i18n.StringId.LanguageId));
			globeTextLayout = dwriteFactory.CreateTextLayout("🌐", textFormat, Width, Height);
			globeBrush = renderTarget.CreateSolidColorBrush(new(0xffffffu));
		}

		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			base.CalculateLayout(renderTarget, dwriteFactory);
			float size = Percent(7);
			if (size < 1)
				size = 1;
			globeTextLayout.SetFontSize(size, new(0, 1u));
			
			 // textLayout.GetMetrics() can't work with surrogate pairs, nice. I have to guess the size of the globe emoji
			float marginStart = Width - (size * 1.4f);
			float marginTop = Height - (size * 1.4f);
			globeBounds = new(marginStart, marginTop, Width, Height);
		}

		protected override void DisposeView()
		{
			base.DisposeView();
			globeTextLayout?.Dispose();
			globeBrush?.Dispose();
		}

		public override void OnLMBUp(float x, float y)
		{
			if (CollisionHelper.TestPointRectColliding(10, x, y, globeBounds))
			{
				SaveFile.LanguageId = (LanguageId)((((int)SaveFile.LanguageId) + 1) % Languages.Length);
				shouldRecreate = true;
				SaveFile.Save();
				WndManager.SetScene<MenuScene>();
				return;
			}

			base.OnLMBUp(x, y);
		}


		public override void Render(D2D1RenderTarget renderTarget)
		{
			brush.Opacity = (float)textTransparencyAnimator.Value;
			renderTarget.Clear();

			particlesManager.Render(renderTarget);

			D2D1Point2F textPoint = new(textBounds.Left, textBounds.Top);
			renderTarget.DrawTextLayout(textPoint, textLayout, brush);

			textPoint.X = globeBounds.Left;
			textPoint.Y = globeBounds.Top;
			renderTarget.DrawTextLayout(textPoint, globeTextLayout, globeBrush);

			if (opacityAnimator.Value != 0)
			{
				fadeBrush.Opacity = (float)opacityAnimator.Value;
				renderTarget.FillRectangle(new(0, 0, Width, Height), fadeBrush);
			}
		}

		public override void OnTick(double dt)
		{
			base.OnTick(dt);
			if (textTransparencyAnimator.IsFinished)
			{
				float target = textTransparencyAnimator.Value == 0 ? 1.0f : 0.0f;
				textTransparencyAnimator.To(target, 500);
			}
			textTransparencyAnimator.OnTick(dt);
		}
	}
}
