namespace Scanner.Utils;

public static class Constants
{
	public const string Domain = "https://bluearchive.wiki";
	public const string BaseUrl = Domain + "/wiki/";
	public const string CharaListPageUrl = BaseUrl + "Characters";


	public static class DefaultCharaListStudentFields
	{
		public static readonly string CharaName = "no_charaName_found";
		public static readonly int? Age = null;
		public static readonly string School = "no_school_found";
		public static readonly string SkinSet = "no_skinSet_found";
		public static readonly string ReleaseDate = "no_releaseDate_found";
		public static readonly string SmallImgUrl = "no_smallImgUrl_found";
		public static readonly string PageUrl = "no_pageUrl_found";
	}
	public static class DefaultCharaDetailsFields
	{
		public static readonly string Name = "no_name_found";
		public static readonly string LastName = "no_lastName_found";
		public static readonly int? Age = null;
		public static readonly int? Height = null;
		public static readonly string? Birthday = null;
		public static readonly string Hobbies = "no_hobbies_found";
		public static readonly string Designer = "no_designer_found";
		public static readonly string Illustrator = "no_illustrator_found";
		public static readonly string Voice = "no_voice_found";
		public static readonly string ImageProfileUrl = "no_imageProfileUrl_found";
		public static readonly string ImageFullUrl = "no_imageFullUrl_found";
		public static readonly string AudioUrl = "no_audioUrl_found";
	}
}