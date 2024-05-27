using JeremyAnsel.DirectX.D2D1;
using roser.animators;

namespace roser.scenes
{
	internal class VictoryScene : TextScene
	{
		protected readonly ValueAnimator transparencyAnimator = new(1, 1, 5000);

		public VictoryScene() : base(WindowManager.Language.GetString(i18n.StringId.Victory), 0x00aa00)
		{
			particlesManager.AnimateMovement = true;
			particlesManager.AnimateSize = true;
			particlesManager.AnimateColor = true;
		}

		protected override void Continue()
		{
			WndManager.SetScene<MenuScene>();
		}

		public override void Render(D2D1RenderTarget renderTarget)
		{
			brush.Opacity = (float)transparencyAnimator.Value;
			base.Render(renderTarget);
		}

		public override void OnTick(double dt)
		{
			base.OnTick(dt);
			if (transparencyAnimator.IsFinished && transparencyAnimator.Value == 1)
			{
				transparencyAnimator.To(0, 100);
			}
			transparencyAnimator.OnTick(dt);
		}
	}
}
