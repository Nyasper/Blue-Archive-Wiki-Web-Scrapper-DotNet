namespace Main.FileHandler.Updater;
using Scanner.Model;
using Scanner.CharaDetails;
using Creator;
using Downloader;
using Verifier;
using Repository;
using Utils;

public class Updater(
	IRepository<Student> repository,
	IVerifier<Student> verifier,
	ICreator creator,
	IDownloader downloader,
	ICharaDetails<Student> charaDetailsScanner) : IUpdater
{
	private readonly IRepository<Student> _studentRepository = repository;

	public async Task UpdateAll()
	{
		Queue<Func<Task>> toUpdate = new([UpdateDatabase, UpdateLocalFiles]);
		while (toUpdate.Count > 0)
		{
			Func<Task> taskToRun = toUpdate.Dequeue();
			await taskToRun();
		}
	}

	public async Task UpdateDatabase()
	{
		Notifier.MessageInitiatingTask("Searching for Updates");
		CharaListItem[] availableUpdates = (await verifier.SearchDatabaseUpdates()).ToArray();
		if (availableUpdates.Length == 0)
		{
			Notifier.MessageNothingToDo("Database OK");
			return;
		}

		Notifier.LogStudentsList($"{availableUpdates.Length} New Students to Save:", availableUpdates);
		// bool continue = Menu.YesNoQuestion("Update the database");

		Notifier.MessageInitiatingTask("Scanning Students Data");
		IEnumerable<Student> scannedData = await charaDetailsScanner.ScanMany(availableUpdates);
		Notifier.MessageTaskCompleted("All data scanned successfully");

		Notifier.MessageInitiatingTask("Saving data in Database");
		await _studentRepository.SaveInDatabase(scannedData);
		Notifier.MessageTaskCompleted("Database updated successfully");
		await creator.GenerateJsonData();
	}
	public async Task UpdateLocalFiles()
	{
		Notifier.MessageInitiatingTask("Verifying students files");
		Student[] studentsWithoutFiles = (await verifier.VerifyAllStudentsFiles()).ToArray();

		if (studentsWithoutFiles.Length == 0)
		{
			Notifier.MessageNothingToDo("All files OK");
			return;
		}
		// bool continue = Menu.YesNoQuestion("Proceed to download the files");
		Notifier.MessageInitiatingTask("Downloading missing files");
		await downloader.DownloadFiles(studentsWithoutFiles);
		Notifier.MessageTaskCompleted("All files updated successfully");

		await creator.GenerateHtmlImagePreview();
	}
}


