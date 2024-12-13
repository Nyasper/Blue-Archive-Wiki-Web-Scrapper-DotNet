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
		Console.WriteLine($"cant students: {students.Length}");
		Assert.IsInstanceOfType<Student[]>(students, "Should return Student[] type");
	}
}

