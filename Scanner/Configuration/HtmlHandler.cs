namespace Scanner.Configuration;
using HtmlAgilityPack;
using Utils;

public class HtmlHandler : IHtmlHandler
{
	public Task<HtmlDocument> ScanHtml(string url)
	{
		return Retry.WithRetryAsync(async () =>
		{
			var web = new HtmlWeb { Timeout = 300_000 };
			return await web.LoadFromWebAsync(url);
		}, $"Fetching HTML: {url}", TimeSpan.FromSeconds(10));
	}
}