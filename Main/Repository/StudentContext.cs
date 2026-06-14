using System.IO;
using Microsoft.Data.Sqlite;
using Main.Utils;
using Dapper;

namespace Main.Repository;

public class StudentContext
{
	public string DbPath { get; }
	private readonly string _connectionString;

	public StudentContext()
	{
		if (!Directory.Exists(Constants.DataPath)) Directory.CreateDirectory(Constants.DataPath);
		DbPath = Path.Join(Constants.DataPath, "BlueArchive.db");
		_connectionString = $"Data Source={DbPath}";
		InitializeDatabase();
	}

	public SqliteConnection CreateConnection()
	{
		return new SqliteConnection(_connectionString);
	}

	private void InitializeDatabase()
	{
		using var connection = CreateConnection();
		connection.Open();

		const string createTableSql = @"
			CREATE TABLE IF NOT EXISTS students (
				charaName TEXT PRIMARY KEY,
				name TEXT NOT NULL,
				lastName TEXT NOT NULL,
				school TEXT NOT NULL,
				age INTEGER,
				height INTEGER,
				birthday TEXT,
				hobbies TEXT,
				designer TEXT,
				illustrator TEXT,
				voice TEXT NOT NULL,
				releaseDate TEXT NOT NULL,
				skinSet TEXT NOT NULL,
				pageUrl TEXT NOT NULL,
				imageProfileUrl TEXT NOT NULL,
				imageFullUrl TEXT NOT NULL,
				smallImageUrl TEXT NOT NULL,
				audioUrl TEXT NOT NULL,
				createdAt TEXT NOT NULL
			);

			CREATE INDEX IF NOT EXISTS IX_students_charaName ON students (charaName);
		";

		connection.Execute(createTableSql);
	}
}