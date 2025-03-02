namespace Main.FileHandler.Downloader;
using Scanner.Model;


public interface IDownloader
{
	Task DownloadFiles(Student student);
	Task DownloadFiles(Student[] students);
}