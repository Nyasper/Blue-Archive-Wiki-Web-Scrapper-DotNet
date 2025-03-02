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
	public static void LogMissingFiles(FileVerification file)
	{
		string message = $"'{file.CharaName}' doesn't have: ";
		List<string> missingFiles = [];
		if (!file.HasProfileImage) missingFiles.Add("profile image");
		if (!file.HasFullImage) missingFiles.Add("full image");
		if (!file.HasAudio) missingFiles.Add("audio");
		Console.WriteLine(message + String.Join(", ", missingFiles));
	}
	public static void LogStudentsList(string message, IEnumerable<Student> studentsCollection)
	{
		NormalMessage(message);

		Student[] students = studentsCollection.ToArray();
		for (int i = 0; i < students.Length; i++)
		{
			Console.WriteLine($"{i + 1}: {students[i].charaName} 💙");
		}
	}
	public static void LogStudentsList(string message, IEnumerable<CharaListItem> studentsCollection)
	{
		NormalMessage(message + " 💚");

		CharaListItem[] students = studentsCollection.ToArray();
		for (int i = 0; i < students.Length; i++)
		{
			Console.WriteLine($"{i + 1}: {students[i].Name} 💙");
		}
		Console.WriteLine(Nl);
	}

}