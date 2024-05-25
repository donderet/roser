namespace roser.scenes
{
	internal class VictoryScene : TextScene
	{
		public VictoryScene() : base(WindowManager.Language.GetString(i18n.StringId.Victory), 0x00aa00)
		{
		}
		protected override void Continue()
		{
			WndManager.SetScene<MenuScene>();
		}
	}
}
