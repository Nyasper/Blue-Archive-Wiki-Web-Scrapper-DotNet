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
		ImageProfile
	}

	public async Task DownloadFiles(Student student)
	{
		try
		{
			Task[] fileQueue = [
				Download(student, FileFormat.ImageProfile),
				Download(student, FileFormat.ImageFull),
				Download(student, FileFormat.Audio)
			];
			await Task.WhenAll(fileQueue);
			Notifier.MessageTaskCompleted($"updated files of '{student.charaName}'");
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on downloading files of: '{student.charaName}'");
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
			string schoolPath = Path.Join(Constants.MediaPath, student.school);
			CreateFolderIfNotExist(schoolPath);
			string finalPath = Path.Join(schoolPath, student.charaName);
			switch (fileFormat)
			{
				case FileFormat.ImageProfile:
					fileToDownload = await GetByteArray(student.pageImageProfileUrl);
					finalPath += ".png";
					break;
				case FileFormat.ImageFull:
					Console.WriteLine($"Downloading image full of'{student.charaName}' from '{student.pageImageFullUrl}'");
					fileToDownload = await GetByteArray(student.pageImageFullUrl);
					finalPath += "_full.png";
					break;
				case FileFormat.Audio:
					fileToDownload = await GetByteArray(student.audioUrl);
					finalPath += ".ogg";
					break;
				default:
					throw new Exception("ERROR: Invalid file Format.");
			}
			await File.WriteAllBytesAsync(finalPath, fileToDownload);
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on downloading {fileFormat} of {student.charaName}");
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