namespace Main.Utils;

public static class UtilsMethods
{
	public static bool HasAsianCharacter(char c)
	{
		int code = c;

		// Chino / Kanji
		if (code >= 0x4E00 && code <= 0x9FFF)
			return true;

		// Hiragana
		else if (code >= 0x3040 && code <= 0x309F)
			return true;

		// Katakana
		else if (code >= 0x30A0 && code <= 0x30FF)
			return true;

		// Hangul (coreano)
		else if (code >= 0xAC00 && code <= 0xD7AF)
			return true;

		return false;
	}
}