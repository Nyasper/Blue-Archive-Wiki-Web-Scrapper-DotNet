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
	private const string StudentToScan = "Eimi_(Swimsuit)";

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
		StudentDetailsItem studentDetailsItem = await _charaDetailsScanner.ScanStudentDetails(StudentToScan);
		studentDetailsItem.Should().BeOfType<StudentDetailsItem>("Should return StudentDetailsItem");

		Console.WriteLine(studentDetailsItem.ToString());
	}

	[Fact]
	public async Task Scanner()
	{
		Student student = await _scanner.Scan(StudentToScan);
		student.Should().BeOfType<Student>("Should return Student");
		
		Console.WriteLine(student.ToString());
	}
}