namespace Scanner.Configuration;
using HtmlAgilityPack;

public interface IHtmlHandler
{
	Task<HtmlDocument> ScanHtml(string url);
}