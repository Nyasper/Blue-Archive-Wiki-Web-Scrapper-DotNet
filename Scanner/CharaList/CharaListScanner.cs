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
		var trNodes = html.DocumentNode.SelectNodes("//tbody[1]/tr");
		if (trNodes is null) return [];

		var tableNodes = trNodes.ToList();

		StudentListItem[] charaListItems = tableNodes.Skip(1).Select((item) =>
		{
			// the charaName is in an anchor: '<a title="charaName">'
			var aElement = item.SelectSingleNode(".//a[@title]");
			if (aElement is null) return null;

			var imgElement = aElement.SelectSingleNode(".//img[@src]");
			if (imgElement is null) return null;

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
		})
		.Where(students => students != null && !ExcludedStudents.Contains(students.CharaName))
		.Select(s => s!)
		.OrderBy(s => s.School)
		.ThenBy(s => s.CharaName)
		.ToArray();

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
		string[] includeInOther = ["other", "ETC", "Sakugawa", "Tokiwadai"];
		
		if (includeInOther.Contains(schoolParam, StringComparer.OrdinalIgnoreCase))
		{
			return "other";
		}
		
		return schoolParam.ToLower();
	}
	static private string GetSkinSet(string charaName)
	{
		string[] excludeSkinSets = [
			"kid"
		];
		if (!charaName.EndsWith(')') || !charaName.Contains("_(")) return "default";
		string skinSet = charaName.Split('(')[1].Split(')')[0].Trim().ToLower().Replace("-", "_");
		if (excludeSkinSets.Contains(skinSet)) return "default";
		return skinSet;
	}

	static private readonly string[] ExcludedStudents = ["Shiroko*Terror", "Hoshino_(Battle)_Attacker"];
}