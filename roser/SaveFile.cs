namespace roser
{
	internal class SaveFile
	{
		public bool IsFullscreen { get; set; }


		public SaveFile()
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
