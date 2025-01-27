namespace Main.Repository;
using Scanner.Model;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public class Repository(StudentContext context) : IRepository<Student>
{
	//CREATE
  public async Task SaveInDatabase(Student student)
  {
    Student? existStudent = await context.students.FindAsync(student.charaName);

    if (existStudent is null)
    {
      await context.students.AddAsync(student);
      Console.WriteLine($"{student.charaName} saved in Sqlite");
    }
  }
  public async Task SaveInDatabase(IEnumerable<Student> students)
  {
    await using var transaction = await context.Database.BeginTransactionAsync();

    try
    {
      foreach (var student in students)
      {
        await SaveInDatabase(student);
      }
      await context.SaveChangesAsync();
      await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
      await transaction.RollbackAsync();
      Console.WriteLine($"Error saving students: {ex.Message}");
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
  public async Task<Student[]> GetAll()
  {
    return await context.students.AsNoTracking().OrderBy(s => s.school).ThenBy(s => s.charaName).ToArrayAsync();
  }


  //DELETE
  // public async Task DeleteSqlite(Student student)
  // {
  //   using var db = new StudentContext();
  //   db.students.Remove(student);
  //   await db.SaveChangesAsync();
  // }

}
