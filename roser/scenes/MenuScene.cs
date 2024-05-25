namespace roser.scenes
{
	internal class MenuScene : TextScene
	{
		public MenuScene() : base(WindowManager.Language.GetString(i18n.StringId.StartGame), 0xaa0000)
		{
		}

		protected override void Continue()
		{
			WndManager.SetScene<GameScene>();
		}
	}
}
