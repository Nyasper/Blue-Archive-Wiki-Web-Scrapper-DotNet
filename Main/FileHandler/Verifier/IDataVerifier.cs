namespace Main.FileHandler.Verifier;
using Scanner.Model;


public interface IDataVerifier<T>
{
	Task<T[]> VerifyDataInDatabase(T[] entity);
}