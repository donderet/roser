using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using roser.animators;
using roser.native;
using roser.particles;

namespace roser.scenes
{
	internal abstract class TextScene(string text, uint uintColor) : AbstractScene
	{
		protected readonly BackgroundParticlesManager particlesManager = new();
		protected readonly ValueAnimator opacityAnimator = new(1, 0, 200);

		protected D2D1RectF textBounds;
		protected DWriteTextLayout? textLayout;
		protected D2D1SolidColorBrush? brush;
		protected D2D1SolidColorBrush? fadeBrush;

		protected override void DisposeView()
		{
			textLayout?.Dispose();
			textLayout = null;
			brush?.Dispose();
			brush = null;
			fadeBrush?.Dispose();
			fadeBrush = null;
			particlesManager.ReleaseParticles();
		}

		public override void OnKeyDown(VK vk)
		{
			if (vk == VK.Space || vk == VK.Return)
				Fade();
		}

		public override void OnLMBUp(float x, float y)
		{
			Fade();
		}

		protected void Fade()
		{
			if (opacityAnimator.Value == 0)
				opacityAnimator.To(1, 100);
		}

		protected abstract void Continue();

		public override void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			DisposeView();
			particlesManager.Height = Height;
			particlesManager.Width = Width;
			particlesManager.CreateResources(renderTarget);
			D2D1ColorF color = new(uintColor);
			using DWriteTextFormat textFormat = dwriteFactory.CreateTextFormat("Aharoni", dwriteFactory.GetSystemFontCollection(), DWriteFontWeight.Light, DWriteFontStyle.Normal, DWriteFontStretch.Normal, 96, WindowManager.Language.GetString(i18n.StringId.LanguageId));
			textFormat.TextAlignment = DWriteTextAlignment.Center;
			textLayout = dwriteFactory.CreateTextLayout(text, textFormat, Width, Height);
			brush = renderTarget.CreateSolidColorBrush(color);
			fadeBrush = renderTarget.CreateSolidColorBrush(new(0x0u));
		}

		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			float size = Percent(15);
			if (size < 1)
				size = 1;
			textLayout.SetFontSize(size, new(0, (uint)text.Length));
			var startGameTextMetrics = textLayout.GetMetrics();
			// Text is centered by DWriteTextAlignment.Center
			float marginStart = 0;
			float marginTop = (Height - startGameTextMetrics.Height) / 2;
			textBounds = new(marginStart, marginTop, marginStart + startGameTextMetrics.Width, marginTop + startGameTextMetrics.Height);
		}

		public override void Render(D2D1RenderTarget renderTarget)
		{
			renderTarget.Clear();

			particlesManager.Render(renderTarget);

			D2D1Point2F textPoint = new(textBounds.Left, textBounds.Top);
			renderTarget.DrawTextLayout(textPoint, textLayout, brush);
			if (opacityAnimator.Value != 0)
			{
				fadeBrush.Opacity = (float)opacityAnimator.Value;
				renderTarget.FillRectangle(new(0, 0, Width, Height), fadeBrush);
			}
		}

		public override void OnTick(double dt)
		{
			opacityAnimator.OnTick(dt);
			particlesManager.OnTick(dt);
			if (opacityAnimator.IsFinished && opacityAnimator.Value == 1)
			{
				Continue();
				return;
			}	
		}
	}
}
