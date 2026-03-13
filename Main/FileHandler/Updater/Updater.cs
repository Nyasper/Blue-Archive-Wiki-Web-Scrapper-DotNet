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
		/*int selectedOption = Menu.LogMenu(["Update All", "Update only Database", "Update only Local Files", "Exit"]);
		switch (selectedOption)
		{
			case 1:
				await UpdateDatabase();
				await UpdateLocalFiles();
				Console.WriteLine("All files Updated Successfully...");
				break;
			case 2:
				await UpdateDatabase();
				Console.WriteLine("Database Updated.");
				break;
			case 3:
				await UpdateLocalFiles();
				Console.WriteLine("Local Files Updated.");
				break;
			default:
				Notifier.MessageTaskCancelled("Invalid Option selected, cancelling update.");
				return;
		}*/
		Notifier.MessageInitiatingTask("Searching for updates");
		
		Student[] students = await studentRepository.GetAll();
		var missingStudentData = await verifier.VerifyStudentDataInDatabase(students);
		await UpdateDatabase(missingStudentData, students); 
		
		students = await studentRepository.GetAll();
		var missingStudentFiles = verifier.VerifyStudentLocalFiles(students);
		await UpdateLocalFiles(missingStudentFiles, students);

		Notifier.MessageTaskCompleted("Update complete");
	}

	private async Task UpdateDatabase(Student[] missingStudentData, Student[] allStudents)
	{
		if (missingStudentData.Length == 0) return;
		
		Notifier.LogStudentsList("New Students to save In Database found", missingStudentData);
		
		bool shouldUpdate = Menu.YesNoQuestion("Update the database?");
		if (!shouldUpdate)
		{
			Notifier.MessageTaskCancelled("Database update cancelled by the user.");
			return;
		}

		await studentRepository.SaveInDatabase(missingStudentData);
		await fileGenerator.GenerateJsonData(allStudents);
		
		Notifier.MessageTaskCompleted("all data updated successfully");
	}
	private async Task UpdateLocalFiles(StudentFileVerification[] missingStudentFiles, Student[] allStudents)
	{
		Student[] studentsWithoutFiles = allStudents.IntersectBy(missingStudentFiles.Select(f => f.CharaName), s => s.CharaName).ToArray();
		if (studentsWithoutFiles.Length == 0) return;
		
		Notifier.LogStudentsList("New Students files to download", missingStudentFiles);
		
		bool shouldDownload = Menu.YesNoQuestion("Proceed to download the files?");
		if (!shouldDownload)
		{
			Notifier.MessageTaskCancelled("File download cancelled by the user.");
			return;
		}
		
		await downloader.DownloadFiles(studentsWithoutFiles);
		await fileGenerator.GenerateHtmlDataPreview(allStudents);
		
		Notifier.MessageTaskCompleted($"all files downloaded successfully");
	}
}