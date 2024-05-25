using JeremyAnsel.DirectX.D2D1;
using roser.animators;

namespace roser.scenes
{
	internal class MenuScene : TextScene
	{
		protected ValueAnimator transparencyAnimator = new(1, 1, 0);

		public MenuScene() : base(WindowManager.Language.GetString(i18n.StringId.StartGame), 0xaa0000)
		{
		}

		protected override void Continue()
		{
			WndManager.SetScene<GameScene>();
		}

		public override void Render(D2D1RenderTarget renderTarget)
		{
			brush.Opacity = (float)transparencyAnimator.Value;
			base.Render(renderTarget);
		}

		public override void OnTick(double dt)
		{
			base.OnTick(dt);
			if (transparencyAnimator.IsFinished)
			{
				float target = transparencyAnimator.Value == 0 ? 1.0f : 0.0f;
				transparencyAnimator.To(target, 500);
			}
			transparencyAnimator.OnTick(dt);
		}
	}
}
