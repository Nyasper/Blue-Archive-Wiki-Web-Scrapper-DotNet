namespace Testing;
using Main.Repository;

using Scanner.CharaDetails;
using Scanner.CharaList;
using Scanner.Configuration;
using Scanner.Model;

[TestClass]
public sealed class Scanners
{
	private StudentContext _context;
	private Repository _repository;
	private IHtmlHandler _htmlHandler;
	private CharaListScanner _charaListScanner;
	private ICharaDetailsScanner _charaDetailsScanner;

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
		_charaDetailsScanner = new Scanner.CharaDetails.CharaDetailsScanner(_htmlHandler);
	}

	[TestMethod]
	public async Task CharaList()
	{
		var studentsOnDb = await _repository.GetAll();
		var studentsOnPage = (await _charaListScanner.ScanCharaList()).ToArray();

		Assert.IsInstanceOfType<IEnumerable<CharaListStudent>>(studentsOnPage, "Should return CharaListItem Collection");
		Assert.IsGreaterThanOrEqualTo(studentsOnDb.Length, studentsOnPage.Length, "The number of characters in the page should be at least the number of students");
		Console.WriteLine($"students in DB: {studentsOnDb.Length}\nstudents in page: {studentsOnPage.Length}");
	}

	[TestMethod]
	public async Task CharaDetails()
	{
		const int maxScanned = 5;
		Random random = new();

		string[] excludeCharas = ["Shiroko_(Terror)"];
		var allStudents = (await _repository.GetAll()).Where(s => !excludeCharas.Contains(s.charaName)).ToArray();
		var randomStudents = allStudents.OrderBy(x => random.Next()).Take(maxScanned).ToArray();
		var scannedData = (await _charaDetailsScanner.ScanInfo(randomStudents)).ToArray();

		int charasInterestions = randomStudents.IntersectBy(scannedData.Select(scanned => scanned.charaName), s => s.charaName).Count();

		Assert.HasCount(maxScanned, randomStudents);
		Assert.HasCount(maxScanned, scannedData);
		Assert.AreEqual(maxScanned, charasInterestions, $"Should have {charasInterestions}/{maxScanned} intersections");
		Console.WriteLine($"characters in database/page intersections {charasInterestions}/{maxScanned}");
		foreach (var result in scannedData)
		{
			Console.WriteLine(result.ToString());
		}
	}
}