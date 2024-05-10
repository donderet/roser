using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using static roser.Logger;

namespace roser.scenes
{
	internal class MenuScene : AbstractScene
	{
		private D2D1RectF startGameBounds;
		private DWriteTextLayout? startTextLayout;
		private D2D1SolidColorBrush? brush;

		public override void OnLMBUp(float x, float y)
		{
			if (CollisionHelper.TestCollision(Percent(10), x, y, startGameBounds))
			{
				LogI("The text is clicked");
				WndManager.SetScene<GameScene>();
			}
		}

		protected override void DisposeView()
		{
			startTextLayout?.Dispose();
			startTextLayout = null;
			brush?.Dispose();
			brush = null;
		}

		public override void CreateResources(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			DisposeView();
			D2D1ColorF color = new(0xaa0000);
			DWriteTextFormat textFormat = dwriteFactory.CreateTextFormat("Aharoni", dwriteFactory.GetSystemFontCollection(), DWriteFontWeight.Light, DWriteFontStyle.Normal, DWriteFontStretch.Normal, 96, "uk-UA");
			startTextLayout = dwriteFactory.CreateTextLayout(Language.GetString(i18n.StringId.StartGame), textFormat, (float)(Width * DisplayInfo.DipScale), (float)(Height * DisplayInfo.DipScale));
			brush = renderTarget.CreateSolidColorBrush(color);
			LogI("Solid color brush created");
		}

		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			startTextLayout.SetFontSize(Percent(15), new(0, (uint)Language.GetString(i18n.StringId.StartGame).Length));
			var startGameTextMetrics = startTextLayout.GetMetrics();
			float marginStart = (Width - startGameTextMetrics.Width) / 2;
			float marginTop = (Height - startGameTextMetrics.Height) / 2;
			startGameBounds = new(marginStart, marginTop, marginStart + startGameTextMetrics.Width, marginTop + startGameTextMetrics.Height);
		}

		public override void Render(D2D1RenderTarget renderTarget)
		{
			renderTarget.Clear();
			D2D1Point2F textPoint = new(startGameBounds.Left, startGameBounds.Top);
			renderTarget.DrawTextLayout(textPoint, startTextLayout, brush);
		}

		public override void OnTick(double dt)
		{

		}
	}
}
