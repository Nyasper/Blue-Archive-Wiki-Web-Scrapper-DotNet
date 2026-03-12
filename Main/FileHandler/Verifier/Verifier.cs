using Main.Repository;

using Scanner.Model;

namespace Main.FileHandler.Verifier;

public class Verifier(IDataVerifier<Student> dataVerifier, IStudentFileVerifier studentFileVerifier) : IVerifier<Student>
{
	public async Task<Student[]> VerifyStudentDataInDatabase(Student[] students)
	{
		try
		{
			Student[] verifiedDatabase = await dataVerifier.VerifyDataInDatabase(students);
			
			return verifiedDatabase;
		}
		catch (Exception e)
		{
			Console.WriteLine("Error trying to verify Database: " + e.Message);
			throw;
		}
	}

	public StudentFileVerification[] VerifyStudentLocalFiles(Student[] students)
	{
		try
		{
			StudentFileVerification[] localFilesVerified = studentFileVerifier.VerifyStudentLocalFiles(students);
			return localFilesVerified;
		}
		catch (Exception e)
		{
			Console.WriteLine("Error trying to verify local files: " + e.Message);
			throw;
		}
	}
}