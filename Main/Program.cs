namespace Main;

using Scanner.CharaDetails;
using Repository;
using FileHandler.Creator;
using FileHandler.Downloader;
using FileHandler.Updater;
using FileHandler.Verifier;
using Scanner.Model;
using Scanner.CharaList;
using Scanner.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class Program
{
	public static async Task Main()
	{
		var services = new ServiceCollection()
			.AddDbContext<StudentContext>()
			.AddSingleton<IDownloader, Downloader>()
			.AddSingleton<ICreator, Creator>()
			.AddSingleton<ICharaListScanner, CharaListScanner>()
			.AddSingleton<ICharaDetails<Student>, CharaDetails>()
			.AddSingleton<IHtmlHandler, HtmlHandler>()
			.AddSingleton<IVerifier<Student>, Verifier>()
			.AddSingleton<Updater>()
			.AddSingleton<IRepository<Student>, Repository.Repository>()
			.BuildServiceProvider();

		await Run(services);
	}

	private static async Task Run(IServiceProvider services)
	{
		var updater = services.GetRequiredService<Updater>();
		await updater.UpdateAll();
		await updater.UpdateLocalFiles();
		Console.WriteLine("Press any key to finish");
		Console.ReadKey();
	}
}
