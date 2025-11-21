using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace Scanner.CharaDetails;

using Configuration;

using Model;

using Utils;

public class CharaDetailsScanner(IHtmlHandler htmlHandler) : ICharaDetailsScanner
{
	public async Task<StudentDetailsItem> ScanStudentDetails(string charaNameParam)
	{
		try
		{
			var html = await htmlHandler.ScanHtml(charaNameParam);
			IGetter detailsGetter = new DetailsGetter(html, charaNameParam);

			return new StudentDetailsItem
			{
				Name = detailsGetter.GetName(),
				LastName = detailsGetter.GetLastName(),
				Age = detailsGetter.GetAge(),
				Birthday = detailsGetter.GetBirthday(),
				Height = detailsGetter.GetHeight(),
				Hobbies = detailsGetter.GetHobbies(),
				Designer = detailsGetter.GetDesigner(),
				Illustrator = detailsGetter.GetIllustrator(),
				Voice = detailsGetter.GetVoice(),
				ImageProfileUrl = detailsGetter.GetPageImageProfileUrl(),
				ImageFullUrl = await detailsGetter.GetPageImageFullUrl(),
				AudioUrl = detailsGetter.GetAudioUrl(),
			};
		}
		catch (Exception)
		{
			Console.WriteLine($"Error trying to scan data of: \"{charaNameParam}\"");
			throw;
		}
	}
	public async Task<StudentDetailsItem[]> ScanStudentDetails(IEnumerable<StudentListItem> students)
	{
		return await Task.WhenAll(students.Select(s => ScanStudentDetails(s.CharaName)));
	}
	public async Task<StudentDetailsItem[]> ScanStudentDetails(IEnumerable<Student> students)
	{
		return await Task.WhenAll(students.Select(s => ScanStudentDetails(s.CharaName)));
	}
}