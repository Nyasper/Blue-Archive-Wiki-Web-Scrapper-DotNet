namespace Main.FileHandler.Verifier;
using Scanner.Model;

public interface IVerifier<T>
{
	Task<T[]> VerifyStudentDataInDatabase(T[] entity);
	StudentFileVerification[] VerifyStudentLocalFiles(T[] entity);

}