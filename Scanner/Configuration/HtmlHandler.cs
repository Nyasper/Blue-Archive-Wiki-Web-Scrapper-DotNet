namespace Scanner.Configuration;
using HtmlAgilityPack;

public class HtmlHandler : IHtmlHandler
{
	public Task<HtmlDocument> ScanHtml(string url)
	{
		var web = new HtmlWeb();
		var htmlDoc = web.LoadFromWebAsync(url);

		return htmlDoc;
	}
}