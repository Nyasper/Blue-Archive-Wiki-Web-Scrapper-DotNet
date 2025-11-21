using Main.Utils;

namespace Main.Repository;
using Microsoft.EntityFrameworkCore;

using Scanner.Model;

public class StudentContext : DbContext
{
	public DbSet<Student> Students { get; set; }
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
			.HasKey(s => s.CharaName);
		modelBuilder.Entity<Student>().HasIndex(s => s.CharaName);

		// change table names to camelCase
		foreach (var entity in modelBuilder.Model.GetEntityTypes())
		{
			var tableName = entity.GetTableName();
			if (!string.IsNullOrEmpty(tableName))
			{
				entity.SetTableName(ToCamelCase(tableName));
			}

			// change column names to camelCase
			foreach (var property in entity.GetProperties())
			{
				var columnName = property.GetColumnName();
				property.SetColumnName(ToCamelCase(columnName));
			}
		}
	}

	private static string ToCamelCase(string name)
	{
		if (string.IsNullOrEmpty(name) || name.Length < 2)
			return name;

		// Si ya empieza con minúscula lo dejamos
		if (char.IsLower(name[0]))
			return name;

		return char.ToLowerInvariant(name[0]) + name.Substring(1);
	}
}