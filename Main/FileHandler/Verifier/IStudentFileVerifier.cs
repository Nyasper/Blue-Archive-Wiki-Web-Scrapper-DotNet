namespace Main.FileHandler.Verifier;
using Scanner.Model;

public interface IStudentFileVerifier
{
	StudentFileVerification VerifyStudentLocalFiles(Student student);
	StudentFileVerification[] VerifyStudentLocalFiles(Student[] students);
}