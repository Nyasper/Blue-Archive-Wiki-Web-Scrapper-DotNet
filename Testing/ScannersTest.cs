namespace Testing;

using FluentAssertions;
using Main.Repository;
using Scanner;
using Scanner.CharaDetails;
using Scanner.CharaList;
using Scanner.Configuration;
using Scanner.Model;
using Xunit;

public sealed class ScannersTest
{
	private readonly StudentContext _context;
	private readonly IRepository<Student> _repository;
	private readonly IHtmlHandler _htmlHandler;
	private readonly CharaListScanner _charaListScanner;
	private readonly ICharaDetailsScanner _charaDetailsScanner;
	private readonly IScanner<Student> _scanner;
	

	public ScannersTest()
	{
		// db
		_context = new StudentContext();
		_repository = new Repository(_context);

		// html handler
		_htmlHandler = new HtmlHandler();

		// chara list
		_charaListScanner = new CharaListScanner(_htmlHandler);

		// chara details
		_charaDetailsScanner = new CharaDetailsScanner(_htmlHandler);

		// main scanner
		_scanner = new Scanner(_charaListScanner, _charaDetailsScanner);
	}

	[Fact]
	public async Task CharaList()
	{
		var studentsOnDb = await _repository.GetAll();
		var studentsOnPage = await _charaListScanner.ScanCharaList();

		Console.WriteLine($"Students in DB: {studentsOnDb.Length}\nstudents in page: {studentsOnPage.Length}");
		studentsOnPage.Should().BeAssignableTo<IEnumerable<StudentListItem>>("Should return CharaListItem Collection");
	}

	[Fact]
	public async Task CharaDetails()
	{
		var expected = Constants.StudentToScan;
		StudentDetailsItem studentDetailsItem = await _charaDetailsScanner.ScanStudentDetails(expected.CharaName);
		studentDetailsItem.Should().BeOfType<StudentDetailsItem>("Should return StudentDetailsItem");

		Console.WriteLine(studentDetailsItem.ToString());

		studentDetailsItem.Name.Should().Be(expected.Name,
			because: $"Name should match the expected value '{expected.Name}'");
		studentDetailsItem.LastName.Should().Be(expected.LastName,
			because: $"LastName should match the expected value '{expected.LastName}'");
		studentDetailsItem.Age.Should().Be(expected.Age,
			because: $"Age should match the expected value '{expected.Age}'");
		studentDetailsItem.Height.Should().Be(expected.Height,
			because: $"Height should match the expected value '{expected.Height}'");
		studentDetailsItem.Birthday.Should().Be(expected.Birthday,
			because: $"Birthday should match the expected value '{expected.Birthday}'");
		studentDetailsItem.Hobbies.Should().Be(expected.Hobbies,
			because: $"Hobbies should match the expected value '{expected.Hobbies}'");
		studentDetailsItem.Designer.Should().Be(expected.Designer,
			because: $"Designer should match the expected value '{expected.Designer}'");
		studentDetailsItem.Illustrator.Should().Be(expected.Illustrator,
			because: $"Illustrator should match the expected value '{expected.Illustrator}'");
		studentDetailsItem.Voice.Should().Be(expected.Voice,
			because: $"Voice should match the expected value '{expected.Voice}'");
		studentDetailsItem.ImageProfileUrl.Should().Be(expected.ImageProfileUrl,
			because: $"ImageProfileUrl should match the expected value '{expected.ImageProfileUrl}'");
		studentDetailsItem.ImageFullUrl.Should().Be(expected.ImageFullUrl,
			because: $"ImageFullUrl should match the expected value '{expected.ImageFullUrl}'");
		studentDetailsItem.AudioUrl.Should().Be(expected.AudioUrl,
			because: $"AudioUrl should match the expected value '{expected.AudioUrl}'");
	}

	[Fact]
	public async Task Scanner()
	{
		var expected = Constants.StudentToScan;
		Student student = await _scanner.Scan(expected.CharaName);
		student.Should().BeOfType<Student>("Should return Student");

		Console.WriteLine(student.ToString());

		student.CharaName.Should().Be(expected.CharaName,
			because: $"CharaName should match the expected value '{expected.CharaName}'");
		student.Name.Should().Be(expected.Name,
			because: $"Name should match the expected value '{expected.Name}'");
		student.LastName.Should().Be(expected.LastName,
			because: $"LastName should match the expected value '{expected.LastName}'");
		student.School.Should().Be(expected.School,
			because: $"School should match the expected value '{expected.School}'");
		student.Age.Should().Be(expected.Age,
			because: $"Age should match the expected value '{expected.Age}'");
		student.Height.Should().Be(expected.Height,
			because: $"Height should match the expected value '{expected.Height}'");
		student.Birthday.Should().Be(expected.Birthday,
			because: $"Birthday should match the expected value '{expected.Birthday}'");
		student.Hobbies.Should().Be(expected.Hobbies,
			because: $"Hobbies should match the expected value '{expected.Hobbies}'");
		student.Designer.Should().Be(expected.Designer,
			because: $"Designer should match the expected value '{expected.Designer}'");
		student.Illustrator.Should().Be(expected.Illustrator,
			because: $"Illustrator should match the expected value '{expected.Illustrator}'");
		student.Voice.Should().Be(expected.Voice,
			because: $"Voice should match the expected value '{expected.Voice}'");
		student.ReleaseDate.Should().Be(expected.ReleaseDate,
			because: $"ReleaseDate should match the expected value '{expected.ReleaseDate}'");
		student.SkinSet.Should().Be(expected.SkinSet,
			because: $"SkinSet should match the expected value '{expected.SkinSet}'");
		student.PageUrl.Should().Be(expected.PageUrl,
			because: $"PageUrl should match the expected value '{expected.PageUrl}'");
		student.ImageProfileUrl.Should().Be(expected.ImageProfileUrl,
			because: $"ImageProfileUrl should match the expected value '{expected.ImageProfileUrl}'");
		student.ImageFullUrl.Should().Be(expected.ImageFullUrl,
			because: $"ImageFullUrl should match the expected value '{expected.ImageFullUrl}'");
		student.SmallImageUrl.Should().Be(expected.SmallImageUrl,
			because: $"SmallImageUrl should match the expected value '{expected.SmallImageUrl}'");
		student.AudioUrl.Should().Be(expected.AudioUrl,
			because: $"AudioUrl should match the expected value '{expected.AudioUrl}'");
	}
}