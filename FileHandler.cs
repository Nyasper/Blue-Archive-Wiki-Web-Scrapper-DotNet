namespace BlueArchiveWebScrapper;

using System.IO;
using System.Text.Json;

using BlueArchiveWebScrapper.db;
using BlueArchiveWebScrapper.model;

public static class FileHandler
{
  private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
  private static readonly string BlueArchiveWSPath = Path.Join(DocumentsPath, "BlueArchiveWS");
  private static readonly string MediaPath = Path.Join(BlueArchiveWSPath, "media");
  private enum FileFormats
  {
    audio,
    ImageFull,
    ImageProfile
  }
  public static async Task DownloadFiles(Student student)
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
    }
    catch (Exception)
    {
      Console.WriteLine($"Error on downloading files of: '{student.charaName}'");
      throw;
    }
  }
  public static async Task<string> SaveDataInJSON<T>(IEnumerable<T> model, string FileName)
  {
    string DataFinalPath = Path.Join(BlueArchiveWSPath, FileName + ".json");

    string jsonData = JsonSerializer.Serialize(model);
    await File.WriteAllTextAsync(DataFinalPath, jsonData);
    return DataFinalPath;
  }
  public static async Task<string> CreateHTMLImagesPreview(Student[] students)
  {
    const string FileName = "imagesPreview";
    IGrouping<string, Student>[] allSchools = students.GroupBy((s) => s.school).ToArray();

    // Generar el contenido HTML
    const string HTMLHeader = "<html>\n<head>\n<link rel=\"stylesheet\" href=\"style.css\">\n<title>Students Images Preview</title>\n</head>\n<body>";
    const string stylesCSS = @"
:root {
    color-scheme: light dark;
}

* {
    box-sizing: border-box;
}

body {
    margin: 0;
    padding: 0 15px;
    background-color: rgb(26, 26, 26);
}

.schoolTitle {
    font-size: 2.6;
    margin: 15px 0;
    text-align: center;
}

.schoolContainer {
    border-radius: 10px;
    display: grid;
    grid-template-columns: repeat(6, 1fr);
    place-items: center;
    margin: 15px 0;
    padding: 10px 20px;
    background-color: rgba(53, 53, 53, 0.473);
}

.studentContainer > h2 {
    text-align: center;
    margin: 5px 0;
}

.imageContainer {
    display: flex;
    flex-direction: row;
    align-items: center;
    height: 160px;
    width: 300px;
    margin: 5px 0;
}

.imageContainer img {
    height: 100%;
    width: 100%;
    object-fit: contain;
    overflow: hidden;
}";
    const string HTMLFooter = $"\n<style>{stylesCSS}\n</style>\n</body>\n</html>";
    List<string> SchoolContainer = allSchools.Select(school => string.Join("\n", [$"\n<h2 class=\"schoolTitle\">{school.Key}</h2>\n  <div class=\"schoolContainer\">\n ",string.Join("\n",school.Select((student, index) =>
      $" <div class=\"studentContainer\">\n  <h2>{index + 1}: {student.charaName}</h2>\n  <div class=\"imageContainer\">\n   <img src=\"media/{student.school}/{student.charaName}.png\" class=\"profileImage\" alt=\"profileImage of {student.charaName}\"></img>\n   <img src=\"media/{student.school}/{student.charaName}_full.png\" class=\"fullImage\" alt=\"fullImage of {student.charaName}\">\n </div>\n</div>")),"</div>"])).ToList();

    string HtmlContent = string.Join("\n", SchoolContainer);

    // Concatenar todas las partes del HTML
    string finalHTML = $"{HTMLHeader}\n{HtmlContent}{HTMLFooter}";

    string filePath = Path.Join(BlueArchiveWSPath, FileName + ".html");
    await File.WriteAllTextAsync(filePath, finalHTML);
    return filePath;
  }
  private static async Task Download(Student student, FileFormats fileFormat)
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
    }
    catch (Exception)
    {
      Console.WriteLine($"Error on dowloading {fileFormat} of {student.charaName}");
      throw;
    }
  }
  public static FileVerification VerifyStudentFilesExists(Student student)
  {
    string BasePath = Path.Join(MediaPath, student.school, student.charaName);
    string profileImagePath = BasePath + ".png";
    string fullImagePath = BasePath + "_full.png";
    string audioPath = BasePath + ".ogg";

    return
      new(
        CharaName: student.charaName,
        School: student.school,
        HasProfileImage: FileExists(profileImagePath),
        HasFullImage: FileExists(fullImagePath),
        HasAudio: FileExists(audioPath)
      );
  }
  private static bool FileExists(string FilePath)
  {
    var fileInfo = new FileInfo(FilePath);
    return fileInfo.Exists && fileInfo.Length > 0;
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
  public record FileVerification(string CharaName, string School, bool HasProfileImage, bool HasFullImage, bool HasAudio);
}



