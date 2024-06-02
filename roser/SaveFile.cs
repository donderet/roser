using roser.i18n;
using System.Runtime.InteropServices;
using System.Text;
using static roser.i18n.Language;
using static roser.native.Kernel32;

namespace roser
{
	internal class SaveFile
	{
		public static bool IsFullscreen { get; set; }

		private static LanguageId _languageId = LanguageId.English;

		public static uint CurrentLevel { get; set; }

		public static LanguageId LanguageId
		{
			get
			{
				return _languageId;
			}
			set
			{
				_languageId = value;
				WindowManager.Language = Languages[(int)_languageId];
			}
		}

		private static void SetDefaultLanguage()
		{
			// Thread.CurrentThread.CurrentCulture doesn't work, it is always invariant for some reason
			nint ptr = Marshal.AllocHGlobal(128);
			GetLocaleInfoEx(null, 3, ptr, 64);
			StringBuilder sb = new();
			char c;
			for (int i = 0; (c = (char)Marshal.ReadInt16(ptr + i)) != '\x0'; i += 2)
			{
				sb.Append(char.ToLower(c));
			}
			Marshal.FreeHGlobal(ptr);
			string isoCode = sb.ToString();

			for (int i = 0; i < Languages.Length; i++)
			{
				var testIso = Languages[i].GetString(StringId.LanguageId);
				if (testIso.StartsWith(isoCode))
				{
					LanguageId = (LanguageId)i;
					break;
				}
			}
			if (WindowManager.Language == null)
				LanguageId = LanguageId.English;
		}

		public static void Init()
		{
			SetDefaultLanguage();
			var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var path = Path.Combine(directory, "roser", "save");
			if (!File.Exists(path))
				return;
			using FileStream fs = File.OpenRead(path);
			using BinaryReader reader = new(fs);
			IsFullscreen = reader.ReadBoolean();
			var lang = reader.ReadUInt32();
			if (Enum.IsDefined(typeof(LanguageId), lang))
				LanguageId = (LanguageId) lang;
			CurrentLevel = reader.ReadUInt32();
		}

		public static void Save()
		{
			var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			var dirPath = Path.Combine(directory, "roser");
			var path = Path.Combine(directory, "roser", "save");
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);
			using FileStream fs = File.OpenWrite(path);
			using BinaryWriter writer = new(fs);
			writer.Write(IsFullscreen);
			writer.Write((uint)LanguageId);
			writer.Write(CurrentLevel);
		}
	}
}
