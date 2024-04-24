namespace BlueArchiveWebScrapper;
using HtmlAgilityPack;
public class HtmlAgility
{
    public static Task<HtmlDocument> ScanHtml(string url)
  {
      var web = new HtmlWeb();
      var htmlDoc = web.LoadFromWebAsync(url);

      return htmlDoc;
  }
}
