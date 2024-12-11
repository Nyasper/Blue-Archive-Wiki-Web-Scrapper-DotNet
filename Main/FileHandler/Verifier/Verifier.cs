namespace Main.FileHandler.Verifier;
using Repository;
using Scanner.Model;
using Utils;
using Scanner.CharaList;
using System.Collections.Concurrent;

public class Verifier(IRepository<Student> repository, ICharaListScanner charaListScanner) : IVerifier<Student>
{
	public async Task<IEnumerable<Student>> VerifyAllStudentsFiles()
	{
		var allStudents = await repository.GetAll();
		ConcurrentBag<FileVerification> studentsWithoutAllFiles = []; //ConcurrentBag<> = List<> but for Parallelism.

		Parallel.ForEach(allStudents, student =>
		{
			var result = VerifyStudentFilesExists(student);
			if (result.HasProfileImage && result.HasFullImage && result.HasAudio) return;

			studentsWithoutAllFiles.Add(result);
			Notifier.LogMissingFiles(result);
		});
		return allStudents.IntersectBy(studentsWithoutAllFiles.Select((f)=>f.CharaName), (s) => s.charaName);
	}
	public async Task<IEnumerable<CharaListItem>> SearchDatabaseUpdates()
	{
		var charactersInPage = await charaListScanner.ScanCharaList();
		var charactersSqlite = await repository.GetAll();

		// Search Differences
		return charactersInPage.ExceptBy(charactersSqlite.Select(db => db.charaName), p => p.Name);
	}

	public FileVerification VerifyStudentFilesExists(Student student)
	{
		string targetPath = Path.Join(Constants.MediaPath, student.school, student.charaName);
		string profileImagePath = targetPath + ".png";
		string fullImagePath = targetPath + "_full.png";
		string audioPath = targetPath + ".ogg";

		return
			new FileVerification(
				CharaName: student.charaName,
				School: student.school,
				HasProfileImage: FileExists(profileImagePath),
				HasFullImage: FileExists(fullImagePath),
				HasAudio: FileExists(audioPath)
			);
	}
	public bool FileExists(string filePath)
	{
		var fileInfo = new FileInfo(filePath);
		return fileInfo.Exists && fileInfo.Length > 0;
	}
}