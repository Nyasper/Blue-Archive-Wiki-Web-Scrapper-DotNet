using Scanner.Model;
using static Scanner.Utils.Constants;

namespace Main.Extensions;

public static class StudentExtensions
{
	extension(StudentListItem[] listItems)
	{
		public Student[] ScanDetails(StudentDetailsItem[] studentDetails)
		{
			return listItems.Select((s, i) =>
			{
				if (i >= studentDetails.Length)
				{
					return new Student()
					{
						CharaName = s.CharaName,
						Name = DefaultCharaDetailsFields.Name,
						LastName = DefaultCharaDetailsFields.LastName,
						School = s.School,
						Age = DefaultCharaDetailsFields.Age,
						Height = DefaultCharaDetailsFields.Height,
						Birthday = DefaultCharaDetailsFields.Birthday,
						Hobbies = DefaultCharaDetailsFields.Hobbies,
						Designer = DefaultCharaDetailsFields.Designer,
						Illustrator = DefaultCharaDetailsFields.Illustrator,
						Voice = DefaultCharaDetailsFields.Voice,
						ReleaseDate = s.ReleaseDate,
						SkinSet = s.SkinSet,
						PageUrl = s.PageUrl,
						ImageProfileUrl = DefaultCharaDetailsFields.ImageProfileUrl,
						ImageFullUrl = DefaultCharaDetailsFields.ImageFullUrl,
						SmallImageUrl =  s.SmallImgUrl,
						AudioUrl = DefaultCharaDetailsFields.AudioUrl,
					};
				}
				StudentDetailsItem details = studentDetails[i];
				return new Student()
				{
					CharaName = s.CharaName,
					Name = details.Name,
					LastName = details.LastName,
					School = s.School,
					Age = details.Age,
					Height = details.Height,
					Birthday = details.Birthday,
					Hobbies = details.Hobbies,
					Designer = details.Designer,
					Illustrator = details.Illustrator,
					Voice = details.Voice,
					ReleaseDate = s.ReleaseDate,
					SkinSet = s.SkinSet,
					PageUrl = s.PageUrl,
					ImageProfileUrl = details.ImageProfileUrl,
					ImageFullUrl = details.ImageFullUrl,
					SmallImageUrl =  s.SmallImgUrl,
					AudioUrl = details.AudioUrl,
				};
			}).ToArray();
		}
		public static Student[] operator +(StudentListItem[] studentList, StudentDetailsItem[] studentDetails)
		{
			return studentList.Select((s, i) =>
			{
				if (i >= studentDetails.Length)
				{
					return new Student()
					{
						CharaName = s.CharaName,
						Name = DefaultCharaDetailsFields.Name,
						LastName = DefaultCharaDetailsFields.LastName,
						School = s.School,
						Age = DefaultCharaDetailsFields.Age,
						Height = DefaultCharaDetailsFields.Height,
						Birthday = DefaultCharaDetailsFields.Birthday,
						Hobbies = DefaultCharaDetailsFields.Hobbies,
						Designer = DefaultCharaDetailsFields.Designer,
						Illustrator = DefaultCharaDetailsFields.Illustrator,
						Voice = DefaultCharaDetailsFields.Voice,
						ReleaseDate = s.ReleaseDate,
						SkinSet = s.SkinSet,
						PageUrl = s.PageUrl,
						ImageProfileUrl = DefaultCharaDetailsFields.ImageProfileUrl,
						ImageFullUrl = DefaultCharaDetailsFields.ImageFullUrl,
						SmallImageUrl = s.SmallImgUrl,
						AudioUrl = DefaultCharaDetailsFields.AudioUrl,
					};
				}
				StudentDetailsItem details = studentDetails[i];
				return new Student()
				{
					CharaName = s.CharaName,
					Name = details.Name,
					LastName = details.LastName,
					School = s.School,
					Age = details.Age,
					Height = details.Height,
					Birthday = details.Birthday,
					Hobbies = details.Hobbies,
					Designer = details.Designer,
					Illustrator = details.Illustrator,
					Voice = details.Voice,
					ReleaseDate = s.ReleaseDate,
					SkinSet = s.SkinSet,
					PageUrl = s.PageUrl,
					ImageProfileUrl = details.ImageProfileUrl,
					ImageFullUrl = details.ImageFullUrl,
					SmallImageUrl = s.SmallImgUrl,
					AudioUrl = details.AudioUrl,
				};
			}).ToArray();
		}
	}
}