using System.ComponentModel;

using Scanner.CharaList;
using Scanner.Configuration;
using Scanner.Model;
using Scanner.Utils;

namespace Scanner.CharaDetails;

using HtmlAgilityPack;

/*


 TODO: What I need to scann from details page:
 - name
 - lastName
 - age
 - height
 - birthday
 - hobbies
 - designer
 - illustrator
 - voice
 - urls
 */

public class Getter(HtmlDocument html, string studentCharaName) : IGetter
{
	private readonly string Nl = Environment.NewLine;

	public string GetCharaName() => studentCharaName;

	public string GetName()
	{
		static string ExtractName(string name) => name.Split(' ')[1].Split('(')[0];

		try
		{
			var name = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[11]/td").InnerText.Trim();

			char asianChar = name.FirstOrDefault(Main.Utils.UtilsMethods.HasAsianCharacter);
			// if it has an asian char split before.
			if (asianChar != '\0')
			{
				return ExtractName(name).Split(asianChar)[0];
			}

			return ExtractName(name);
		}
		catch (Exception ex)
		{
			throw new Exception("error in 'GetName()'" + Nl, ex);
		}
	}

	public string GetLastName()
	{
		try
		{
			var lastName = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[11]/td").InnerText;
			return lastName.Split(' ')[0].Trim();
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetLastName()'" + Nl);
		}
	}

	public string GetSchool()
	{
		try
		{
			var school = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[4]/td[1]").InnerText.Trim();

			return Student.Schools.Contains(school) ? school : "other";
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetSchool()'" + Nl);
		}
	}

	public string GetRole()
	{
		try
		{
			var role = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[4]/td[2]").InnerText.Trim();
			return role.Replace("/", "_");
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetRole()'" + Nl);
		}
	}

	public string GetCombatClass()
	{
		try
		{
			var combatClass = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[7]/td").InnerText;
			return combatClass;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetCombatClass()'" + Nl);
		}
	}

	public string GetWeaponType()
	{
		try
		{
			var weaponType = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[9]/td/table/tbody/tr/td[1]/div").InnerText;
			return weaponType;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetWeaponType()'" + Nl);
		}
	}

	public int? GetAge()
	{
		var AgeString = html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[12]/td").InnerText.Trim();
		try
		{
			int age = int.Parse(AgeString);
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
			var birthday = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[13]/td").InnerText.Trim();
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
			string heightString = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[14]/td").InnerText.Trim();
			if (!heightString.Contains("cm")) return null;
			int height = int.Parse(heightString.Split("cm")[0]);
			return height;
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
			var hobbies = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[15]/td").InnerText.Trim();
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
			var designer = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[16]/td").InnerText.Trim().Replace(" ", "_");
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
			var illustrator = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[17]/td").InnerText.Trim().Replace(" ", "_");
			return illustrator.Contains('-') ? null : illustrator;
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
			var voice = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[18]/td").InnerHtml.Trim();
			return voice.Replace(" ", "_");
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetVoice()'" + Nl);
		}
	}

	public string GetReleaseDate()
	{
		try
		{
			var releaseDateNode = html.DocumentNode.SelectSingleNode("//th[normalize-space(text()) = 'Release Date JP']/following-sibling::td[1]");
			if (string.IsNullOrEmpty(releaseDateNode.InnerText)) throw new Exception("error in 'GetReleaseDate()'" + Nl);

			var releaseDate = releaseDateNode.InnerText.Trim().Replace("/", "-");
			return releaseDate;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetReleaseDate()'" + Nl);
		}
	}

	public string GetSkinSet()
	{
		try
		{
			if (!studentCharaName.EndsWith(')') || !studentCharaName.Contains("_(") || Student.ExcludeSkinSets.Contains(studentCharaName)) return "default";
			return studentCharaName.Split('(')[1].Split(')')[0].Trim().ToLower();
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetSkinSet()'" + Nl);
		}
	}

	public string GetPageUrl()
	{
		return Constants.BaseUrl + studentCharaName;
	}

	public string GetPageImageProfileUrl()
	{
		try
		{
			var imageProfileUrl = html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[2]/td/div[2]/div/section/article[1]/figure/a/img");
			var srcAttr = imageProfileUrl.GetAttributes("src").First().Value;
			if (string.IsNullOrEmpty(srcAttr)) throw new Exception("error in 'GetImageFullUrl()'" + Nl);

			return "https:" + srcAttr;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetImageProfileUrl()'" + Nl);
		}
	}

	public async Task<string> GetPageImageFullUrl()
	{
		Console.WriteLine("scanning page image full url...");
		try
		{
			const string aElementXPath = "/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[2]/td/div[2]/div/section/article[2]/figure/a";
			string toOriginalImageHrefAttr = html.DocumentNode.SelectSingleNode(aElementXPath).GetAttributes("href").First().Value;
			if (String.IsNullOrEmpty(toOriginalImageHrefAttr)) throw new Exception("error in 'GetImageFullUrl()'" + Nl);

			string toOriginalImageUrl = $"{Constants.Domain}{toOriginalImageHrefAttr}";

			var imgFull_html = await new HtmlHandler().ScanHtml(toOriginalImageUrl);
			const string originalImageUrlXpath = "/html/body/div[3]/div[3]/div[5]/div[2]/p/bdi/a";
			string originalImageHref = imgFull_html.DocumentNode.SelectSingleNode(originalImageUrlXpath).GetAttributes("href").First().Value;
			if (String.IsNullOrEmpty(originalImageHref)) throw new Exception("error in 'GetImageFullUrl()'" + Nl);
			string result = "https:" + originalImageHref;
			return result;
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
			// este es el audioElement original
			var elementWithDataVoiceAttribute =  html.DocumentNode.SelectSingleNode("//*[@data-voice]");
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