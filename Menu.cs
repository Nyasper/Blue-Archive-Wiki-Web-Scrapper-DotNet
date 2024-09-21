namespace BlueArchiveWebScrapper;

public static class Menu
{
  private static readonly string[] MainMenuOptions = 
  [
    "Scan Chara List and save it to JSON", //1
    "Search Updates", //2
    "Apply Updates", //3
    "Search Files Updates", //4
    "Apply Files Updates", //5
    "Update data.JSON" //6
  ];
  public static async Task MainMenu()
  {
    Console.Clear();
    LogMenu(MainMenuOptions);
    string? ask;
    int option;	
    bool parsedSuccefully;
    do {
      Console.Clear();
      LogMenu(MainMenuOptions);
      ask = Console.ReadLine();
      parsedSuccefully = int.TryParse(ask, out option);
    } while (!parsedSuccefully || option<0 || option>MainMenuOptions.Length);
    await MainMenuOptionHandler(option);
    Console.ReadKey();
  }
  public static bool YesNoMenu()
  {
    Console.WriteLine("\nproceed? (y/n)\n");
    char key = char.ToLower(Console.ReadKey(intercept:true).KeyChar);
    return key == 'y';
  }
  private static void LogMenu(string[] Options)
  {
    Console.WriteLine("\nSeleccionar una Opcion Valida");
    for (int i = 0; i < Options.Length; i++)
    {
      Console.WriteLine($"\n{i+1}: {Options[i]}");
    }
    Console.WriteLine("\n\n0: Salir\n");
  }
  private static async Task MainMenuOptionHandler (int Option)
  {
    switch (Option)
    {
      case 1: await CharaList.SaveCharaListInJSON(); break;
      case 2: await Updater.LogAvaiblesUpdates(); break;
      case 3: await Updater.ApplyUpdates(); break;
      case 4: await Updater.LogFilesUpdates(); break;
      case 5: await Updater.ApplyFilesUpdates(); break;
      case 6: await Updater.SaveDataJSON(); break;
      case 0: Environment.Exit(0); break;
      default: Console.WriteLine("Opcion no Valida"); break;
    }
  }

}
