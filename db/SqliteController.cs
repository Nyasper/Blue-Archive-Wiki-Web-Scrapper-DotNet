using Microsoft.EntityFrameworkCore;
namespace BlueArchiveWebScrapper.db;

public static class SqliteController
{
  //CREATE
  public static async Task SaveSqlite(this Student student)
  {
    using var db = new StudentContext();
    var ExistStudent = await db.FindAsync<Student>(student.charaName);
    if (ExistStudent == null) {
      await db.Database.MigrateAsync();
      db.students.Add(student);
      await db.SaveChangesAsync();
      Console.WriteLine($"{student.charaName} saved in Sqlite");
    }
  }
  //READ
  public static async Task<List<Student>> GetAllStudentsSqlite()
  {
    using var db = new StudentContext();
 
    return await db.students.AsNoTracking().MyCustomOrdenated().ToListAsync();
  }
   public static async Task<List<Student>> GetAllStudentsWithoutFiles()
  {
    using var db = new StudentContext();
    return await db.students.Where(s => !s.files).AsNoTracking().MyCustomOrdenated().ToListAsync();
  }
  public static async Task<Student?> GetDbInfo(this Student student)
  {
    using var db = new StudentContext();
    var StudentFound = await db.FindAsync<Student>(student.charaName);
    return StudentFound;
  }


  //UPDATE
  public static async Task FilesDownloaded(string CharaNameParam)
  {
    using var db = new StudentContext();
    Student? student = await db.students.FirstOrDefaultAsync(s=>s.charaName == CharaNameParam);
    if (student != null)
    {
      student.files = true;
      await db.SaveChangesAsync();
    }
  }

  //DELETE
  public static async Task DeleteSqlite(this Student student)
  {
    using var db = new StudentContext();
    db.students.Remove(student);
    await db.SaveChangesAsync();
  }

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
