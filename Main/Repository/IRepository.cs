namespace Main.Repository;

public interface IRepository<S>
{
	Task SaveInDatabase(S entity);
	Task SaveInDatabase(IEnumerable<S> entities);
	Task SaveInDbFromJsonFile(string jsonFile);
	Task<HashSet<S>> GetAll();
}