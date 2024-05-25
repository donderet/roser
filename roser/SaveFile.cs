namespace roser
{
	internal class SaveFile
	{
		public static bool IsFullscreen { get; set; }
		public static uint CurrentLevel { get; set; }

		public static void Init()
		{
			var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var path = Path.Combine(directory, "roser", "save");
			if (!File.Exists(path))
				return;
			using FileStream fs = File.OpenRead(path);
			// TODO: Implement reading save
		}
	}
}
