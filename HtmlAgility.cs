namespace BlueArchiveWebScrapper;
using HtmlAgilityPack;
public static class HtmlAgility
{
  public static Task<HtmlDocument> ScanHtml(string url)
  {
    var web = new HtmlWeb();
    var htmlDoc = web.LoadFromWebAsync(url);

    return htmlDoc;
  }
}
