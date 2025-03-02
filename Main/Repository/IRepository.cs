namespace Main.Repository;

public interface IRepository<T>
{
	Task SaveInDatabase(T entity);
	Task SaveInDatabase(IEnumerable<T> entities);
	Task SaveInDbFromJsonFile(string jsonFile);
	Task<T?> Get (string charaName);
	Task<T[]> GetAll();
}