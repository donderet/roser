namespace roser.scenes
{
	internal class DefeatScene : TextScene
	{
		public DefeatScene() : base(WindowManager.Language.GetString(i18n.StringId.Defeat), 0xaa0000)
		{
		}

		protected override void Continue()
		{
			WndManager.SetScene<GameScene>();
		}
	}
}
