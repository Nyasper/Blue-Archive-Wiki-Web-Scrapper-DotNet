namespace Main.FileHandler.Updater;

public interface IUpdater
{
	Task UpdateAll();
	Task UpdateDatabase();
	Task UpdateLocalFiles();
}