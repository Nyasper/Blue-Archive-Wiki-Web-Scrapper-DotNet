using Scanner.Model;

namespace Testing;
using Main.Repository;


[TestClass]
public class Database
{
	private StudentContext _context;
	private Repository _repository;

	[TestInitialize]
	public void Setup()
	{
		_context = new StudentContext();
		_repository = new Repository(_context);
	}

	[TestMethod]
	public async Task GetAll()
	{
		var students = await _repository.GetAll();
		Assert.IsInstanceOfType<HashSet<Student>>(students, "Should return HashSet<Student> type");
	}
}