using System.Text.Json;

using BlueArchiveWebScrapper.model;

using Microsoft.EntityFrameworkCore;
namespace BlueArchiveWebScrapper.db;

public static class SqliteController
{
  //CREATE
  public static async Task AddToDatabase(StudentContext db, Student student)
  {
    Student? existStudent = await db.students.FindAsync(student.charaName);

    if (existStudent is null)
    {
      await db.students.AddAsync(student);
      Console.WriteLine($"{student.charaName} saved in Sqlite");
    }
  }
  public static async Task SaveManyInDatabase(IEnumerable<Student> students)
  {
    using var db = new StudentContext();
    using var transaction = await db.Database.BeginTransactionAsync();

    try
    {
      foreach (var student in students)
      {
        await AddToDatabase(db, student);
      }
      await db.SaveChangesAsync();
      await transaction.CommitAsync();
    }
    catch (Exception ex)
    {
      await transaction.RollbackAsync();
      Console.WriteLine($"Error saving students: {ex.Message}");
    }
  }
  public static async Task ImportFromJsonFile(string jsonFile)
  {
    string archivo = await File.ReadAllTextAsync(jsonFile);
    try
    {

    }
    catch (FileNotFoundException)
    {
      System.Console.WriteLine($"{jsonFile} does not exist.");
      return;
    }
    var estudiantes = JsonSerializer.Deserialize<Student[]>(archivo);

    if (estudiantes != null)
    {
      await SaveManyInDatabase(estudiantes);
    }

    System.Console.WriteLine($"Database Updated with {jsonFile}");
  }

  //READ
  public static async Task<Student[]> GetAllStudents()
  {
    using var db = new StudentContext();
    return await db.students.AsNoTracking().MyCustomOrdenated().ToArrayAsync();
  }


  //DELETE
  // public static async Task DeleteSqlite(Student student)
  // {
  //   using var db = new StudentContext();
  //   db.students.Remove(student);
  //   await db.SaveChangesAsync();
  // }

  private static IQueryable<Student> MyCustomOrdenated(this IQueryable<Student> students)
  {
    return students.OrderBy(s => s.school).ThenBy(s => s.charaName);
  }

}

public class StudentContext : DbContext
{
  public DbSet<Student> students { get; set; }
  public string DbPath { get; }
  public StudentContext()
  {
    var DocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    var FinalRoute = Path.Join(DocumentsFolder, "BlueArchiveWS");
    if (!Directory.Exists(FinalRoute)) Directory.CreateDirectory(FinalRoute);
    DbPath = Path.Join(FinalRoute, "BlueArchive.db");
  }
  protected override void OnConfiguring(DbContextOptionsBuilder options)
   => options.UseSqlite($"Data Source={DbPath}");
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Student>()
      .HasKey(s => s.charaName);
    modelBuilder.Entity<Student>().HasIndex(s => s.charaName);
  }

}
