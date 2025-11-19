namespace Scanner.Utils;

public static class Constants
{
	public const string Domain = "https://bluearchive.wiki";
	public const string BaseUrl = Domain + "/wiki/";
	public const string CharaListPageUrl = BaseUrl + "Characters";


	public static class DefaultCharaListStudentFields
	{
		public static string CharaName = "no_charaName_found";
		public static readonly int? Age = null;
		public static readonly string School = "no_school_found";
		public static readonly string SkinSet = "no_skinSet_found";
		public static readonly string ReleaseDate = "no_releaseDate_found";
		public static readonly string SmallImgUrl = "no_smallImgUrl_found";
	}
	public static class DefaultCharaDetailsFields
	{
		public static string Name = "no_name_found";
		public static string LastName = "no_lastName_found";
	}
}