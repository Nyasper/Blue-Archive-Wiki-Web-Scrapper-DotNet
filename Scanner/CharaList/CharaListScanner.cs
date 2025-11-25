using System.Linq;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace Scanner.CharaList;
using Configuration;
using Model;
using Utils;

public class CharaListScanner(IHtmlHandler htmlHandler) : ICharaListScanner
{
	public async Task<StudentListItem[]> ScanCharaList()
	{
		var html = await htmlHandler.ScanHtml(Constants.CharaListPageUrl);
		var tableNodes = html.DocumentNode.SelectNodes("//tbody[1]/tr").ToList();

		StudentListItem[] charaListItems = tableNodes.Skip(1).Select((item) =>
		{
			// the charaName is in an anchor: '<a title="charaName">'
			var aElement = item.SelectSingleNode(".//a[@title]");
			var imgElement = aElement.SelectSingleNode(".//img[@src]");

			string charaName = GetCharaName(aElement);
			string school = GetSchool(item.GetAttributeValue("data-school", "").Trim());
			string releaseDate = item.GetAttributeValue("data-releasedate-jp", "").Trim();
			string skinSet = GetSkinSet(charaName);
			string smallImgUrl = "https:" + imgElement.GetAttributeValue("src", "").Trim();


			return new StudentListItem()
			{
				CharaName = charaName,
				School = school,
				ReleaseDate = releaseDate,
				SkinSet = skinSet,
				SmallImgUrl =  smallImgUrl,
				PageUrl = Constants.BaseUrl + charaName
			};
		}).Where(students=>!ExcludedStudents.Contains(students.CharaName)).OrderBy(s=>s.School).ThenBy(s=>s.CharaName).ToArray();

		return charaListItems;
	}
	static private string GetCharaName(HtmlNode elementNode)
	{
		var charaName = elementNode.GetAttributeValue("title", "").Replace(" ", "_");
		if (charaName.Contains('＊', StringComparison.OrdinalIgnoreCase))
		{
			return charaName.Replace("＊", "*", StringComparison.OrdinalIgnoreCase);
		}

		return charaName;
	}
	static private string GetSchool(string schoolParam)
	{
		string[] schools = [
			"Abydos",
			"Arius",
			"Gehenna",
			"Highlander",
			"Hyakkiyako",
			"Millennium",
			"Red Winter",
			"SRT",
			"Shanhaijing",
			"Trinity",
			"Valkyrie",
			"Wildhunt"
		];
		string schoolFound = schools.Contains(schoolParam, StringComparer.OrdinalIgnoreCase) ? schoolParam : "other";
		return schoolFound.ToLower();
	}
	static private string GetSkinSet(string charaName)
	{
		string[] excludeSkinSets = [
			"kid"
		];
		if (!charaName.EndsWith(')') || !charaName.Contains("_(") || excludeSkinSets.Contains(charaName)) return "default";
		return charaName.Split('(')[1].Split(')')[0].Trim().ToLower().Replace("-", "_");
	}

	static private readonly string[] ExcludedStudents = ["Shiroko＊Terror", "Shiroko*Terror", "Shiroko * Terror"];
}