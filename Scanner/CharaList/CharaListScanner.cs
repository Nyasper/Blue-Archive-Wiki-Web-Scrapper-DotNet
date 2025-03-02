namespace Scanner.CharaList;
using Configuration;

using Model;

using Utils;

public class CharaListScanner(IHtmlHandler htmlHandler) : ICharaListScanner
{
	public async Task<CharaListItem[]> ScanCharaList()
	{
		var html = await htmlHandler.ScanHtml(Constants.CharaListPageUrl);

		// table element
		var tbody = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table/tbody");
		var trCollection = tbody.Elements("tr").ToArray();
		// The first element is the header so skip
		CharaListItem[] charaListItems = trCollection.Skip(1).Select((tr) =>
		{
			var character = tr.Elements("td").ToArray()[0].FirstChild.FirstChild;
			if (character == null) throw new Exception("Error Scanning chara list");

			string url = Constants.Domain + character.Attributes.ToArray()[0].Value;
			string name = GetCharaName(url);
			string img = "https:" + character.FirstChild.Attributes.ToArray()[1].Value;
			return new CharaListItem(name, img, url);
		}).ToArray();

		return charaListItems;
	}

	private static string GetCharaName(string url)
	{
		string charaName = url.Split("/wiki/")[1];

		string[] excludedNames = ["Shiroko%EF%BC%8ATerror", "Shiroko_(Terror)"];
		return excludedNames.Contains(charaName) ? "Shiroko" : charaName;
	}
}