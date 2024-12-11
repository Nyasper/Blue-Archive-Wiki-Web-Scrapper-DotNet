namespace Main.Utils;
public static class Menu
{
	private static readonly string Nl = Environment.NewLine;
  private static readonly string[] MainMenuOptions =
  [
    "Scan Chara List and save in a JSON", //1
    "Update Database", //2
    "Update Files", //3
  ];
  public static void MainMenu()
  {
    Console.Clear();
    LogMenu(MainMenuOptions);
    int option;
    bool successfully;
    do
    {
      Console.Clear();
      LogMenu(MainMenuOptions);
      string? ask = Console.ReadLine();
      successfully = int.TryParse(ask, out option);
    } while (!successfully || option < 0 || option > MainMenuOptions.Length);
	  MainMenuOptionHandler(option);
    Console.ReadKey();
  }
  public static bool YesNoQuestion(string questionMessage)
  {
    Console.WriteLine(Nl + questionMessage + "? (y/n)" + Nl);
    char key = char.ToLower(Console.ReadKey(intercept: true).KeyChar);
    bool response = key == 'y';
    if (response == false) Notifier.MessageTaskCancelled("Task Cancelled");
    return response;
  }
  private static void LogMenu(string[] options)
  {
    Console.WriteLine(Nl+"Select a valid option:");
    for (int i = 0; i < options.Length; i++)
    {
      Console.WriteLine($"\n{i + 1}: {options[i]}");
    }
    Console.WriteLine(Nl+Nl+"0: Exit\n");
  }
  private static void MainMenuOptionHandler(int option)
  {
    switch (option)
    {
      // case 1: await CharaList.SaveCharaListInJSON(); break;
      // case 2: await Updater.UpdateDatabase(); break;
      // case 3: await Updater.UpdateStudentsFiles(); break;
      case 0: Environment.Exit(0); break;
      default: Console.WriteLine("Invalid Option"); break;
    }
  }

}
