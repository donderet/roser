namespace roser.i18n
{
	internal class Language
	{
		public static ILanguage[] Languages { get; } = [
				new RuthenianLanguage(),
				new UkrainianLanguage(),
				new EnglishLanguage(),
			];

		internal enum LanguageId : uint
		{
			Ruthenian,
			Ukrainian,
			English,
		}
	}
}
