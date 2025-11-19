namespace Scanner.CharaDetails;

using Configuration;

using Model;

using Utils;

public class CharaDetailsScanner(IHtmlHandler htmlHandler) : ICharaDetailsScanner
{
	public async Task<Student> ScanInfo(string charaNameParam)
	{
		try
		{
			var html = await htmlHandler.ScanHtml(Constants.BaseUrl + charaNameParam);
			IGetter getter = new Getter(html, charaNameParam);

			return new Student
			{
				charaName = getter.GetCharaName(),
				name = getter.GetName(),
				lastName = getter.GetLastName(),
				school = getter.GetSchool(),
				role = getter.GetRole(),
				combatClass = getter.GetCombatClass(),
				weaponType = getter.GetWeaponType(),
				age = getter.GetAge(),
				birthday = getter.GetBirthday(),
				height = getter.GetHeight(),
				hobbies = getter.GetHobbies(),
				designer = getter.GetDesigner(),
				illustrator = getter.GetIllustrator(),
				voice = getter.GetVoice(),
				releaseDate = getter.GetReleaseDate(),
				skinSet = getter.GetSkinSet(),
				pageUrl = getter.GetPageUrl(),
				pageImageProfileUrl = getter.GetPageImageProfileUrl(),
				pageImageFullUrl = await getter.GetPageImageFullUrl(),
				audioUrl = getter.GetAudioUrl(),
			};
		}
		catch (Exception)
		{
			Console.WriteLine($"Error trying to scan data of: \"{charaNameParam}\"");
			throw;
		}
	}
	public async Task<IEnumerable<Student>> ScanInfo(IEnumerable<Student> students)
	{
		return await Task.WhenAll(students.Select(s => ScanInfo(s.charaName)));
	}
	public async Task<IEnumerable<Student>> ScanInfo(IEnumerable<CharaListStudent> charasItems)
	{
		return await Task.WhenAll(charasItems.Select(s => ScanInfo(s.CharaName)));
	}
}