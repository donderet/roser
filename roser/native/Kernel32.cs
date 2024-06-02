using System.Runtime.InteropServices;

namespace roser.native
{
	internal partial class Kernel32
	{
		[LibraryImport("kernel32.dll")]
		public static partial int GetLocaleInfoEx(
			[MarshalAs(UnmanagedType.LPWStr)]
			string? lpLocaleName,
			int lcType,
			nint lpLcData,
			int cchData
			);
	}
}
