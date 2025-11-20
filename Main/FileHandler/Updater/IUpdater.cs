using Scanner.Model;

namespace Main.FileHandler.Updater;

public interface IUpdater
{
	Task UpdateAll();
	Task UpdateDatabaseOnly();
	Task UpdateLocalFilesOnly();
	Task<StudentListItem[]> SearchDatabaseUpdates();
}