namespace BlueArchiveWebScrapper;

public static class Menu
{
  private static readonly string[] MainMenuOptions =
  [
    "Scan Chara List and save in a JSON", //1
    "Update Database", //2
    "Update Files", //3
  ];
  public static async Task MainMenu()
  {
    Console.Clear();
    LogMenu(MainMenuOptions);
    string? ask;
    int option;
    bool parsedSuccefully;
    do
    {
      Console.Clear();
      LogMenu(MainMenuOptions);
      ask = Console.ReadLine();
      parsedSuccefully = int.TryParse(ask, out option);
    } while (!parsedSuccefully || option < 0 || option > MainMenuOptions.Length);
    await MainMenuOptionHandler(option);
    Console.ReadKey();
  }
  public static bool YesNoQuestion(string questionMessage)
  {
    Console.WriteLine(Notifier.NL + questionMessage + "? (y/n)" + Notifier.NL);
    char key = char.ToLower(Console.ReadKey(intercept: true).KeyChar);
    bool response = key == 'y';
    if (response == false) Notifier.MessageTaskCancelled("Task Cancelled");
    return response;
  }
  private static void LogMenu(string[] Options)
  {
    Console.WriteLine("\nSeleccionar una Opcion Valida");
    for (int i = 0; i < Options.Length; i++)
    {
      Console.WriteLine($"\n{i + 1}: {Options[i]}");
    }
    Console.WriteLine("\n\n0: Salir\n");
  }
  private static async Task MainMenuOptionHandler(int Option)
  {
    switch (Option)
    {
      case 1: await CharaList.SaveCharaListInJSON(); break;
      case 2: await Updater.UpdateDatabase(); break;
      case 3: await Updater.UpdateStudentsFiles(); break;
      case 0: Environment.Exit(0); break;
      default: Console.WriteLine("Opcion no Valida"); break;
    }
  }

}
