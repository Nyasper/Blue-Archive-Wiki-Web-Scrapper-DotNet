namespace Testing;

using Scanner.Model;


internal static class Constants
{
	public const string ExpectedImageContentType = "image/png";
	public const string ExpectedAudioContentType = "application/ogg";
	
	public static readonly Student StudentToScan = new()
	{
		CharaName = "Eimi_(Swimsuit)",
		Name = "Eimi",
		LastName = "Izumimoto",
		School = "millennium",
		Age = 15,
		Height = 167,
		Birthday = "May 1",
		Hobbies = "Zoning out, listening to music",
		Designer = "ポップキュン_(POPQN)",
		Illustrator = "ポップキュン_(POPQN)",
		Voice = "Matsunaga_Akane",
		ReleaseDate = "2023/12/06",
		SkinSet = "swimsuit",
		PageUrl = "https://bluearchive.wiki/wiki/Eimi_(Swimsuit)",
		ImageProfileUrl = "https://static.wikitide.net/bluearchivewiki/thumb/c/ce/Eimi_%28Swimsuit%29.png/266px-Eimi_%28Swimsuit%29.png",
		ImageFullUrl = "https://static.wikitide.net/bluearchivewiki/f/fc/Eimi_%28Swimsuit%29_diorama_00.png",
		SmallImageUrl = "https://static.wikitide.net/bluearchivewiki/thumb/c/ce/Eimi_%28Swimsuit%29.png/40px-Eimi_%28Swimsuit%29.png",
		AudioUrl = "https://static.wikitide.net/bluearchivewiki/c/cb/Eimi_%28Swimsuit%29_Title.ogg"
	};
}