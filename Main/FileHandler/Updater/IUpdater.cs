using System.Threading.Tasks;

using Scanner.Model;

namespace Main.FileHandler.Updater;

public interface IUpdater
{
	Task Update();
	Task UpdateDatabase();
	Task UpdateLocalFiles();
}