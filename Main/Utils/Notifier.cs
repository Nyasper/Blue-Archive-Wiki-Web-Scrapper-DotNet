using Main.FileHandler.Verifier;

namespace Main.Utils;

using System;

using Scanner.Model;

public static class Notifier
{
	private static readonly string Nl = Environment.NewLine;
	public static void NewBlankMessage(string message)
	{
		Console.Clear();
		Console.WriteLine(message);
	}
	private static void NormalMessage(string message)
	{
		Console.WriteLine(Nl + message);
	}
	public static void MessageInitiatingTask(string message)
	{
		Console.WriteLine(Nl + message + "... 🚀");
	}
	public static void MessageTaskCompleted(string message)
	{
		Console.WriteLine(message + " ✔️" + Nl);
	}
	public static void MessageNothingToDo(string message)
	{
		Console.WriteLine(message + " ✅");
	}
	public static void MessageTaskCancelled(string message)
	{
		Console.WriteLine(Nl + message + " ❌");
	}
	public static void LogMissingFiles(StudentFileVerification studentFile)
	{
		string message = $"'{studentFile.CharaName}' doesn't have: ";
		var missingFiles = new List<string>(3);
		if (!studentFile.HasProfileImage) missingFiles.Add("'profile image'");
		if (!studentFile.HasFullImage) missingFiles.Add("'full image'");
		if (!studentFile.HasAudio) missingFiles.Add("'audio'");
		missingFiles[^1] = "and " + missingFiles[^1] + ".";

		Console.WriteLine(message + String.Join(", ", missingFiles));
	}
	public static void LogStudentsList(string message, IEnumerable<Student> studentsCollection)
	{
		NormalMessage(message);

		Student[] students = studentsCollection.ToArray();
		for (int i = 0; i < students.Length; i++)
		{
			Console.WriteLine($"{i + 1}: {students[i].CharaName} 💙");
		}
	}
	public static void LogStudentsList(string message, StudentListItem[] studentsCollection)
	{
		NormalMessage(message + " 💚");

		for (int i = 0; i < studentsCollection.Length; i++)
		{
			Console.WriteLine($"{i + 1}: {studentsCollection[i].CharaName} 💙");
		}
		Console.WriteLine(Nl);
	}
	public static void LogStudentsList(string message, StudentFileVerification[] studentsCollection)
	{
		NormalMessage(message);
		
		for (int i = 0; i < studentsCollection.Length; i++)
		{
			Console.WriteLine($"{i + 1}: {studentsCollection[i].CharaName} 💙");
		}
	}

}