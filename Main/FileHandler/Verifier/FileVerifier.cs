namespace Main.FileHandler.Verifier;
using System.Collections.Concurrent;

using Repository;

using Scanner.CharaList;
using Scanner.Model;

using Utils;

public class FileVerifier(IRepository<Student> repository) : IFileVerifier
{
	public FileVerification VerifyLocalFiles(Student student)
	{
		string outputDirectory = Path.Join(Constants.MediaPath, student.school, student.charaName);
		string profileImageDirectory = outputDirectory + ".png";
		string fullImageDirectory = outputDirectory + "_full.png";
		string audioDirectory = outputDirectory + ".ogg";

		return
			new FileVerification(
				CharaName: student.charaName,
				School: student.school,
				HasProfileImage: FileExists(profileImageDirectory),
				HasFullImage: FileExists(fullImageDirectory),
				HasAudio: FileExists(audioDirectory)
			);
	}
	public FileVerification[] VerifyLocalFiles(Student[] students)
	{
		ConcurrentBag<FileVerification> studentsWithoutAllFiles = []; //ConcurrentBag<> = List<> but for Parallelism.

		Parallel.ForEach(students, student =>
		{
			FileVerification result = VerifyLocalFiles(student);
			if (result.HasProfileImage && result.HasFullImage && result.HasAudio) return;

			studentsWithoutAllFiles.Add(result);
			Notifier.LogMissingFiles(result);
		});

		// return allStudents.IntersectBy(studentsWithoutAllFiles.Select((f) => f.CharaName), (s) => s.charaName);
		return studentsWithoutAllFiles.ToArray();
	}
	private static bool FileExists (string filePath) {
		var fileInfo = new FileInfo(filePath);
		return fileInfo.Exists && fileInfo.Length > 0;
	}
}