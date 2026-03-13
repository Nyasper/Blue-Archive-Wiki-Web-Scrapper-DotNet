namespace Testing;
using Main.Repository;
using Scanner;
using Scanner.CharaDetails;
using Scanner.CharaList;
using Scanner.Configuration;
using Scanner.Model;

[TestClass]
public sealed class ScannersTest
{
	private StudentContext _context;
	private IRepository<Student> _repository;
	private IHtmlHandler _htmlHandler;
	private CharaListScanner _charaListScanner;
	private ICharaDetailsScanner _charaDetailsScanner;
	private IScanner<Student> _scanner;

	[TestInitialize]
	public void Setup()
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

	[TestMethod]
	public async Task CharaList()
	{
		var studentsOnDb = await _repository.GetAll();
		var studentsOnPage = await _charaListScanner.ScanCharaList();
		var s = studentsOnPage.FirstOrDefault(s => s.CharaName == "Shiroko*Terror");
		if (s is null)
		{
			Console.WriteLine("Shiroko terror doesnt found");
		}
		else
		{
			Console.WriteLine(s);
		}

		Assert.IsInstanceOfType<IEnumerable<StudentListItem>>(studentsOnPage, "Should return CharaListItem Collection");
		Assert.IsGreaterThanOrEqualTo(studentsOnDb.Length, studentsOnPage.Length, "The number of characters in the page should be at least the number of Students");
		Console.WriteLine($"Students in DB: {studentsOnDb.Length}\nstudents in page: {studentsOnPage.Length}");
	}

	[TestMethod]
	public async Task CharaDetails()
	{
		const string studentToScan = "Yuuka_(Pajama)";

		StudentDetailsItem studentDetailsItem = await _charaDetailsScanner.ScanStudentDetails(studentToScan);

		Console.WriteLine(studentDetailsItem.ToString());
		Assert.IsNotNull(studentDetailsItem);
		Assert.IsInstanceOfType<StudentDetailsItem>(studentDetailsItem);
	}
}