namespace Main.FileHandler.Updater;

using FileGenerator;
using Downloader;
using Repository;
using Scanner.Model;
using Utils;
using Verifier;

public class Updater(
	IRepository<Student> studentRepository,
	IVerifier<Student> verifier,
	IFileGenerator<Student> fileGenerator,
	IDownloader downloader) : IUpdater
{
	public async Task Update()
	{
		Notifier.MessageInitiatingTask("Searching for updates");
		
		Student[] students = await studentRepository.GetAll();
		var missingStudentData = await verifier.VerifyStudentDataInDatabase(students);
		(bool hasDatabaseBeenUpdated, Student[] updatedStudents) = await UpdateDatabase(missingStudentData, students); 
		
		var missingStudentFiles = verifier.VerifyStudentLocalFiles(updatedStudents);
		bool hasLocalFilesBeenUpdated = await UpdateLocalFiles(missingStudentFiles, updatedStudents);

		if (hasDatabaseBeenUpdated || hasLocalFilesBeenUpdated)
		{
			Notifier.MessageTaskCompleted("Update complete");
		}
		else
		{
			Notifier.MessageTaskCompleted("Nothing to Update");
		}
	}

	private async Task<(bool Updated, Student[] Students)> UpdateDatabase(Student[] missingStudentData, Student[] allStudents)
	{
		if (missingStudentData.Length == 0) return (false, allStudents);
		
		Notifier.LogStudentsList("New Students to save In Database found", missingStudentData);
		
		bool shouldUpdate = YesNoQuestion("Update the database?");
		if (!shouldUpdate)
		{
			Notifier.MessageTaskCancelled("Database update cancelled by the user.");
			return (false, allStudents);
		}

		await studentRepository.SaveInDatabase(missingStudentData);

		var updatedStudents = allStudents
			.Concat(missingStudentData)
			.OrderBy(s => s.School)
			.ThenBy(s => s.CharaName)
			.ToArray();

		await fileGenerator.GenerateJsonData(updatedStudents);
		
		Notifier.MessageTaskCompleted("all data updated successfully");
		return (true, updatedStudents);
	}
	private async Task<bool> UpdateLocalFiles(StudentFileVerification[] missingStudentFiles, Student[] allStudents)
	{
		Student[] studentsWithoutFiles = allStudents.IntersectBy(missingStudentFiles.Select(f => f.CharaName), s => s.CharaName).ToArray();
		if (studentsWithoutFiles.Length == 0) return false;
		
		Notifier.LogStudentsList("New Students files to download", missingStudentFiles);
		
		bool shouldDownload = YesNoQuestion("Proceed to download the files?");
		if (!shouldDownload)
		{
			Notifier.MessageTaskCancelled("File download cancelled by the user.");
			return false;
		}
		
		await downloader.DownloadFiles(studentsWithoutFiles);
		await fileGenerator.GenerateHtmlDataPreview(allStudents);
		
		Notifier.MessageTaskCompleted($"all files downloaded successfully");
		return true;
	}

	private static bool YesNoQuestion(string questionMessage)
	{
		Console.WriteLine(Environment.NewLine + questionMessage + " (y/n)" + Environment.NewLine);
		char key = char.ToLower(Console.ReadKey(intercept: true).KeyChar);
		bool response = key == 'y';
		return response;
	}
}