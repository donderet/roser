namespace roser.i18n
{
	internal class EnglishLanguage : ILanguage
	{
		public static string[] strings = [
				"en-UK",
				"Roser",
				"Play",
				"Level passed",
				"You lost",
			];

		public string GetString(StringId stringId)
		{
			return strings[(int)stringId];
		}
	}
}
