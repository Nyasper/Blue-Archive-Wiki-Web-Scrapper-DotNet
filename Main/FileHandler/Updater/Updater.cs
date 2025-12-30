namespace Main.FileHandler.Updater;

using Scanner.CharaList;
using FileGenerator;
using Downloader;
using Repository;
using Scanner.CharaDetails;
using Scanner.Model;
using Utils;
using Verifier;
using Extensions;
using System.Diagnostics;

public class Updater(
	IRepository<Student> studentRepository,
	IFileVerifier fileVerifier,
	IFileGenerator fileGenerator,
	IDownloader downloader,
	ICharaDetailsScanner charaDetailsScanner,
	ICharaListScanner charaListScanner) : IUpdater
{
	public async Task Update()
	{
		int selectedOption = Menu.LogMenu(["Update All", "Update only Database", "Update only Local Files", "Exit"]);
		switch (selectedOption)
		{
			case 1:
				await UpdateDatabase();
				await UpdateLocalFiles();
				Console.WriteLine("Updating All...");
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
		}
	}

	public async Task UpdateDatabase()
	{
		Notifier.MessageInitiatingTask("Scanning for Updates");

		Student[] availableUpdates = await SearchDatabaseUpdates();
		if (availableUpdates.Length == 0)
		{
			Notifier.MessageNothingToDo("Database is already updated, nothing to do.");
			return;
		}

		Notifier.LogStudentsList($"{availableUpdates.Length} New Students to Save:", availableUpdates);


		bool shouldUpdate = Menu.YesNoQuestion("Update the database?");
		if (!shouldUpdate)
		{
			Notifier.MessageTaskCancelled("Database update cancelled by user.");
			Environment.Exit(0);
		}

		await studentRepository.SaveInDatabase(availableUpdates);
		await fileGenerator.GenerateJsonData();
	}
	public async Task UpdateLocalFiles()
	{
		Notifier.MessageInitiatingTask("Scanning Students files");
		Student[] students = await studentRepository.GetAll();
		FileVerification[] studentsWithoutFiles = fileVerifier.VerifyLocalFiles(students);

		students = students.IntersectBy(studentsWithoutFiles.Select(f => f.CharaName), s => s.CharaName).ToArray();
		if (studentsWithoutFiles.Length == 0)
		{
			Notifier.MessageNothingToDo("All files are already downloaded, nothing to do.");
			return;
		}

		bool shouldDownload = Menu.YesNoQuestion("Proceed to download the files?");
		if (!shouldDownload)
		{
			Notifier.MessageTaskCancelled("File download cancelled by user.");
			Environment.Exit(0);
		}

		Notifier.MessageInitiatingTask("Downloading missing files");
		await downloader.DownloadFiles(students);
		Notifier.MessageTaskCompleted("All files updated successfully");

		// await fileGenerator.GenerateHtmlImagePreview();
	}

	public async Task<Student[]> SearchDatabaseUpdates()
	{
		Student[] studentsOnDb = await studentRepository.GetAll();
		StudentListItem[] studentsOnPage = await charaListScanner.ScanCharaList();

		// Search Differences
		StudentListItem[] differences = studentsOnPage.ExceptBy(studentsOnDb.Select(db => db.CharaName), p => p.CharaName).ToArray();

		StudentDetailsItem[] studenDetails = await charaDetailsScanner.ScanStudentDetails(differences);
		Student[] studentsScanned = differences + studenDetails;

		return studentsScanned;
	}
}