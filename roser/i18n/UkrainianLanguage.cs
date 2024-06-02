namespace roser.i18n
{
	internal class UkrainianLanguage : ILanguage
	{
		public static string[] strings = [
				"uk-UA",
				"Roser",
				"Грати",
				"Рівень пройдений",
				"Ви програли",
			];

		public string GetString(StringId stringId)
		{
			return strings[(int)stringId];
		}
	}
}
