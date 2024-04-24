namespace BlueArchiveWebScrapper;

using System.IO;
using System.Text.Json;


public static class FileHandler 
{
  private static readonly string DocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
  private static readonly string BlueArchiveWSPath = Path.Join(DocumentsPath,"BlueArchiveWS");
  private static readonly string MediaPath = Path.Join(BlueArchiveWSPath,"media");
  private static readonly string DataPath = Path.Join(BlueArchiveWSPath,"data");
  private readonly static JsonSerializerOptions JsonOptions = new()
    {
      PropertyNameCaseInsensitive = true,
      WriteIndented = true
    };

   public static async Task DownloadFiles(this Student student)
  {
    try
    {
      string SchoolPath = Path.Join(MediaPath, student.school);
      CreateFolderIfNotExist(SchoolPath);
      string FinalPath = Path.Join(SchoolPath, student.charaName);

      await DownloadImgProfile(student, FinalPath);
      await DownloadImgFull(student, FinalPath);
      await DownloadAudio(student, FinalPath);
      await SqliteController.FilesDownloaded(student.charaName);
      Console.WriteLine($"💙 Archivos de {student.charaName} Descargados 💙");
    }
    catch (Exception)
    {
      Console.WriteLine($"Error al intentar descargar los arhivos de: '{student.charaName}'");
      throw;
    }
  }
  private static async Task DownloadImgProfile(Student student,string FinalPath)
  {
    try
    {
      if (student.pageImageProfileUrl == null) throw new ArgumentNullException($"No imageprofileUrl to download from {student.charaName}");
      using var FileStream = await GetFileStream(student.pageImageProfileUrl);
      using var streamToWriteTo = File.Open(FinalPath+".png", FileMode.Create);
      await FileStream.CopyToAsync(streamToWriteTo);  
    }
    catch (Exception)
    {
      throw new Exception($"Error al intentar descargar 'image profile' de '{student.charaName}' en {student.pageImageProfileUrl}'");
    }
  }

  private static async Task DownloadImgFull(Student student, string FinalPath)
  {
     try
    {
      if (student.pageImageFullUrl == null) throw new ArgumentNullException($"No imageFullUrl to download from {student.charaName}");
      using var FileStream = await GetFileStream(student.pageImageFullUrl);
      using var streamToWriteTo = File.Open(FinalPath+"_full.png", FileMode.Create);
      await FileStream.CopyToAsync(streamToWriteTo);  
    }
    catch (Exception)
    {
      throw new Exception($"Error al intentar descargar 'image full' de '{student.charaName}' en '{student.pageImageFullUrl}'");
    }
  }

  private static async Task DownloadAudio(Student student, string FinalPath)
  {
    try
    {
      if (student.audioUrl == null) throw new ArgumentNullException($"No audioUrl to download from {student.charaName}");
      using var FileStream = await GetFileStream(student.audioUrl);
      using var streamToWriteTo = File.Open(FinalPath+".ogg", FileMode.Create);
      await FileStream.CopyToAsync(streamToWriteTo);  
    }
    catch (Exception)
    {
      throw new Exception($"Error al intentar descargar 'audio' de '{student.charaName}' en '{student.audioUrl}'");
    }
  }

  private static void CreateFolderIfNotExist(string FolderName)
  {
    FolderName = $"{FolderName}";
    if (!Directory.Exists(FolderName))
    {
      Directory.CreateDirectory(FolderName);
    }
  }

  public static async Task CreateJson(this Student student)
  {
    if (student.school == null) throw new ArgumentNullException($"school null on {student.charaName}");
    string JsonContent = JsonSerializer.Serialize(student,JsonOptions);

    string FinalPath = Path.Join(DataPath,student.school,student.charaName);
    CreateFolderIfNotExist(FinalPath);

    await WriteFile(FinalPath+".json",JsonContent);
  }

  public static async Task ReadJson(this Student student)
  {
    string FinalPath = Path.Join(DataPath,student.school,student.charaName);

    var JsonContent = await ReadFile(FinalPath+".json");
    var DeserializedJson = JsonSerializer.Deserialize<Student>(JsonContent);
    Console.WriteLine("El json es: "+DeserializedJson);
  }

   private static async Task<Stream> GetFileStream(string FileUrl)
  {
     try
    {
      using var client = new HttpClient();
      var res = await client.GetAsync(FileUrl);
      return await res.Content.ReadAsStreamAsync();
    }
    catch (Exception)
    {
      Console.WriteLine($"Error al intetnar obtener el Stream desde '{FileUrl}'");
      throw;
    }
  }
      public static async Task<string> ReadFile(string Path) 
  {
    try
    {
      using var reader = new StreamReader(Path);
      return await reader.ReadToEndAsync();
    }
    catch (Exception)
    {
      throw;
    }
    }

  private static async Task WriteFile(string Path,string Content) 
  {
    try
    {
      using var writer = new StreamWriter(Path);
      await writer.WriteAsync(Content);
    }
    catch (Exception)
    {
      throw;
    } 
  }

}
