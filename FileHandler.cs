namespace BlueArchiveWebScrapper;

using System.IO;

public static class FileHandler 
{
  private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
  private static readonly string BlueArchiveWSPath = Path.Join(DocumentsPath,"BlueArchiveWS");
  private static readonly string MediaPath = Path.Join(BlueArchiveWSPath,"media");
  private enum FileFormats{
    audio,
    ImageFull,
    ImageProfile
  }
  public static async Task DownloadFiles(this Student student)
  {
    try
    {
      string SchoolPath = Path.Join(MediaPath, student.school);
      CreateFolderIfNotExist(SchoolPath);
    
      Task[] FileQueue = [
        Download(student, FileFormats.ImageProfile),
        Download(student, FileFormats.ImageFull),
        Download(student, FileFormats.audio)
      ];

      await Task.WhenAll(FileQueue);
      await SqliteController.FilesDownloaded(student.charaName);
      Console.WriteLine($"💙 Archivos de {student.charaName} Descargados 💙");
    }
    catch (Exception)
    {
      Console.WriteLine($"Error on downloading files of: '{student.charaName}'");
      throw;
    }
  }

  static private async Task Download(Student student, FileFormats fileFormat)
  {
    try
    {
      byte[] FileToDownload;
      string SchoolPath = Path.Join(MediaPath, student.school);
      CreateFolderIfNotExist(SchoolPath);
      string FinalPath = Path.Join(SchoolPath, student.charaName);

      if (fileFormat == FileFormats.ImageProfile) 
      {
        FileToDownload = await GetByteArray(student.pageImageProfileUrl);
        FinalPath += ".png";
      }
      else if (fileFormat == FileFormats.ImageFull)
      {
        FileToDownload = await GetByteArray(student.pageImageFullUrl);
        FinalPath += "_full.png";
      }
      else if (fileFormat == FileFormats.audio)
      {
        FileToDownload = await GetByteArray(student.audioUrl);
        FinalPath += ".ogg";
      }
      else throw new Exception("ERROR: Invalid file Format.");

      await File.WriteAllBytesAsync(FinalPath, FileToDownload);
      Console.WriteLine($"{student.charaName} Files downloaded on {FinalPath}");
    }
    catch (Exception)
    {
      Console.WriteLine($"Error on dowloading {fileFormat} of {student.charaName}");
      throw;
    }
  }

  public static async Task nuevoMetodoParaDescargarArchivo(this Student student)
  {
    string DesktopFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "probandoCsharp");
    CreateFolderIfNotExist(DesktopFolder);
    string FinalPath = Path.Join(DesktopFolder, student.charaName+".png");


    using var client = new HttpClient();
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0");
    byte[] imageBytes = await client.GetByteArrayAsync(student.pageImageFullUrl);

    // save the image to the hard drive
    await File.WriteAllBytesAsync(FinalPath, imageBytes);
  }
  private static void CreateFolderIfNotExist(string FolderName)
  {
    FolderName = $"{FolderName}";
    if (!Directory.Exists(FolderName))
    {
      Directory.CreateDirectory(FolderName);
    }
  }
  private static async Task<byte[]> GetByteArray(string FileUrl)
  {
     try
    {
      using var client = new HttpClient();
       client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0");
      byte[] res = await client.GetByteArrayAsync(FileUrl);
      return res;
    }
    catch (Exception)
    {
      Console.WriteLine($"Error on getting ByteArray from URL: '{FileUrl}'");
      throw;
    }
  }
}

