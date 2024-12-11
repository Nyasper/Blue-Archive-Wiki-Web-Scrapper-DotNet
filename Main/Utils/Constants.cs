namespace Main.Utils;
public static class Constants
{
	private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
	private static readonly string MainPath = Path.Join(DocumentsPath, "BlueArchiveWS");
	public static readonly string MediaPath = Path.Join(MainPath, "media");
	public static readonly string DataPath = Path.Join(MainPath, "data");
}