using Scanner.Model;

namespace Main.FileHandler.Updater;

public interface IUpdater
{
	Task Update();
	Task UpdateDatabase();
	Task UpdateLocalFiles();
	Task<Student[]> SearchDatabaseUpdates();
}