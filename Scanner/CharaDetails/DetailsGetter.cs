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

public class DetailsGetter(HtmlDocument html, string studentCharaName) : IDetailsGetter
{
	private readonly string Nl = Environment.NewLine;
	public (string, string) GetFullName()
	{
		try
		{
			var thFullNameNode = html.DocumentNode.SelectSingleNode("//th[text()='Full Name']");
			string fullName = thFullNameNode.NextSibling.InnerText.Trim();

			if (string.IsNullOrEmpty(fullName))
			{
				throw new Exception("error in 'GetFullName()'" + Nl);
			}

			char asianChar = fullName.FirstOrDefault(Main.Utils.UtilsMethods.HasAsianCharacter);
			// if it has an asian char split before.
			fullName = asianChar != '\0' ? fullName.Split(asianChar)[0] : fullName;

			string lastName = fullName.Split(' ')[0];
			string name = fullName.Split(' ')[1];

			return (name, lastName);
		}
		catch (Exception ex)
		{
			throw new Exception("error in 'GetFullName()'" + Nl, ex);
		}
	}
	public int? GetAge()
	{
		var thAgeNode = html.DocumentNode.SelectSingleNode("//th[text()='Age']");
		var ageString = thAgeNode.NextSibling.InnerText.Trim();
		try
		{
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
			var thBirthdayNode = html.DocumentNode.SelectSingleNode("//th[text()='Birthday']");
			var birthday = thBirthdayNode.NextSibling.InnerText.Trim();
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
			var thHeightNode = html.DocumentNode.SelectSingleNode("//th[text()='Height']");
			var heightString = thHeightNode.NextSibling.InnerText.Trim();

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
			var thHobbiesNode = html.DocumentNode.SelectSingleNode("//th[text()='Hobbies']");
			var hobbies = thHobbiesNode.NextSibling.InnerText.Trim();

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
			var thDesignerNode = html.DocumentNode.SelectSingleNode("//th[text()='Designer']");
			var designer = thDesignerNode.NextSibling.InnerText.Trim().Replace(" ", "_");

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
			var thIllustratorNode = html.DocumentNode.SelectSingleNode("//th[text()='Illustrator']");
			var illustrator = thIllustratorNode.NextSibling.InnerText.Trim().Replace(" ", "_");

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
			var thVoiceNode = html.DocumentNode.SelectSingleNode("//th[text()='Voice']");
			var voice = thVoiceNode.NextSibling.InnerText.Trim().Replace(" ", "_");

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
			var allImages = html.DocumentNode.SelectSingleNode($"//img[@alt='{studentCharaName.Replace("_", " ")}']");
			var imageProfileUrl = allImages.GetAttributeValue("src", "")?.Trim() ?? "";

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
			var toOriginalImageFullPageANode = html.DocumentNode.SelectNodes($"//img[@alt='{studentCharaName.Replace("_", " ")}']")[1].ParentNode;
			var toOriginalImageFullPage = Constants.Domain + toOriginalImageFullPageANode.GetAttributeValue("href", "");

			var pageImgFull = await new HtmlHandler().ScanHtml(toOriginalImageFullPage);
			var imageUrlNode = pageImgFull.DocumentNode.SelectSingleNode("//a[text()='Original file']").GetAttributeValue("href", "");

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
			var elementWithDataVoiceAttribute =  html.DocumentNode.SelectSingleNode("//td[@data-voice]");
			string audioUrl = elementWithDataVoiceAttribute.GetAttributeValue("data-voice", "");

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