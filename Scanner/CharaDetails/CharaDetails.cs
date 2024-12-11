namespace Scanner.CharaDetails;
using Configuration;
using Model;
using Utils;

public class CharaDetails(IHtmlHandler htmlHandler) : ICharaDetails<Student>
{
	public async Task<Student> ScanInfo(string charaNameParam)
	{
		try
		{
			var html = await htmlHandler.ScanHtml(Constants.BaseUrl + charaNameParam);
			return new Student
			{
				charaName = Getters.GetCharaName(html),
				name = Getters.GetName(html),
				lastName = Getters.GetLastName(html),
				school = Getters.GetSchool(html),
				role = Getters.GetRole(html),
				combatClass = Getters.GetCombatClass(html),
				weaponType = Getters.GetWeaponType(html),
				age = Getters.GetAge(html),
				birthday = Getters.GetBirthday(html),
				height = Getters.GetHeight(html),
				hobbies = Getters.GetHobbies(html),
				designer = Getters.GetDesigner(html),
				illustrator = Getters.GetIllustrator(html),
				voice = Getters.GetVoice(html),
				releaseDate = Getters.GetReleaseDate(html),
				skinSet = Getters.GetSkinSet(charaNameParam),
				pageUrl = Getters.GetPageUrl(charaNameParam),
				pageImageProfileUrl = Getters.GetPageImageProfileUrl(html),
				pageImageFullUrl = await Getters.GetPageImageFullUrl(html),
				audioUrl = Getters.GetAudioUrl(html),
			};
		}
		catch (Exception)
		{
			Console.WriteLine($"Error trying to scan data of: \"{charaNameParam}\"");
			throw;
		}
	}

	public async Task<IEnumerable<Student>> ScanMany(IEnumerable<CharaListItem> charasItems)
	{
		return await Task.WhenAll(charasItems.Select(s => ScanInfo(s.Name)));
	}
}