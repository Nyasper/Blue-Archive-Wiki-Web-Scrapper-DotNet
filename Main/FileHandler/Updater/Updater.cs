namespace Main.FileHandler.Updater;

using Scanner.CharaList;
using Creator;
using Downloader;
using Repository;
using Scanner.CharaDetails;
using Scanner.Model;
using Utils;
using Verifier;
using Extensions;

public class Updater(
	IRepository<Student> repository,
	IFileVerifier fileVerifier,
	ICreator creator,
	IDownloader downloader,
	ICharaDetailsScanner charaDetailsScanner,
	ICharaListScanner charaListScanner) : IUpdater
{
	private readonly IRepository<Student> _studentRepository = repository;

	public async Task UpdateAll()
	{
		Func<Task>[] toUpdate = [UpdateDatabase, UpdateLocalFiles];
		foreach (var task in toUpdate) await task();
	}

	public async Task UpdateDatabase()
	{
		Notifier.MessageInitiatingTask("Searching for Updates");

		Student[] availableUpdates = await SearchDatabaseUpdates();
		Notifier.MessageTaskCompleted("All data scanned successfully");
		Notifier.LogStudentsList($"{availableUpdates.Length} New Students to Save:", availableUpdates);

		if (availableUpdates.Length == 0)
		{
			Notifier.MessageNothingToDo("Database OK");
			return;
		}

		// bool continue = Menu.YesNoQuestion("Update the database");

		await _studentRepository.SaveInDatabase(availableUpdates);
		await creator.GenerateJsonData();
	}
	public async Task UpdateLocalFiles()
	{
		Notifier.MessageInitiatingTask("Verifying Students files");
		Student[] students = await _studentRepository.GetAll();
		FileVerification[] studentsWithoutFiles = fileVerifier.VerifyLocalFiles(students);

		students = students.IntersectBy(studentsWithoutFiles.Select(f=>f.CharaName), s=>s.CharaName).ToArray();
		if (studentsWithoutFiles.Length == 0)
		{
			Notifier.MessageNothingToDo("All files OK");
			return;
		}
		// bool continue = Menu.YesNoQuestion("Proceed to download the files");
		Notifier.MessageInitiatingTask("Downloading missing files");
		await downloader.DownloadFiles(students);
		Notifier.MessageTaskCompleted("All files updated successfully");

		await creator.GenerateHtmlImagePreview();
	}

	public async Task<Student[]> SearchDatabaseUpdates()
	{
		Student[] studentsOnDb = await repository.GetAll();
		StudentListItem[] studentsOnPage = await charaListScanner.ScanCharaList();

		// Search Differences
		StudentListItem[] differences = studentsOnPage.ExceptBy(studentsOnDb.Select(db => db.CharaName), p => p.CharaName).ToArray();

		StudentDetailsItem[] studenDetails = await charaDetailsScanner.ScanStudentDetails(differences);
		Student[] studentsScanned = differences + studenDetails;

		return studentsScanned;
	}
}