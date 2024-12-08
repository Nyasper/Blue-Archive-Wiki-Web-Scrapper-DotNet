using HtmlAgilityPack;

namespace BlueArchiveWebScrapper
{
  public record class CharaListInfo(string name, string img, string url);

  public class CharaList
  {
    private static readonly string domain = "https://bluearchive.wiki";
    private static readonly string CharaListPageUrl = domain + "/wiki/Characters";

    static public async Task SaveCharaListInJSON()
    {
      var charaList = await GetCharaList();
      string JSONpath = await FileHandler.SaveDataInJSON(model: charaList, FileName: "charaList");
      Console.WriteLine("\nCharaList JSON saved in: " + JSONpath);
    }
    static public async Task<HashSet<CharaListInfo>> GetCharaList()
    {
      var html = await HtmlAgility.ScanHtml(CharaListPageUrl);
      var tbody = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table/tbody");
      var trCollection = tbody.Elements("tr");

      return ScanCharaList(trCollection);
    }

    static public async Task LogCharaList()
    {
      Notifier.NewBlankMessage("Showing Chara List");
      var charaList = await GetCharaList();
      foreach (var chara in charaList) Console.WriteLine($"\nname: {chara.name}\nimg: {chara.img}\nurl: {chara.url}");
      Console.WriteLine($"{Environment.NewLine}{charaList.Count} Characters in total.");
    }

    private static HashSet<CharaListInfo> ScanCharaList(IEnumerable<HtmlNode> trCollection)
    {
      HashSet<CharaListInfo> CharaListInfo = [];
      var trCollectionArr = trCollection.ToArray();
      for (int i = 1; i < trCollectionArr.Length; i++)
      {
        var character = trCollectionArr[i].Elements("td").ToArray()[0].FirstChild.FirstChild;
        string url = domain + character.Attributes.ToArray()[0].Value;
        string name = GetCharaName(url);
        string img = "https:" + character.FirstChild.Attributes.ToArray()[1].Value;

        CharaListInfo Chara = new(name, img, url);
        CharaListInfo.Add(Chara);
      }
      return CharaListInfo;
    }
    private static string GetCharaName(string url)
    {
      string charaName = url.Split("/wiki/")[1];

      string[] exludeNames = ["Shiroko%EF%BC%8ATerror"];
      return exludeNames.Contains(charaName) ? "Shiroko" : charaName;
    }
  }
}