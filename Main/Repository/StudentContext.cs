using Main.Utils;

namespace Main.Repository;
using Scanner.Model;
using Microsoft.EntityFrameworkCore;

public class StudentContext : DbContext
{
	public DbSet<Student> students { get; set; }
	public string DbPath { get; }
	public StudentContext()
	{
		if (!Directory.Exists(Constants.DataPath)) Directory.CreateDirectory(Constants.DataPath);
		DbPath = Path.Join(Constants.DataPath, "BlueArchive.db");
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