using Main.Utils;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using Scanner.Model;
using Retry = Scanner.Utils.Retry;

namespace Main.Repository;

public class Repository(StudentContext context) : IRepository<Student>
{
	//CREATE
	public async Task SaveInDatabase(Student student)
	{
		try
		{
			using var connection = context.CreateConnection();
			const string sql = @"
				INSERT OR IGNORE INTO students (
					charaName, name, lastName, school, age, height, birthday, hobbies, designer, illustrator,
					voice, releaseDate, skinSet, pageUrl, imageProfileUrl, imageFullUrl, smallImageUrl, audioUrl, createdAt
				) VALUES (
					@CharaName, @Name, @LastName, @School, @Age, @Height, @Birthday, @Hobbies, @Designer, @Illustrator,
					@Voice, @ReleaseDate, @SkinSet, @PageUrl, @ImageProfileUrl, @ImageFullUrl, @SmallImageUrl, @AudioUrl, @CreatedAt
				);";
			await connection.ExecuteAsync(sql, student);
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on saving Student: '{student.CharaName}'");
			throw;
		}
	}

	public async Task SaveInDatabase(IEnumerable<Student> students)
	{
		Notifier.MessageInitiatingTask("Saving data in Database");
		await Retry.WithRetryAsync(async () =>
		{
			using var connection = context.CreateConnection();
			await connection.OpenAsync();
			using var transaction = await connection.BeginTransactionAsync();

			try
			{
				const string sql = @"
					INSERT OR IGNORE INTO students (
						charaName, name, lastName, school, age, height, birthday, hobbies, designer, illustrator,
						voice, releaseDate, skinSet, pageUrl, imageProfileUrl, imageFullUrl, smallImageUrl, audioUrl, createdAt
					) VALUES (
						@CharaName, @Name, @LastName, @School, @Age, @Height, @Birthday, @Hobbies, @Designer, @Illustrator,
						@Voice, @ReleaseDate, @SkinSet, @PageUrl, @ImageProfileUrl, @ImageFullUrl, @SmallImageUrl, @AudioUrl, @CreatedAt
					);";

				await connection.ExecuteAsync(sql, students, transaction);
				await transaction.CommitAsync();

				Notifier.MessageTaskCompleted("Database updated successfully");
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				Console.WriteLine($"Error saving Students: {ex.Message}");
				throw;
			}
		}, "Saving Students in Database", TimeSpan.FromSeconds(5));
	}

	public async Task SaveInDbFromJsonFile(string jsonFilePath)
	{
		try
		{
			string jsonFile = await File.ReadAllTextAsync(jsonFilePath);
			var students = JsonSerializer.Deserialize<Student[]>(jsonFile, Constants.JsonOptions);

			if (students != null)
			{
				await SaveInDatabase(students);
			}

			Console.WriteLine($"Database Updated with {jsonFilePath}");
		}
		catch (FileNotFoundException)
		{
			Console.WriteLine($"{jsonFilePath} does not exist.");
		}
	}

	//READ
	public async Task<Student?> Get(string charaName)
	{
		using var connection = context.CreateConnection();
		const string sql = "SELECT * FROM students WHERE charaName = @CharaName;";
		return await connection.QueryFirstOrDefaultAsync<Student>(sql, new { CharaName = charaName });
	}

	public async Task<Student[]> GetAll()
	{
		using var connection = context.CreateConnection();
		const string sql = "SELECT * FROM students ORDER BY school, charaName;";
		var result = await connection.QueryAsync<Student>(sql);
		return result.ToArray();
	}
}