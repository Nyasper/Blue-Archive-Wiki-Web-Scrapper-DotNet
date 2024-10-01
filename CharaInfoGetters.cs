using BlueArchiveWebScrapper.model;

using HtmlAgilityPack;
namespace BlueArchiveWebScrapper;

public partial class CharaInfo
{
  private static string GetCharaName(HtmlDocument html)
  {
    try
    {
      var CharaName = html.DocumentNode.SelectSingleNode("html/body/div[3]/h1/span").InnerText;
      return CharaName.Trim().Replace(" ", "_");
    }
    catch (Exception)
    {
      throw new Exception("error en 'GetImageFullUrl()'\n");
    }
  }
  private static string GetName(HtmlDocument html)
  {
    try
    {
      var Name = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[11]/td").InnerText.Trim();
      return Name.Split(' ')[1].Split('(')[0];
    }
    catch (Exception)
    {
      throw new Exception("Error en 'GetName()'\n");
    }
  }
  private static string GetLastName(HtmlDocument html)
  {
    try
    {
      var LastName = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[11]/td").InnerText;
      return LastName.Split(' ')[0].Trim();
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetLastName()'\n");
    }
  }
  private static string GetSchool(HtmlDocument html)
  {
    try
    {
      var School = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[4]/td[1]").InnerText.Trim();
      if (Student.Schools.FirstOrDefault(s => s == School) == null)
      {
        return "other";
      }
      return School;
    }
    catch (Exception)
    {
      throw new Exception("error en 'GetSchool()'\n");
    }
  }
  private static string GetRole(HtmlDocument html)
  {
    try
    {
      var Role = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[4]/td[2]").InnerText.Trim();
      return Role.Replace("/", "_");
    }
    catch (Exception)
    {
      throw new Exception("Error en 'GetRole()'\n");
    }
  }
  private static string GetCombatClass(HtmlDocument html)
  {
    try
    {
      var CombatClass = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[7]/td").InnerText;
      return CombatClass;
    }
    catch (Exception)
    {
      throw new Exception("Error en 'GetCombatClass()'\n");
    }
  }
  private static string GetWeaponType(HtmlDocument html)
  {
    try
    {
      var WeaponType = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[9]/td/table/tbody/tr/td[1]/div").InnerText;
      return WeaponType;
    }
    catch (Exception)
    {
      throw new Exception("Error en 'GetWeaponType()'\n");
    }
  }
  private static int? GetAge(HtmlAgilityPack.HtmlDocument html)
  {
    var AgeString = html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[12]/td").InnerText.Trim();
    try
    {
      var Age = int.Parse(AgeString);
      return Age;
    }
    catch (FormatException)
    {
      return null;
    }
    catch (Exception)
    {
      throw new Exception("error en 'GetAge()'\n");
    }
  }
  private static string? GetBirthday(HtmlDocument html)
  {
    try
    {
      var Birthday = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[13]/td").InnerText.Trim();
      if (Birthday == "-") return null;
      return Birthday;
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetBirthday()'\n");
    }
  }
  private static int? GetHeight(HtmlDocument html)
  {
    try
    {
      string HeightString = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[14]/td").InnerText.Trim();
      if (HeightString.Contains("cm"))
      {
        int Height = int.Parse(HeightString.Split("cm")[0]);
        return Height;
      }
      return null;
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetHeight()'\n");
    }
  }
  private static string GetHobbies(HtmlDocument html)
  {
    try
    {
      var Hobbies = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[15]/td").InnerText.Trim();
      return Hobbies;
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetHobbies()'\n");
    }
  }
  private static string? GetDesigner(HtmlDocument html)
  {
    try
    {
      var Designer = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[16]/td").InnerText.Trim().Replace(" ", "_");
      if (Designer.Contains('-')) return null;
      return Designer;
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetDesigner()'\n");
    }
  }
  private static string? GetIllustrator(HtmlDocument html)
  {
    try
    {
      var Illustrator = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[17]/td").InnerText.Trim().Replace(" ", "_");
      if (Illustrator.Contains('-')) return null;
      return Illustrator;
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetIllustrator()'\n");
    }
  }
  private static string GetVoice(HtmlDocument html)
  {
    try
    {
      var Voice = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[18]/td").InnerHtml.Trim();
      return Voice.Replace(" ", "_");
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetVoice()'\n");
    }
  }
  private static string GetReleaseDate(HtmlDocument html)
  {
    try
    {
      var ReleaseDate = html.DocumentNode.SelectSingleNode("/html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[19]/td").InnerText.Trim().Replace("/", "-");
      return ReleaseDate;
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetReleaseDate()'\n");
    }
  }
  private static string GetSkinSet(string CharaName)
  {
    try
    {
      string SkinSet = "default";
      if (CharaName.EndsWith(')') && CharaName.Contains("_("))
      {
        string ExtractSkinSet = CharaName.Split('(')[1].Split(')')[0].Trim().ToLower();
        if (Student.NoSkinSet.FirstOrDefault(s => s == ExtractSkinSet) == null)
        {
          SkinSet = ExtractSkinSet;
        }
      }
      return SkinSet;
    }
    catch (Exception)
    {

      throw new Exception("Error en 'GetSkinSet()'\n");
    }
  }
  private static string GetPageUrl(string CharaName)
  {
    return baseUrl + CharaName;
  }
  private static string GetPageImageProfileUrl(HtmlDocument html)
  {
    try
    {
      var ImageProfileUrl = html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[2]/td/div[2]/div/section/article[1]/figure/a/img");
      var SrcAttr = ImageProfileUrl.Attributes.FirstOrDefault(atr => atr.Name == "src")?.Value;
      return string.IsNullOrEmpty(SrcAttr) ? "" : "https:" + SrcAttr;
    }
    catch (Exception)
    {
      throw new Exception("error en 'GetImageProfileUrl()'\n");
    }
  }
  private static async Task<string> GetPageImageFullUrl(HtmlDocument html)
  {
    try
    {
      var ImageFullAnchorElement =
        html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[2]/td/div[2]/div/section/article[2]/div/div/section/article[1]/figure/a") ?? html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[2]/td/div[2]/div/section/article[2]/figure/a");

      var AnchorHref = ImageFullAnchorElement.Attributes.FirstOrDefault(atr => atr.Name == "href")?.Value;
      if (AnchorHref != null)
      {
        AnchorHref = domain + AnchorHref;
        var imgFullHtml = await HtmlAgility.ScanHtml(AnchorHref);
        var ImgFullAnchor = imgFullHtml.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[2]/p/a");
        var ImgFullUrl = ImgFullAnchor.Attributes.FirstOrDefault(attr => attr.Name == "href")?.Value;
        return string.IsNullOrEmpty(ImgFullUrl) ? "" : "https:" + ImgFullUrl;
      }
      return "";
    }
    catch (Exception)
    {
      // Console.WriteLine("ERROR al intentar obtener 'ImageFullUrl'\n");
      // return "";
      throw new Exception("error en 'GetImageFullUrl()'\n");
    }
  }
  private static string GetAudioUrl(HtmlDocument html)
  {
    try
    {
      var AudioElement = html.DocumentNode.SelectSingleNode("html/body/div[3]/div[3]/div[5]/div[1]/table[1]/tbody/tr[18]/td");
      string? AudioUrl = AudioElement.GetAttributes().FirstOrDefault(atr => atr.Name == "data-voice")?.Value;
      return string.IsNullOrEmpty(AudioUrl) ? "" : "https:" + AudioUrl;
    }
    catch (Exception)
    {

      throw new Exception("error en 'GetAudioUrl()'\n");
    }
  }
}
