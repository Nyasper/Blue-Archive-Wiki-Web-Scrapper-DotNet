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
		Queue<Func<Task>> toUpdate = [];
		toUpdate.Enqueue(UpdateDatabase);
		toUpdate.Enqueue(UpdateLocalFiles);

		while (toUpdate.Count > 0)
		{
			Func<Task> taskToRun = toUpdate.Dequeue();
			await taskToRun();
		}
	}

	public async Task UpdateDatabase()
	{
		Notifier.MessageInitiatingTask("Searching for Updates");
		IEnumerable<CharaListItem> availableUpdates = await verifier.SearchDatabaseUpdates();
		int updatesCount = availableUpdates.Count();
		if (updatesCount == 0)
		{
			Notifier.MessageNothingToDo("Database OK");
			return;
		}

		Notifier.LogStudentsList($"{updatesCount} New Students to Save:", availableUpdates);
		// bool continuee = Menu.YesNoQuestion("Update the database");

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
		IEnumerable<Student> studentsWithoutFiles = await verifier.VerifyAllStudentsFiles();
		int studentsWithoutFilesCount = studentsWithoutFiles.Count();

		if (studentsWithoutFilesCount == 0)
		{
			Notifier.MessageNothingToDo("All files OK");
			return;
		}
		// bool continuee = Menu.YesNoQuestion("Proceed to download the files");
		Notifier.MessageInitiatingTask("Downloading missing files");
		await downloader.DownloadFiles(studentsWithoutFiles);
		Notifier.MessageTaskCompleted("All files updated successfully");

		await creator.GenerateJsonData();
	}
}


