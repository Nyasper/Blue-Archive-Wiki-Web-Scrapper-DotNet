using System.Text.Json;

namespace Main.Utils;
public static class Constants
{
	public static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = false
	};
	private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
	private static readonly string MainPath = Path.Join(DocumentsPath, "BlueArchiveWS");
	public static readonly string MediaPath = Path.Join(MainPath, "media");
	public static readonly string DataPath = Path.Join(MainPath, "data");
	public static readonly string JsonConfigPath = Path.Join(MainPath, "config.json");
}