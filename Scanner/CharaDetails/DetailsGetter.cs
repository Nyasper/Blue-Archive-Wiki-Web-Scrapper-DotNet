using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Scanner.CharaList;
using Scanner.Configuration;
using Scanner.Model;
using Scanner.Utils;

namespace Scanner.CharaDetails;

using HtmlAgilityPack;

public class DetailsGetter(HtmlDocument html, string studentCharaName, IHtmlHandler? htmlHandler = null) : IDetailsGetter
{
	private readonly string Nl = Environment.NewLine;
	private readonly IHtmlHandler _htmlHandler = htmlHandler ?? new HtmlHandler();

	public (string, string) GetFullName()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Full Name']/following-sibling::td[1]");
			if (node is null) throw new Exception("Full Name th node not found or has no adjacent td");
			string fullName = node.InnerText.Trim();

			if (string.IsNullOrEmpty(fullName))
			{
				throw new Exception("error in 'GetFullName()'" + Nl);
			}

			char asianChar = fullName.FirstOrDefault(UtilsMethods.HasAsianCharacter);
			// if it has an asian char split before.
			fullName = asianChar != '\0' ? fullName.Split(asianChar)[0] : fullName;

			var parts = fullName.Split(' ', 2);
			string lastName = parts[0];
			string name = parts.Length > 1 ? parts[1] : string.Empty;

			return (name, lastName);
		}
		catch (Exception ex)
		{
			throw new Exception("error in 'GetFullName()'" + Nl, ex);
		}
	}
	public int? GetAge()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Age']/following-sibling::td[1]");
			if (node is null) throw new Exception("Age th node not found or has no adjacent td");
			var ageString = node.InnerText.Trim();
			int age = int.Parse(ageString);
			return age;
		}
		catch (FormatException)
		{
			return null;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetAge()'" + Nl);
		}
	}
	public string? GetBirthday()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Birthday']/following-sibling::td[1]");
			if (node is null) throw new Exception("Birthday th node not found or has no adjacent td");
			var birthday = node.InnerText.Trim();
			return birthday == "-" ? null : birthday;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetBirthday()'" + Nl);
		}
	}
	public int? GetHeight()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Height']/following-sibling::td[1]");
			if (node is null) throw new Exception("Height th node not found or has no adjacent td");
			var heightString = node.InnerText.Trim();

			if (!heightString.Contains("cm")) return null;
			int height = int.Parse(heightString.Split("cm")[0]);
			return height;
		}
		catch (FormatException)
		{
			return null;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetHeight()'" + Nl);
		}
	}
	public string GetHobbies()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Hobbies']/following-sibling::td[1]");
			if (node is null) throw new Exception("Hobbies th node not found or has no adjacent td");
			var hobbies = node.InnerText.Trim();

			if (string.IsNullOrEmpty(hobbies))
			{
				throw new Exception("error in 'GetHobbies()'" + Nl);
			}

			return hobbies;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetHobbies()'" + Nl);
		}
	}
	public string? GetDesigner()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Designer']/following-sibling::td[1]");
			if (node is null) throw new Exception("Designer th node not found or has no adjacent td");
			var designer = node.InnerText.Trim().Replace(" ", "_");

			return designer.Contains('-') ? null : designer;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetDesigner()'" + Nl);
		}
	}
	public string? GetIllustrator()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Illustrator']/following-sibling::td[1]");
			if (node is null) throw new Exception("Illustrator th node not found or has no adjacent td");
			var illustrator = node.InnerText.Trim().Replace(" ", "_");

			return illustrator;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetIllustrator()'" + Nl);
		}
	}
	public string GetVoice()
	{
		try
		{
			var node = html.DocumentNode.SelectSingleNode("//th[text()='Voice']/following-sibling::td[1]");
			if (node is null) throw new Exception("Voice th node not found or has no adjacent td");
			var voice = node.InnerText.Trim().Replace(" ", "_");

			if (string.IsNullOrEmpty(voice))
			{
				throw new Exception("error in 'GetVoice()'" + Nl);
			}

			return voice;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetVoice()'" + Nl);
		}
	}
	public string GetPageUrl()
	{
		return Constants.BaseUrl + studentCharaName;
	}
	public string GetImageProfileUrl()
	{
		try
		{
			var imgNode = html.DocumentNode.SelectSingleNode($"//img[@alt='{studentCharaName.Replace("_", " ")}']");
			if (imgNode is null) throw new Exception("Profile image node not found");
			var imageProfileUrl = HtmlEntity.DeEntitize((imgNode.GetAttributeValue("src", "")))?.Trim() ?? "";

			if (string.IsNullOrEmpty(imageProfileUrl)) throw new Exception("error in 'GetImageProfileUrl()'" + Nl);

			return "https:" + imageProfileUrl;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetImageProfileUrl()'" + Nl);
		}
	}
	public async Task<string> GetImageFullUrl()
	{
		try
		{
			var imgNodes = html.DocumentNode.SelectNodes($"//img[@alt='{studentCharaName.Replace("_", " ")}']");
			if (imgNodes is null || imgNodes.Count < 2) throw new Exception("Original full image node not found or count < 2");
			var toOriginalImageFullPageANode = imgNodes[1].ParentNode;
			if (toOriginalImageFullPageANode is null) throw new Exception("Original full image parent anchor not found");
			var toOriginalImageFullPage = Constants.Domain + toOriginalImageFullPageANode.GetAttributeValue("href", "");

			var pageImgFull = await _htmlHandler.ScanHtml(toOriginalImageFullPage);
			var originalFileLink = pageImgFull.DocumentNode.SelectSingleNode("//a[text()='Original file']");
			if (originalFileLink is null) throw new Exception("Original file link not found");
			var imageUrlNode = HtmlEntity.DeEntitize((originalFileLink.GetAttributeValue("href", "")));

			if (string.IsNullOrEmpty(imageUrlNode))
			{
				throw new Exception("error in 'GetImageFullUrl()'" + Nl);
			}

			return "https:" + imageUrlNode;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetImageFullUrl()'" + Nl);
		}
	}
	public string GetAudioUrl()
	{
		try
		{
			var elementWithDataVoiceAttribute = html.DocumentNode.SelectSingleNode("//td[@data-voice]");
			
			if (elementWithDataVoiceAttribute is null) throw new Exception("Audio voice element not found");
			string audioUrl = HtmlEntity.DeEntitize(elementWithDataVoiceAttribute.GetAttributeValue("data-voice", ""));

			if (string.IsNullOrEmpty(audioUrl))
			{
				throw new Exception("error in 'GetAudioUrl()'" + Nl);
			}

			return "https:" + audioUrl;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetAudioUrl()'" + Nl);
		}
	}
}