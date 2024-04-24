namespace roser.i18n
{
	internal class UkrainianLanguage : ILanguage
	{
		public static string[] strings = [
				"Roser",
				"Почати гру",
			];

		public string GetString(StringId stringId)
		{
			return strings[(int)stringId];
		}
	}
}
