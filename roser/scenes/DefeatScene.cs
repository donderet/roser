﻿using JeremyAnsel.DirectX.D2D1;
using JeremyAnsel.DirectX.DWrite;
using roser.animators;

namespace roser.scenes
{
	internal class DefeatScene : TextScene
	{
		protected ValueAnimator transparencyAnimator = new(1, 1, 5000);
			
		public DefeatScene() : base(WindowManager.Language.GetString(i18n.StringId.Defeat), 0xaa0000)
		{
			particlesManager.AnimateSize = true;
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

		public override void CalculateLayout(D2D1RenderTarget renderTarget, DWriteFactory dwriteFactory)
		{
			base.CalculateLayout(renderTarget, dwriteFactory);
			// Recreate static particles
			particlesManager.ReleaseParticles();
			particlesManager.CreateResources(renderTarget);
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
