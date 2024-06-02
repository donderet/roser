namespace roser.i18n
{
	internal class RuthenianLanguage : ILanguage
	{
		public static string[] strings = [
				"rue-UA",
				"Roser",
				"Бавити",
				"Етапа пройдена",
				"Вы пройграли",
			];

		public string GetString(StringId stringId)
		{
			return strings[(int)stringId];
		}
	}
}
