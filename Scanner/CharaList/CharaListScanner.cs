namespace Scanner.CharaList;
using Configuration;
using Model;
using Utils;

public class CharaListScanner(IHtmlHandler htmlHandler) : ICharaListScanner
{
	public async Task<CharaListStudent[]> ScanCharaList()
	{
		var html = await htmlHandler.ScanHtml(Constants.CharaListPageUrl);
		var tableNodes = html.DocumentNode.SelectNodes("//tbody[1]/tr").ToList();

		CharaListStudent[] charaListItems = tableNodes.Skip(1).Select((item) =>
		{
			// the charaName is in an anchor: '<a title="charaName">'
			var aElement = item.SelectSingleNode(".//a[@title]");
			var imgElement = aElement.SelectSingleNode(".//img[@src]");

			string charaName = aElement.GetAttributeValue("title", "");
			string school = item.GetAttributeValue("data-school", "");
			string releaseDate = item.GetAttributeValue("data-releasedate-jp", "");
			string skinSet = GetSkinSet(charaName);
			string smallImgUrl = "https:" + imgElement.GetAttributeValue("src", "");

			return new CharaListStudent()
			{
				CharaName = charaName,
				School = school,
				ReleaseDate = releaseDate,
				SkinSet = skinSet,
				SmallImgUrl =  smallImgUrl
			};
		}).ToArray();

		return charaListItems;
	}

	private static string GetCharaName(string url)
	{
		string charaName = url.Split("/wiki/")[1];

		string[] excludedNames = ["Shiroko%EF%BC%8ATerror", "Shiroko_(Terror)"];
		return excludedNames.Contains(charaName) ? "Shiroko" : charaName;
	}


	static private string GetSkinSet(string charaName)
	{
		if (!charaName.EndsWith(')') || !charaName.Contains("_(") || Student.ExcludeSkinSets.Contains(charaName)) return "default";
		return charaName.Split('(')[1].Split(')')[0].Trim().ToLower();
	}
}