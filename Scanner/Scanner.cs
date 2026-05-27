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
}