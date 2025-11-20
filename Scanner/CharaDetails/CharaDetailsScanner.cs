using HtmlAgilityPack;

namespace Scanner.CharaDetails;

using Configuration;

using Model;

using Utils;

public class CharaDetailsScanner(IHtmlHandler htmlHandler) : ICharaDetailsScanner
{
	private HtmlDocument? _html = null;
	public async Task<Student> ScanStudentDetails(string charaNameParam)
	{
		try
		{
			var html = await GetHtml(charaNameParam);
			IGetter detailsGetter = new DetailsGetter(html, charaNameParam);

			return new StudentDetailsItem()
			{
				Name = detailsGetter.GetName(),
				LastName = detailsGetter.GetLastName(),
				school = detailsGetter.GetSchool(),
				age = detailsGetter.GetAge(),
				birthday = detailsGetter.GetBirthday(),
				height = detailsGetter.GetHeight(),
				hobbies = detailsGetter.GetHobbies(),
				designer = detailsGetter.GetDesigner(),
				illustrator = detailsGetter.GetIllustrator(),
				voice = detailsGetter.GetVoice(),
				releaseDate = detailsGetter.GetReleaseDate(),
				skinSet = detailsGetter.GetSkinSet(),
				pageUrl = detailsGetter.GetPageUrl(),
				pageImageProfileUrl = detailsGetter.GetPageImageProfileUrl(),
				pageImageFullUrl = await detailsGetter.GetPageImageFullUrl(),
				audioUrl = detailsGetter.GetAudioUrl(),
			};
		}
		catch (Exception)
		{
			Console.WriteLine($"Error trying to scan data of: \"{charaNameParam}\"");
			throw;
		}
	}
	public async Task<IEnumerable<Student>> ScanStudentDetails(IEnumerable<Student> students)
	{
		return await Task.WhenAll(students.Select(s => ScanInfo(s.charaName)));
	}
	public async Task<IEnumerable<Student>> ScanStudentDetails(IEnumerable<StudentListItem> studentListItems)
	{
		return await Task.WhenAll(studentListItems.Select(s => ScanStudentDetails(s.CharaName)));
	}
}