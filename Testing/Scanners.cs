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
		var pageCharaList = await _charaListScanner.ScanCharaList();

		Assert.IsInstanceOfType<HashSet<CharaListItem>>(pageCharaList, "Should return CharaListItem");
		Assert.IsTrue( pageCharaList.Count >= students.Count, "The number of characters in the page should be at least the number of students");
		Console.WriteLine($"students in DB: {students.Count}\nstudents in page: {pageCharaList.Count}");
	}

	[TestMethod]
	public async Task CharaDetails()
	{
		const int maxScanned = 5;
		Random random = new();

		string[] excludeCharas = ["Shiroko_(Terror)"];
		var allStudents = (await _repository.GetAll()).Where(s=> !excludeCharas.Contains(s.charaName)).ToHashSet();
		var randomStudents = allStudents.OrderBy(x => random.Next()).Take(maxScanned).ToHashSet();
		var scannedData = (await _charaDetails.ScanMany(randomStudents)).ToHashSet();

		int charasInterestions = randomStudents.IntersectBy(scannedData.Select(scanned=> scanned.charaName), s=>s.charaName).Count();

		Assert.AreEqual(maxScanned, randomStudents.Count);
		Assert.AreEqual(maxScanned, scannedData.Count);
		Assert.AreEqual(maxScanned, charasInterestions, $"Should have {charasInterestions}/{maxScanned} intersections");
		Console.WriteLine($"characters in database/page intersections {charasInterestions}/{maxScanned}");
	}
}