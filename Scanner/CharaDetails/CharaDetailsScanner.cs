using System;
using System.Collections.Concurrent;
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
			var url = Constants.BaseUrl + charaNameParam;
			var html = await htmlHandler.ScanHtml(url);
			IDetailsGetter detailsGetter = new DetailsGetter(html, charaNameParam, htmlHandler);
			(string name, string lastName) = detailsGetter.GetFullName();


			return new StudentDetailsItem
			{
				Name = name,
				LastName = lastName,
				Age = detailsGetter.GetAge(),
				Birthday = detailsGetter.GetBirthday(),
				Height = detailsGetter.GetHeight(),
				Hobbies = detailsGetter.GetHobbies(),
				Designer = detailsGetter.GetDesigner(),
				Illustrator = detailsGetter.GetIllustrator(),
				Voice = detailsGetter.GetVoice(),
				ImageProfileUrl = detailsGetter.GetImageProfileUrl(),
				ImageFullUrl = await detailsGetter.GetImageFullUrl(),
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
		var results = new ConcurrentBag<StudentDetailsItem>();
		await Parallel.ForEachAsync(students,
			new ParallelOptions { MaxDegreeOfParallelism = 4 },
			async (student, _) =>
			{
				StudentDetailsItem item = await ScanStudentDetails(student.CharaName);
				results.Add(item);
			});
		return results.ToArray();
	}
}