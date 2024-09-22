using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using HtmlAgilityPack;

namespace BlueArchiveWebScrapper
{
  public record class CharaListInfo (string name, string img, string url);

  public class CharaList
  {
    private static readonly string domain = "https://bluearchive.wiki";
    private static readonly string CharaListPageUrl = domain + "/wiki/Characters";

    static public async Task SaveCharaListInJSON()
    {
      List<CharaListInfo> charaList = await GetCharaList();
      string JSONpath = await FileHandler.SaveDataInJSON(model:charaList, FileName:"charaList");
      Console.WriteLine("\nCharaList JSON saved in: "+JSONpath);
    }
    static public async Task<List<CharaListInfo>> GetCharaList()
    {
      var html = await HtmlAgility.ScanHtml(CharaListPageUrl);
      var tbody = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table/tbody");
      var trCollection = tbody.Elements("tr");

      return ScanCharaList(trCollection);
    }
      
    static public async Task LogCharaList() 
    {
      Console.Clear();
      var charaList = await GetCharaList();
      foreach (var chara in charaList) Console.WriteLine($"\nname: {chara.name}\nimg: {chara.img}\nurl: {chara.url}");
      Console.WriteLine($"{Environment.NewLine}{charaList.Count} Characters in total.");
    }

    private static List<CharaListInfo> ScanCharaList(IEnumerable<HtmlNode> trCollection)
    {
      List<CharaListInfo> CharaListInfo = [];
      var trCollectionArr = trCollection.ToArray();
      for (int i = 1; i < trCollectionArr.Length; i++)
      {
        var character = trCollectionArr[i].Elements("td").ToArray()[0].FirstChild.FirstChild;
        string url = domain + character.Attributes.ToArray()[0].Value;
        string name = ManageCharaNameFormat(url);
        string img = "https:" + character.FirstChild.Attributes.ToArray()[1].Value;
        
        CharaListInfo Chara = new (name, img, url);
        CharaListInfo.Add(Chara);
      }
      return CharaListInfo;
    }
    private static string ManageCharaNameFormat(string url)
    {
      string charaName = url.Split("/wiki/")[1];
      if (charaName.Contains("Shiroko%EF%BC%8ATerror")) return "Shiroko_(Terror)";

      return charaName;
    }
  }
}