using System.ComponentModel.DataAnnotations;

namespace BlueArchiveWebScrapper;

public class Updater
{

  public static async Task ApplyUpdates()
  {
    CharaListInfo[] AvailablesUpdates = await SearchUpdates();
    string[] NamesOnly = AvailablesUpdates.Select(x => x.name).ToArray();

    var AllInfoScaned = await CharaDetailInfo.ScanManyCharasDetails(NamesOnly);
    foreach (var CharaInfo in AllInfoScaned)
    {
      try
      {
        if (CharaInfo != null)
        {
          await CharaInfo.SaveSqlite();
        }
      }
      catch (Exception)
      {
        throw new Exception($"Error en 'ApplyUpdates()' personaje: '{CharaInfo?.charaName}'\n");
      }
    }
  }
  private static async Task<CharaListInfo[]> SearchUpdates()
  {
    CharaListInfo[] StudentsToUpdate = [];

    List<CharaListInfo> CharactersInPage = await CharaList.ScanCharaList();
    Student[] CharactersSqlite = await SqliteController.GetAllStudentsSqlite();

    int CharaPageCount = CharactersInPage.Count;
    int CharaSqliteCount = CharactersSqlite.Length;
    int newCharactersCount = CharaPageCount - CharaSqliteCount;

      Console.WriteLine($"\nChara in Page: {CharaPageCount}\nChara in DB: {CharaSqliteCount}");

    if (newCharactersCount > 0) StudentsToUpdate = SearchUpdatesDifferences(CharactersInPage, CharactersSqlite);
    else Console.WriteLine("All Characters in DB");

    return StudentsToUpdate;
  }
  public static async Task ApplyFilesUpdates()
  {
    Student[] AvailablesUpdates = await SqliteController.GetAllStudentsWithoutFiles();
      
    IEnumerable<Task> StudentsToDownloadTasks = AvailablesUpdates.Select(c => c.DownloadFiles());
    try
    {
    }
    catch (System.Exception)
    {
      
      throw;
    }

    
    await Task.WhenAll(StudentsToDownloadTasks);
    // Parallel.ForEachAsync(StudentsToDownload, (student)=>student);

    Console.WriteLine($"\n{StudentsToDownloadTasks.Count()} Student Files Downloaded.");
  }

  public static async Task ApplyFilesUpdatesBETA()
  {
     Student[] AvailablesUpdates = await SqliteController.GetAllStudentsWithoutFiles();

    var studentsToDownloadTasks = AvailablesUpdates.Select(c => c.DownloadFiles()).ToList();

    List<Task> failedTasks = [];

    int CounterRetry = 0;
    while (studentsToDownloadTasks.Count > 0)
    {
      if (CounterRetry == 2) break;
      try
      {
        await Task.WhenAll(studentsToDownloadTasks);
        break;
      }
      catch
      {
        failedTasks.AddRange(studentsToDownloadTasks.Where(t => t.IsFaulted));
        
        studentsToDownloadTasks = studentsToDownloadTasks.Where(t => !t.IsFaulted).ToList();
        CounterRetry++;
      }
      studentsToDownloadTasks.AddRange(failedTasks);
      await Task.Delay(200);
    }
    if (failedTasks.Count > 0)
    {
      Console.WriteLine($"\n{failedTasks.Count} Files failed please download again.");
    } else if (AvailablesUpdates.Length>0&&failedTasks.Count==0) Console.WriteLine("\nAll Files Download Succefully.");
  }
  public static async Task LogFilesUpdates()
  {
    Console.Clear();
    Student[] AvailableFilesUpdates = await SqliteController.GetAllStudentsWithoutFiles();
    Console.WriteLine($"\n{AvailableFilesUpdates.Length} Available Students Files to Download\nTotal: {AvailableFilesUpdates.Length*3} files.");
  }
   public static async Task LogAvaiblesUpdates()
   {
    Console.Clear();
    CharaListInfo[] AvailablesUpdates = await SearchUpdates();

    foreach (var update in AvailablesUpdates)
    {
      Console.WriteLine($"\n{update.name}  {update.url}");
    }
    Console.WriteLine($"\n{AvailablesUpdates.Length} Availables Updates.");
   }

  private static CharaListInfo[] SearchUpdatesDifferences(List<CharaListInfo> FirstArray, Student[] SecondArray)
  {
    return FirstArray.Where(a => SecondArray.All(b => b.charaName != a.name)).ToArray();
  }
}
