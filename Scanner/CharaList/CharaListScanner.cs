namespace Scanner.CharaList;
using Configuration;
using Model;
using Utils;
using HtmlAgilityPack;

public class CharaListScanner(IHtmlHandler htmlHandler) : ICharaListScanner
{
	public async Task<HashSet<CharaListItem>> ScanCharaList()
	{
		var html = await htmlHandler.ScanHtml(Constants.CharaListPageUrl);
		var tbody = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table/tbody");
		var trCollection = tbody.Elements("tr");

		return ScanCharaList(trCollection);
	}
	private static HashSet<CharaListItem> ScanCharaList(IEnumerable<HtmlNode> trCollection)
	{
		HashSet<CharaListItem> charaListItems = [];
		HtmlNode[] trCollectionArr = trCollection.ToArray();
		for (int i = 1; i < trCollectionArr.Length; i++)
		{
			var character = trCollectionArr[i].Elements("td").ToArray()[0].FirstChild.FirstChild;
			string url = Constants.Domain + character.Attributes.ToArray()[0].Value;
			string name = GetCharaName(url);
			string img = "https:" + character.FirstChild.Attributes.ToArray()[1].Value;

			CharaListItem chara = new(name, img, url);
			charaListItems.Add(chara);
		}
		return charaListItems;
	}
	private static string GetCharaName(string url)
	{
		string charaName = url.Split("/wiki/")[1];

		string[] exludeNames = ["Shiroko%EF%BC%8ATerror"];
		return exludeNames.Contains(charaName) ? "Shiroko" : charaName;
	}
}