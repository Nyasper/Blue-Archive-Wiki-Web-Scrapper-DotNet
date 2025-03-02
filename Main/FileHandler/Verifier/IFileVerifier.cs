namespace Main.FileHandler.Verifier;
using Scanner.Model;

public interface IFileVerifier
{
	FileVerification VerifyLocalFiles(Student student);
	FileVerification[] VerifyLocalFiles(Student[] students);
}