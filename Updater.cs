using System.Diagnostics;
using BlueArchiveWebScrapper.db;

namespace BlueArchiveWebScrapper;
public static class Updater
{
  public static async Task ApplyUpdates()
  {
    Console.WriteLine("\nUpdating DB...");
    List<CharaListInfo> AvailablesUpdates = await SearchUpdates();
    string[] NamesOnly = AvailablesUpdates.Select(x => x.name).ToArray();

    var AllInfoScaned = await CharaInfo.ScanManyCharasDetails(NamesOnly);
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
    Console.WriteLine("All characters updated succefully\n");
    await SaveDataJSON();
  }
  public static async Task SaveDataJSON()
  {
    List<Student> AllCharactersSqlite = await SqliteController.GetAllStudentsSqlite();
    await FileHandler.SaveDataInJSON(AllCharactersSqlite);
    Console.WriteLine($"data.json generated.\n");
    await FileHandler.CreateHTMLImagesPreview();
  }
  private static async Task<List<CharaListInfo>> SearchUpdates()
  {
    List<CharaListInfo> StudentsToUpdate = [];

    List<CharaListInfo> CharactersInPage = await CharaList.GetCharaList();
    List<Student> CharactersSqlite = await SqliteController.GetAllStudentsSqlite();

    int CharaPageCount = CharactersInPage.Count;
    int CharaSqliteCount = CharactersSqlite.Count;
    int newCharactersCount = CharaPageCount - CharaSqliteCount;

      Console.WriteLine($"\nChara in Page: {CharaPageCount}\nChara in DB: {CharaSqliteCount}");

    if (newCharactersCount > 0) StudentsToUpdate = SearchUpdatesDifferences(CharactersInPage, CharactersSqlite).ToList();
    else Console.WriteLine("All Characters in DB");

    return StudentsToUpdate;
  }
  public static async Task ApplyFilesUpdates()
  {
    List<Student> AvailablesUpdates = await SqliteController.GetAllStudentsWithoutFiles();

    var studentsToDownloadTasks = AvailablesUpdates.Select(c => c.DownloadFiles()).ToList();

    List<Task> failedTasks = [];

    int CounterRetry = 0;
    while (studentsToDownloadTasks.Count > 0)
    {
      if (CounterRetry == 2) break;
      try
      {
        await Task.WhenAll(studentsToDownloadTasks);;
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
    } else if (AvailablesUpdates.Count>0&&failedTasks.Count==0) Console.WriteLine("\nAll Files Download Succefully.");
    await SaveDataJSON();
  }
  public static async Task LogFilesUpdates()
  {
    Console.Clear();
    List<Student> AvailableFilesUpdates = await SqliteController.GetAllStudentsWithoutFiles();
    Console.WriteLine($"\n{AvailableFilesUpdates.Count} Available Students Files to Download\nTotal: {AvailableFilesUpdates.Count*3} files.");
  }
   public static async Task LogAvaiblesUpdates()
   {
    Console.Clear();
    List<CharaListInfo> AvailablesUpdates = await SearchUpdates();

    foreach (var update in AvailablesUpdates)
    {
      Console.WriteLine($"\n{update.name}  {update.url}");
    }
    Console.WriteLine($"\n{AvailablesUpdates.Count} Availables Updates.");
   }

  private static List<CharaListInfo> SearchUpdatesDifferences(List<CharaListInfo> FirstArray, List<Student> SecondArray)
  {
    var charaNames = new HashSet<string>(SecondArray.Select(b => b.charaName)); // HashSet tiene mas rendimiento
    return FirstArray.Where(a => charaNames.Contains(a.name)).ToList();
  }
}
