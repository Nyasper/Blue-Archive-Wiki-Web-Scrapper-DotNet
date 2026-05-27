using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Main.FileHandler.Downloader;

using Scanner.Model;
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

	private static readonly HttpClient HttpClient;
	private static readonly SemaphoreSlim ConcurrencySemaphore = new SemaphoreSlim(5);

	static Downloader()
	{
		HttpClient = new HttpClient();
		HttpClient.DefaultRequestHeaders.Add("User-Agent",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (HTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0");
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
		var tasks = students.Select(async student =>
		{
			await ConcurrencySemaphore.WaitAsync();
			try
			{
				await DownloadFiles(student);
			}
			finally
			{
				ConcurrencySemaphore.Release();
			}
		});
		await Task.WhenAll(tasks);
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
					finalPath += "_full.png";
					break;
				case FileFormat.SmallImage:
					fileToDownload = await GetByteArray(student.SmallImageUrl);
					finalPath += "_small.png";
					break;
				case FileFormat.Audio:
					fileToDownload = await GetByteArray(student.AudioUrl);
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
		if (!Directory.Exists(folderName))
		{
			Directory.CreateDirectory(folderName);
		}
	}
	private static async Task<byte[]> GetByteArray(string fileUrl)
	{
		try
		{
			byte[] res = await HttpClient.GetByteArrayAsync(fileUrl);
			return res;
		}
		catch (HttpRequestException httpRequestException)
		{
			Console.WriteLine($"Error {httpRequestException.StatusCode} from URL: '{fileUrl}'");
			throw;
		}
		catch (Exception)
		{
			Console.WriteLine($"Error on getting ByteArray from URL: '{fileUrl}'");
			throw;
		}
	}
}