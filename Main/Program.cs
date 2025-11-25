using Scanner;

namespace Main;

using FileHandler.Creator;
using FileHandler.Downloader;
using FileHandler.Updater;
using FileHandler.Verifier;

using Microsoft.Extensions.DependencyInjection;

using Repository;

using Scanner.CharaDetails;
using Scanner.CharaList;
using Scanner.Configuration;
using Scanner.Model;


public static class Program
{
	public static async Task Main()
	{
		IServiceProvider services = new ServiceCollection()
			.AddDbContext<StudentContext>()
			.AddSingleton<IRepository<Student>, Repository.Repository>()
			.AddSingleton<IHtmlHandler, HtmlHandler>()
			.AddSingleton<ICharaListScanner, CharaListScanner>()
			.AddSingleton<ICharaDetailsScanner, CharaDetailsScanner>()
			.AddSingleton<IDownloader, Downloader>()
			.AddSingleton<IFileVerifier, FileVerifier>()
			.AddSingleton<ICreator, Creator>()
			.AddSingleton<IUpdater, Updater>()
			.AddSingleton<IScanner<Student>, Scanner.Scanner>()
			.BuildServiceProvider();

		await Run(services);

	}

	private static async Task Run(IServiceProvider services)
	{
		IUpdater updater = services.GetRequiredService<IUpdater>();
		await updater.UpdateAll();

		Console.WriteLine("Press any key to finish");
		Console.ReadKey();
	}
}