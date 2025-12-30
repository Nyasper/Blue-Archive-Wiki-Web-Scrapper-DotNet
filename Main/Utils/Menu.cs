using System.Diagnostics;

namespace Main.Utils;

public static class Menu
{
	private static readonly string Nl = Environment.NewLine;
	public static void MainMenu()
	{
		string[] MainMenuOptions =
			[
				"Scan Chara List and save in a JSON", //1
			"Update Database", //2
			"Update Files", //3
		];
		Console.Clear();
		int option = LogMenu(MainMenuOptions);

		MainMenuOptionHandler(option);
		Console.ReadKey();
	}
	public static bool YesNoQuestion(string questionMessage)
	{
		Console.WriteLine(Nl + questionMessage + " (y/n)" + Nl);
		char key = char.ToLower(Console.ReadKey(intercept: true).KeyChar);
		bool response = key == 'y';
		return response;
	}
	public static int LogMenu(string[] options)
	{
		Console.Clear();
		Console.WriteLine(Nl + "Select a valid option:");
		for (int i = 0; i < options.Length; i++)
		{
			Console.WriteLine($"\n{i + 1}: {options[i]}");
		}
		Console.WriteLine(Nl + Nl + "0: Exit\n");

		int option;
		bool successfully;
		do
		{
			string? ask = Console.ReadLine();
			successfully = int.TryParse(ask, out option);
		} while (!successfully || option < 0 || option > options.Length);
		if (option == 0)
		{
			Environment.Exit(0);
			Notifier.MessageTaskCancelled("Operation cancelled by user.");
		}

		Console.Clear();
		Console.WriteLine($"Option Selected: {option}: {options[option - 1]}{Nl}");
		return option;


	}
	private static void MainMenuOptionHandler(int option)
	{
		switch (option)
		{
			// case 1: await CharaList.SaveCharaListInJSON(); break;
			// case 2: await Updater.UpdateDatabase(); break;
			// case 3: await Updater.UpdateStudentsFiles(); break;
			default: Console.WriteLine("Invalid Option"); break;
		}
	}

}