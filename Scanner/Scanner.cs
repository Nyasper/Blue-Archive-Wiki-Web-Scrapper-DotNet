using Scanner.CharaDetails;
using Scanner.CharaList;
using Scanner.Model;

namespace Scanner;

public class Scanner(ICharaListScanner charaListScanner, ICharaDetailsScanner charaDetailsScanner) : IScanner<Student>
{
	public async Task<Student> Scan(string nameParam)
	{
		StudentListItem[] studentsListItems = await charaListScanner.ScanCharaList();
		StudentListItem? studentListItem = studentsListItems.FirstOrDefault((s) => string.Equals(s.CharaName, nameParam, StringComparison.OrdinalIgnoreCase));
		if (studentListItem is null) throw new Exception("student_not_found_on_scan");

		StudentDetailsItem studentDetails = await charaDetailsScanner.ScanStudentDetails(studentListItem.CharaName);

		return new Student
		{
			CharaName = studentListItem.CharaName,
			Name = studentDetails.Name,
			LastName = studentDetails.LastName,
			School = studentListItem.School,
			Age = studentDetails.Age,
			Height = studentDetails.Height,
			Birthday = studentDetails.Birthday,
			Hobbies = studentDetails.Hobbies,
			Designer = studentDetails.Designer,
			Illustrator = studentDetails.Illustrator,
			Voice = studentDetails.Voice,
			ReleaseDate = studentListItem.ReleaseDate,
			SkinSet = studentListItem.SkinSet,
			PageUrl = studentListItem.PageUrl,
			ImageProfileUrl = studentDetails.ImageProfileUrl,
			ImageFullUrl = studentDetails.ImageFullUrl,
			SmallImageUrl = studentListItem.SmallImgUrl,
			AudioUrl = studentDetails.AudioUrl,
			CreatedAt = DateTime.UtcNow
		};
	}
	// public async Task<Student[]> ScanAll()
	// {
	// 	StudentListItem[] studentsListItems = await charaListScanner.ScanCharaList();
	// 	StudentDetailsItem[] studentDetailsItems = await charaDetailsScanner.ScanStudentDetails(studentsListItems);
	//
	//
	//
	// 	return studentsListItems.Select((s, i) =>
	// 	{
	// 		StudentDetailsItem details = studentDetailsItems[i];
	// 		return new Student()
	// 		{
	// 			CharaName = s.CharaName,
	// 			Name = details.Name,
	// 			LastName = details.LastName,
	// 			School = s.School,
	// 			Age = details.Age,
	// 			Height = details.Height,
	// 			Birthday = details.Birthday,
	// 			Hobbies = details.Hobbies,
	// 			Designer = details.Designer,
	// 			Illustrator = details.Illustrator,
	// 			Voice = details.Voice,
	// 			ReleaseDate = s.ReleaseDate,
	// 			SkinSet = s.SkinSet,
	// 			PageUrl = s.PageUrl,
	// 			ImageProfileUrl = details.ImageProfileUrl,
	// 			ImageFullUrl = details.ImageFullUrl,
	// 			AudioUrl = details.AudioUrl,
	// 		};
	// 	}).ToArray();
	//
	// }
}