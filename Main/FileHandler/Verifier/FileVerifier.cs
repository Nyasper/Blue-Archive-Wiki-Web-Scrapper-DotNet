namespace Main.FileHandler.Verifier;

using System.Collections.Concurrent;

using Repository;

using Scanner.Model;

using Utils;

public class FileVerifier() : IFileVerifier
{
	public FileVerification VerifyLocalFiles(Student student)
	{
		string outputDirectory = Path.Join(Constants.MediaPath, student.School, student.CharaName);
		string profileImageDirectory = outputDirectory + ".png";
		string fullImageDirectory = outputDirectory + "_full.png";
		string smallImageDirectory = outputDirectory + "_small.png";
		string audioDirectory = outputDirectory + ".ogg";

		return
			new FileVerification(
				CharaName: student.CharaName,
				School: student.School,
				HasProfileImage: FileExists(profileImageDirectory),
				HasFullImage: FileExists(fullImageDirectory),
				HasSmallImage: FileExists(smallImageDirectory),
				HasAudio: FileExists(audioDirectory)
			);
	}
	public FileVerification[] VerifyLocalFiles(Student[] students)
	{
		ConcurrentBag<FileVerification> studentsWithoutFiles = []; //ConcurrentBag<> = List<> but for Parallelism.

		Parallel.ForEach(students, student =>
		{
			FileVerification result = VerifyLocalFiles(student);
			if (!result.HasProfileImage || !result.HasFullImage || !result.HasSmallImage || !result.HasAudio)
			{
				studentsWithoutFiles.Add(result);
				if (!result.HasProfileImage)
				{
					Notifier.LogMissingFiles(result);
				}
			}
			;
		});

		return studentsWithoutFiles.ToArray();
	}
	private static bool FileExists(string filePath)
	{
		var fileInfo = new FileInfo(filePath);
		return fileInfo.Exists && fileInfo.Length > 0;
	}
}