using System.Collections.Concurrent;
using System.Diagnostics;

using BlueArchiveWebScrapper.db;
using BlueArchiveWebScrapper.model;

namespace BlueArchiveWebScrapper;
public static class Updater
{
  public static async Task UpdateDatabase()
  {
    Notifier.NewBlankMessage("Update Database");

    Notifier.MessageInitiatingTask("Searching for Updates");
    IEnumerable<CharaListInfo> StudentsCollection = await SearchDatabaseUpdates();
    int AvailablesUpdates = StudentsCollection.Count();

    if (AvailablesUpdates == 0)
    {
      Notifier.MessageNothingToDo("All Students in Database");
      return;
    }

    Notifier.LogStudentsList($"{AvailablesUpdates} New Students to Save:", StudentsCollection);
    bool ccontinue = Menu.YesNoQuestion("Proceed to update the database");
    if (!ccontinue) return;

    Notifier.MessageInitiatingTask("Scanning Students Data");
    string[] CharaNames = StudentsCollection.Select(s => s.name).ToArray(); // Get only de names
    Student?[] AllInfoScaned = await CharaInfo.ScanManyCharasDetails(CharaNames);
    Notifier.MessageTaskCompleted("All data scanned succefully");

    Notifier.MessageInitiatingTask("Saving data in Database");
    await SqliteController.SaveManyInDatabase(AllInfoScaned!);
    Notifier.MessageTaskCompleted("All characters updated succefully");

    await GenerateDataJSON();
  }
  public static async Task UpdateStudentsFiles()
  {
    Notifier.NewBlankMessage("Files Update");

    Notifier.MessageInitiatingTask("Verifying All Students Files");
    Student[] AvailablesUpdates = await VerifyAllStudentsFiles();

    if (AvailablesUpdates.Length == 0)
    {
      Notifier.MessageNothingToDo("All students files OK");
      return;
    }

    Notifier.LogStudentsList($"{AvailablesUpdates.Length} New Student's Files to Save:", AvailablesUpdates);

    bool Continue = Menu.YesNoQuestion("Proceed to download the files");
    if (!Continue) return;

    Notifier.MessageInitiatingTask("Downloading missing Students Files");
    Task[] studentsToDownloadTasks = AvailablesUpdates.Select(FileHandler.DownloadFiles).ToArray();

    try
    {
      await Task.WhenAll(studentsToDownloadTasks);
      Notifier.MessageTaskCompleted("All students files Downloaded Succefully");
    }
    catch (Exception ex)
    {
      if (ex is AggregateException aggregateException)
      {
        foreach (var innerException in aggregateException.InnerExceptions)
        {
          Console.WriteLine($"\nERROR: {innerException.Message}");
        }
      }
      else
      {
        Console.WriteLine($"\nERROR: {ex.Message}");
      }
      throw;
    }
    await GenerateHTMLImagePreview();
  }
  private static async Task<Student[]> VerifyAllStudentsFiles()
  {
    Student[] AllStudents = await SqliteController.GetAllStudents();
    ConcurrentBag<Student> StudentsWithoutAllFiles = []; //ConcurrentBag<> = List<> but for Paralelism.

    Parallel.ForEach(AllStudents, student =>
    {
      var result = FileHandler.VerifyStudentFilesExists(student);

      if (!result.HasProfileImage || !result.HasFullImage || !result.HasAudio)
      {
        StudentsWithoutAllFiles.Add(student);
      }

      if (!result.HasProfileImage) Notifier.MessageFileMissing(type: "Profile Image", result);
      if (!result.HasFullImage) Notifier.MessageFileMissing(type: "Full Image", result);
      if (!result.HasAudio) Notifier.MessageFileMissing(type: "Audio", result);
    });
    return [.. StudentsWithoutAllFiles];
  }
  private static async Task<IEnumerable<CharaListInfo>> SearchDatabaseUpdates()
  {
    HashSet<CharaListInfo> CharactersInPage = await CharaList.GetCharaList();
    Student[] CharactersSqlite = await SqliteController.GetAllStudents();

    // Search Differences
    HashSet<string> charaNames = new(CharactersSqlite.Select(b => b.charaName)); // HashSet has more performance
    IEnumerable<CharaListInfo> option1 = CharactersInPage.Where(a => !charaNames.Contains(a.name));
    // CharaListInfo[] option2 = CharactersInPage.ExceptBy(CharactersSqlite.Select(s => s.charaName), s => s.name).ToArray();
    return option1;
    //TODO: ver si option 2 funciona bien
  }
  private static async Task GenerateDataJSON()
  {
    Student[] students = await SqliteController.GetAllStudents();
    string JSONpath = await FileHandler.SaveDataInJSON(model: students, FileName: "data");

    Notifier.MessageTaskCompleted($"data json generated in: {JSONpath}");
  }
  private static async Task GenerateHTMLImagePreview()
  {
    Student[] students = await SqliteController.GetAllStudents();
    string HTMLPath = await FileHandler.CreateHTMLImagesPreview(students);

    Notifier.MessageTaskCompleted($"Images preview HTML generated in: {HTMLPath}");
  }
}
