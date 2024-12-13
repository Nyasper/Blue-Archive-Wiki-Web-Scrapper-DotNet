namespace Testing;
using Main.Repository;
using Scanner.CharaList;
using Scanner.Configuration;
using Scanner.Model;
using Scanner.CharaDetails;

[TestClass]
public sealed class Scanners
{
	private StudentContext _context;
	private Repository _repository;
	private IHtmlHandler _htmlHandler;
	private CharaListScanner _charaListScanner;
	private ICharaDetails<Student> _charaDetails;

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
		_charaDetails = new CharaDetails(_htmlHandler);
	}

	[TestMethod]
	public async Task CharaList()
	{
		var students = await _repository.GetAll();
		var pageCharaList = (await _charaListScanner.ScanCharaList()).ToArray();

		Assert.IsInstanceOfType<IEnumerable<CharaListItem>>(pageCharaList, "Should return CharaListItem");
		Assert.IsTrue( pageCharaList.Length >= students.Length, "The number of characters in the page should be at least the number of students");
		Console.WriteLine($"students in DB: {students.Length}\nstudents in page: {pageCharaList.Length}");
	}

	[TestMethod]
	public async Task CharaDetails()
	{
		const int maxScanned = 5;
		Random random = new();

		string[] excludeCharas = ["Shiroko_(Terror)"];
		var allStudents = (await _repository.GetAll()).Where(s=> !excludeCharas.Contains(s.charaName)).ToArray();
		var randomStudents = allStudents.OrderBy(x => random.Next()).Take(maxScanned).ToArray();
		var scannedData = (await _charaDetails.ScanMany(randomStudents)).ToArray();

		int charasInterestions = randomStudents.IntersectBy(scannedData.Select(scanned=> scanned.charaName), s=>s.charaName).Count();

		Assert.AreEqual(maxScanned, randomStudents.Length);
		Assert.AreEqual(maxScanned, scannedData.Length);
		Assert.AreEqual(maxScanned, charasInterestions, $"Should have {charasInterestions}/{maxScanned} intersections");
		Console.WriteLine($"characters in database/page intersections {charasInterestions}/{maxScanned}");
	}
}