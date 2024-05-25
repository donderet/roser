using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using roser.native;
using roser.particles;

namespace roser.scenes
{
	internal abstract class TextScene(string text, uint uintColor) : AbstractScene
	{
		protected BackgroundParticlesManager particlesManager = new();

		protected D2D1RectF textBounds;
		protected DWriteTextLayout? textLayout;
		protected D2D1SolidColorBrush? brush;

		protected override void DisposeView()
		{
			textLayout?.Dispose();
			textLayout = null;
			brush?.Dispose();
			brush = null;
		}

		public override void OnKeyDown(VK vk)
		{
			if (vk == VK.Space || vk == VK.Return)
				Continue();
		}

		public override void OnLMBDown(float x, float y)
		{
			Continue();
		}

		protected abstract void Continue();

		public override void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			DisposeView();
			particlesManager.Height = Height;
			particlesManager.Width = Width;
			particlesManager.CreateResources(renderTarget);
			D2D1ColorF color = new(uintColor);
			DWriteTextFormat textFormat = dwriteFactory.CreateTextFormat("Aharoni", dwriteFactory.GetSystemFontCollection(), DWriteFontWeight.Light, DWriteFontStyle.Normal, DWriteFontStretch.Normal, 96, WindowManager.Language.GetString(i18n.StringId.LanguageId));
			textFormat.TextAlignment = DWriteTextAlignment.Center;
			textLayout = dwriteFactory.CreateTextLayout(text, textFormat, Width, Height);
			brush = renderTarget.CreateSolidColorBrush(color);
		}

		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			float size = Percent(15);
			if (size < 1)
				size = 1;
			textLayout.SetFontSize(size, new(0, (uint)text.Length));
			var startGameTextMetrics = textLayout.GetMetrics();
			float marginStart = textLayout.TextAlignment == DWriteTextAlignment.Center ? 0 : (Width - startGameTextMetrics.Width) / 2;
			float marginTop = (Height - startGameTextMetrics.Height) / 2;
			textBounds = new(marginStart, marginTop, marginStart + startGameTextMetrics.Width, marginTop + startGameTextMetrics.Height);
		}

		public override void Render(D2D1RenderTarget renderTarget)
		{
			renderTarget.Clear();

			particlesManager.Render(renderTarget);

			D2D1Point2F textPoint = new(textBounds.Left, textBounds.Top);
			renderTarget.DrawTextLayout(textPoint, textLayout, brush);
		}

		public override void OnTick(double dt)
		{
			particlesManager.OnTick(dt);
		}
	}
}
