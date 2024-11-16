using BlueArchiveWebScrapper.model;

namespace BlueArchiveWebScrapper;
public static partial class CharaInfo
{
  private static readonly string domain = "https://bluearchive.wiki";
  private static readonly string baseUrl = domain + "/wiki/";
  public static async Task<Student?> ScanInfo(string CharaNameParam)
  {
    try
    {
      var html = await HtmlAgility.ScanHtml(baseUrl + CharaNameParam);

      return new Student
      {
        charaName = GetCharaName(html),
        name = GetName(html),
        lastName = GetLastName(html),
        school = GetSchool(html),
        role = GetRole(html),
        combatClass = GetCombatClass(html),
        weaponType = GetWeaponType(html),
        age = GetAge(html),
        birthday = GetBirthday(html),
        height = GetHeight(html),
        hobbies = GetHobbies(html),
        designer = GetDesigner(html),
        illustrator = GetIllustrator(html),
        voice = GetVoice(html),
        releaseDate = GetReleaseDate(html),
        skinSet = GetSkinSet(CharaNameParam),
        pageUrl = GetPageUrl(CharaNameParam),
        pageImageProfileUrl = GetPageImageProfileUrl(html),
        pageImageFullUrl = await GetPageImageFullUrl(html),
        audioUrl = GetAudioUrl(html),
      };
    }
    catch (Exception e)
    {
      Console.WriteLine($"ERROR al escanear la informacion de {CharaNameParam}\n" + e);
      return null;
    }
  }
  public static async Task<Student?[]> ScanManyCharasDetails(string[] CharaNames)
  {
    return await Task.WhenAll(CharaNames.Select(c => ScanInfo(c)).ToArray());
  }

}
