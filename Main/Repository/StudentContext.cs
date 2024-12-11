namespace Main.Repository;
using Scanner.Model;
using Microsoft.EntityFrameworkCore;

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