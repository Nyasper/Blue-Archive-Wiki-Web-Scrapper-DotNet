namespace Main.FileHandler.Verifier;

using Repository;
using Extensions;

using Scanner.CharaDetails;
using Scanner.CharaList;
using Scanner.Model;


public class DataVerifier(
	ICharaListScanner charaListScanner,
	ICharaDetailsScanner charaDetailsScanner) : IDataVerifier<Student>
{
	public async Task<Student[]> VerifyDataInDatabase(Student[] students)
	{
		StudentListItem[] studentsOnPage = await charaListScanner.ScanCharaList();

		// Search Differences
		StudentListItem[] differences =
			studentsOnPage.ExceptBy(students.Select(db => db.CharaName), p => p.CharaName).ToArray();

		StudentDetailsItem[] studentDetails = await charaDetailsScanner.ScanStudentDetails(differences);
		Student[] studentsScanned = differences + studentDetails;
		
		return studentsScanned;
	}
	
}