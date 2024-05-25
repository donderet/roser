using JeremyAnsel.DirectX.D2D1;
using roser.animators;

namespace roser.scenes
{
	internal class MenuScene : TextScene
	{
		protected ValueAnimator textTransparencyAnimator = new(0, 0, 100);

		public MenuScene() : base(WindowManager.Language.GetString(i18n.StringId.StartGame), 0xeeeeee)
		{
			particlesManager.AnimateMovement = true;
			particlesManager.AnimateSize = true;
		}

		protected override void Continue()
		{
			WndManager.SetScene<GameScene>();
		}

		public override void Render(D2D1RenderTarget renderTarget)
		{
			brush.Opacity = (float)textTransparencyAnimator.Value;
			base.Render(renderTarget);
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
