namespace Main.FileHandler.Verifier;
using Scanner.Model;

public interface IVerifier<S>
{
	Task<IEnumerable<Student>> VerifyAllStudentsFiles();
	Task<IEnumerable<CharaListItem>> SearchDatabaseUpdates();
	FileVerification VerifyStudentFilesExists(S student);
}