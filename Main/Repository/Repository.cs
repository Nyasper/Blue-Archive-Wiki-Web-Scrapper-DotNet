using Main.Utils;

namespace Main.Repository;
using System.Text.Json;

using Microsoft.EntityFrameworkCore;

using Scanner.Model;

public class Repository(StudentContext context) : IRepository<Student>
{
	//CREATE
	public async Task SaveInDatabase(Student student)
	{
		Student? existStudent = await context.Students.FindAsync(student.CharaName);

		if (existStudent is null)
		{
			await context.Students.AddAsync(student);
			Console.WriteLine($"{student.CharaName} saved in Sqlite");
		}
	}
	public async Task SaveInDatabase(IEnumerable<Student> students)
	{
		Notifier.MessageInitiatingTask("Saving data in Database");
		await using var transaction = await context.Database.BeginTransactionAsync();

		try
		{
			foreach (var student in students)
			{
				await SaveInDatabase(student);
			}
			await context.SaveChangesAsync();
			await transaction.CommitAsync();

			Notifier.MessageTaskCompleted("Database updated successfully");
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			Console.WriteLine($"Error saving Students: {ex.Message}");
		}
	}
	public async Task SaveInDbFromJsonFile(string jsonFilePath)
	{
		string jsonFile = await File.ReadAllTextAsync(jsonFilePath);
		try
		{

		}
		catch (FileNotFoundException)
		{
			Console.WriteLine($"{jsonFile} does not exist.");
			return;
		}
		var students = JsonSerializer.Deserialize<Student[]>(jsonFile);

		if (students != null)
		{
			await SaveInDatabase(students);
		}

		Console.WriteLine($"Database Updated with {jsonFile}");
	}

	//READ
	public async Task<Student?> Get(string charaName)
	{
		return await context.Students.FindAsync(charaName);
	}
	public async Task<Student[]> GetAll()
	{
		return await context.Students.AsNoTracking().OrderBy(s => s.School).ThenBy(s => s.CharaName).ToArrayAsync();
	}


	//DELETE
	// public async Task DeleteSqlite(Student student)
	// {
	//   using var db = new StudentContext();
	//   db.Students.Remove(student);
	//   await db.SaveChangesAsync();
	// }

}