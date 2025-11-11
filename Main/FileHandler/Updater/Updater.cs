using Scanner.CharaList;

namespace Main.FileHandler.Updater;
using Creator;

using Downloader;

using Repository;

using Scanner.CharaDetails;
using Scanner.Model;

using Utils;

using Verifier;

public class Updater(
	IRepository<Student> repository,
	IFileVerifier fileVerifier,
	ICreator creator,
	IDownloader downloader,
	ICharaDetails charaDetailsScanner,
	ICharaListScanner charaListScanner) : IUpdater
{
	private readonly IRepository<Student> _studentRepository = repository;

	public async Task UpdateAll()
	{
		Queue<Func<Task>> toUpdate = new([UpdateDatabaseOnly, UpdateLocalFilesOnly]);
		while (toUpdate.Count > 0)
		{
			Func<Task> taskToRun = toUpdate.Dequeue();
			await taskToRun();
		}
	}

	public async Task UpdateDatabaseOnly()
	{
		Notifier.MessageInitiatingTask("Searching for Updates");
		CharaListItem[] availableUpdates = await SearchDatabaseUpdates();
		if (availableUpdates.Length == 0)
		{
			Notifier.MessageNothingToDo("Database OK");
			return;
		}

		Notifier.LogStudentsList($"{availableUpdates.Length} New Students to Save:", availableUpdates);
		// bool continue = Menu.YesNoQuestion("Update the database");

		Notifier.MessageInitiatingTask("Scanning Students Data");
		IEnumerable<Student> scannedData = await charaDetailsScanner.ScanInfo(availableUpdates);
		Notifier.MessageTaskCompleted("All data scanned successfully");

		Notifier.MessageInitiatingTask("Saving data in Database");
		await _studentRepository.SaveInDatabase(scannedData);
		Notifier.MessageTaskCompleted("Database updated successfully");

		await creator.GenerateJsonData();
	}
	public async Task UpdateLocalFilesOnly()
	{
		Notifier.MessageInitiatingTask("Verifying students files");
		Student[] students = await _studentRepository.GetAll();
		FileVerification[] studentsWithoutFiles = fileVerifier.VerifyLocalFiles(students);

		students = students.IntersectBy(studentsWithoutFiles.Select(f=>f.CharaName), s=>s.charaName).ToArray();
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

	public async Task<CharaListItem[]> SearchDatabaseUpdates()
	{
		var charactersInPage = await charaListScanner.ScanCharaList();
		var charactersSqlite = await repository.GetAll();

		// Search Differences
		return charactersInPage.ExceptBy(charactersSqlite.Select(db => db.charaName), p => p.Name).ToArray();
	}
}