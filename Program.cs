using System.Text.Json;

using BlueArchiveWebScrapper.db;
using BlueArchiveWebScrapper.model;

using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

using static BlueArchiveWebScrapper.Menu;

namespace BlueArchiveWebScrapper;

public class Program
{
	static async Task Main() => await MainMenu();
}
