﻿namespace Scanner.CharaDetails;

using Configuration;

using HtmlAgilityPack;

using Model;

using Utils;

public static class Getters
{
	private static readonly string Nl = Environment.NewLine;

	public static string GetCharaName(HtmlDocument html)
	{
		try
		{
			var charaName = html.DocumentNode.SelectSingleNode("html/body/div[3]/h1/span").InnerText;
			return charaName.Trim().Replace(" ", "_");
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetImageFullUrl()'" + Nl);
		}
	}
	public static string GetName(HtmlDocument html)
	{
		try
		{
			var name = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[11]/td").InnerText.Trim();
			return name.Split(' ')[1].Split('(')[0];
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetName()'" + Nl);
		}
	}
	public static string GetLastName(HtmlDocument html)
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
	public static string GetSchool(HtmlDocument html)
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
	public static string GetRole(HtmlDocument html)
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
	public static string GetCombatClass(HtmlDocument html)
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
	public static string GetWeaponType(HtmlDocument html)
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
	public static int? GetAge(HtmlDocument html)
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
	public static string? GetBirthday(HtmlDocument html)
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
	public static int? GetHeight(HtmlDocument html)
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
	public static string GetHobbies(HtmlDocument html)
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
	public static string? GetDesigner(HtmlDocument html)
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
	public static string? GetIllustrator(HtmlDocument html)
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
	public static string GetVoice(HtmlDocument html)
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
	public static string GetReleaseDate(HtmlDocument html)
	{
		try
		{
			var releaseDate = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[19]/td").InnerText.Trim().Replace("/", "-");
			return releaseDate;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetReleaseDate()'" + Nl);
		}
	}
	public static string GetSkinSet(string CharaName)
	{
		try
		{
			if (!CharaName.EndsWith(')') || !CharaName.Contains("_(") || Student.ExcludeSkinSets.Contains(CharaName)) return "default";
			return CharaName.Split('(')[1].Split(')')[0].Trim().ToLower();
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetSkinSet()'" + Nl);
		}
	}
	public static string GetPageUrl(string charaName)
	{
		return Constants.BaseUrl + charaName;
	}
	public static string GetPageImageProfileUrl(HtmlDocument html)
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
	public static async Task<string> GetPageImageFullUrl(HtmlDocument html)
	{
		Console.WriteLine("scanning page image full url...");
		try
		{
			const string aElementXPath = "/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[2]/td/div[2]/div/section/article[2]/figure/a";
			string toOriginalImageHrefAttr = html.DocumentNode.SelectSingleNode(aElementXPath).GetAttributes("href").First().Value;
			if (String.IsNullOrEmpty(toOriginalImageHrefAttr)) throw new Exception("error in 'GetImageFullUrl()'" + Nl);

			string toOriginalImageUrl = $"{Constants.Domain}{toOriginalImageHrefAttr}";

			HtmlDocument imgFullHtml = await new HtmlHandler().ScanHtml(toOriginalImageUrl);
			const string originalImageUrlXpath = "/html/body/div[3]/div[3]/div[5]/div[2]/p/bdi/a";
			string originalImageHref = imgFullHtml.DocumentNode.SelectSingleNode(originalImageUrlXpath).GetAttributes("href").First().Value;
			if (String.IsNullOrEmpty(originalImageHref)) throw new Exception("error in 'GetImageFullUrl()'" + Nl);
			Console.WriteLine($"original image href: {originalImageHref}");
			string result = "https:" + originalImageHref;
			return result;
		}
		catch (Exception)
		{
			throw new Exception("error in 'GetImageFullUrl()'" + Nl);
		}
	}
	public static string GetAudioUrl(HtmlDocument html)
	{
		try
		{
			var AudioElement = html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[18]/td");
			string AudioUrl = AudioElement.GetAttributes("data-voice").First().Value;
			if (string.IsNullOrEmpty(AudioUrl)) throw new Exception("error in 'GetAudioUrl()'" + Nl);
			return "https:" + AudioUrl;
		}
		catch (Exception)
		{

			throw new Exception("error in 'GetAudioUrl()'" + Nl);
		}
	}
}