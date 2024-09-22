using BlueArchiveWebScrapper.model;
using Microsoft.EntityFrameworkCore;
namespace BlueArchiveWebScrapper.db;

public static class SqliteController
{
  //CREATE
  public static async Task AddToDatabase(Student student)
  {
    using var db = new StudentContext();
    Student? existStudent = await GetStudentById(student.charaName);

    if (existStudent is null) {
      db.students.Add(student);
      Console.WriteLine($"{student.charaName} saved in Sqlite");
    }
  }

  public static async Task SaveManyInDatabase(IEnumerable<Student> students)
  {
    using var db = new StudentContext();
    using var transaction = await db.Database.BeginTransactionAsync();

    try
    {
      foreach (var student in students){
        await AddToDatabase(student);
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
  //READ
  public static async Task<Student?> GetStudentById(string CharaName)
  {
    using var db = new StudentContext();
    return await db.students.FindAsync(CharaName);
  }
  public static async Task<Student[]> GetAllStudents()
  {
    using var db = new StudentContext();
    return await db.students.AsNoTracking().MyCustomOrdenated().ToArrayAsync();
  }
   public static async Task<Student[]> GetAllStudentsWithoutFiles()
  {
    using var db = new StudentContext();
    return await db.students.Where(s => !s.files).AsNoTracking().MyCustomOrdenated().ToArrayAsync();
  }
  // public static async Task<Student?> GetDbInfo(this Student student) => await GetStudentById(student.charaName);
  //UPDATE
  public static async Task UpdateFilesDownloaded(IEnumerable<Student> students)
  {
    using var db = new StudentContext();
    
    foreach (var student in students)
    {
      student.files = true;
      db.Entry(student).Property(s=>s.files).IsModified = true; // Marcar la entidad como modificada solo en su propiedad files.
    }
    await db.SaveChangesAsync();
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

public class StudentContext : DbContext {
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
    modelBuilder.Entity<Student>().HasIndex(s=>s.charaName);
  }

}
