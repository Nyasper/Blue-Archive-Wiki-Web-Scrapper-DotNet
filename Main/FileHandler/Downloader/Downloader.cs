using Main.Repository;

namespace Main.FileHandler.Downloader;

using Scanner.Model;

using Repository;

using Utils;

public class Downloader : IDownloader
{
	private enum FileFormat
	{
		Audio,
		ImageFull,
		ImageProfile,
		SmallImage
	}

	public async Task DownloadFiles(Student student)
	{
		try
		{
			Task[] fileQueue = [
				Download(student, FileFormat.ImageProfile),
				Download(student, FileFormat.ImageFull),
				Download(student, FileFormat.Audio),
				Download(student, FileFormat.SmallImage)
			];
			await Task.WhenAll(fileQueue);
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on downloading files of: '{student.CharaName}'");
			throw;
		}
	}
	public async Task DownloadFiles(Student[] students)
	{
		try
		{
			Task[] toDownload = students.Select(DownloadFiles).ToArray();
			await Task.WhenAll(toDownload);
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on downloading many files.");
			throw;
		}
	}
	private static async Task Download(Student student, FileFormat fileFormat)
	{
		try
		{
			byte[] fileToDownload;
			string schoolPath = Path.Join(Constants.MediaPath, student.School);
			CreateFolderIfNotExist(schoolPath);
			string finalPath = Path.Join(schoolPath, student.CharaName);
			switch (fileFormat)
			{
				case FileFormat.ImageProfile:
					fileToDownload = await GetByteArray(student.ImageProfileUrl);
					Notifier.MessageTaskCompleted($"Downloaded image profile of'{student.CharaName}' from '{student.ImageProfileUrl}'");
					finalPath += ".png";
					break;
				case FileFormat.ImageFull:
					fileToDownload = await GetByteArray(student.ImageFullUrl);
					Notifier.MessageTaskCompleted($"Downloaded image full of'{student.CharaName}' from '{student.ImageFullUrl}'");
					finalPath += "_full.png";
					break;
				case FileFormat.SmallImage:
					fileToDownload = await GetByteArray(student.SmallImageUrl);
					finalPath += "_small.png";
					Notifier.MessageTaskCompleted($"Downloaded small image of'{student.CharaName}' from '{student.SmallImageUrl}' in {finalPath}");
					break;
				case FileFormat.Audio:
					fileToDownload = await GetByteArray(student.AudioUrl);
					Notifier.MessageTaskCompleted($"Downloaded audio of'{student.CharaName}' from '{student.AudioUrl}'");
					finalPath += ".ogg";
					break;
				default:
					throw new Exception("ERROR: Invalid file Format.");
			}
			await File.WriteAllBytesAsync(finalPath, fileToDownload);
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on downloading {fileFormat} of {student.CharaName}");
			throw;
		}
	}
	private static void CreateFolderIfNotExist(string folderName)
	{
		folderName = $"{folderName}";
		if (!Directory.Exists(folderName))
		{
			Directory.CreateDirectory(folderName);
		}
	}
	private static async Task<byte[]> GetByteArray(string fileUrl)
	{
		try
		{
			using var client = new HttpClient();
			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0");
			byte[] res = await client.GetByteArrayAsync(fileUrl);
			return res;
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on getting ByteArray from URL: '{fileUrl}'");
			throw;
		}
	}
}