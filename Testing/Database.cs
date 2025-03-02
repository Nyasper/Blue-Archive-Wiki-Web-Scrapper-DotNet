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
		Assert.IsInstanceOfType<Student[]>(students, "Should return Student[] type");
		Assert.IsTrue(students.All(student => student is Student), "All elements should be of type Student");
	}
}