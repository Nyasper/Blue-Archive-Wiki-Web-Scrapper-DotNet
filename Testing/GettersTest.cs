using HtmlAgilityPack;

using Scanner.CharaDetails;
using Scanner.Utils;

using Scanner.Configuration;
using Scanner.Model;

namespace Testing;

[TestClass]
public class GettersTest
{
	private IHtmlHandler _htmlHandler;
	private HtmlDocument? _html;
	private const string studentCharaName = "Ichika_(Swimsuit)";
	// private const string studentCharaName = "Yukari_(Swimsuit)";

	private async Task<HtmlDocument> GetHtml()
	{
		return _html ??= await _htmlHandler.ScanHtml(Constants.BaseUrl + studentCharaName);
	}
	private async Task<DetailsGetter> GetGetter() => new DetailsGetter(await GetHtml(), studentCharaName);
	[TestInitialize]
	public void Setup()
	{
		_htmlHandler = new HtmlHandler();
	}

	[TestMethod]
	public async Task GetFullNameTest()
	{
		var getter = await GetGetter();
		(string name, string lastName) = getter.GetFullName();
		Console.WriteLine($"Testing name: {name}\nTesting lastName: {lastName}");

		Assert.IsFalse(string.IsNullOrWhiteSpace(name));
		Assert.IsFalse(string.IsNullOrWhiteSpace(lastName));

		Assert.IsGreaterThan(0, name.Length);
		Assert.IsGreaterThan(0, lastName.Length);
	}
	[TestMethod]
	public async Task GetAge()
	{
		var getter = await GetGetter();
		int? age = getter.GetAge();
		Console.WriteLine($"Testing age: {age}");

		if (age is not null)
		{
			int ageInt = (int)age;
			Assert.IsInstanceOfType<int>(ageInt);
			Assert.IsInRange(0, 50, ageInt);
		}
	}
	[TestMethod]
	public async Task GetBirthDate()
	{
		var getter = await GetGetter();
		string? birthday = getter.GetBirthday();
		Console.WriteLine($"Testing birthday: {birthday}");

		if (birthday is not null)
		{
			Assert.IsGreaterThan(0, birthday.Length);
		}
	}
	[TestMethod]
	public async Task GetHeight()
	{
		var getter = await GetGetter();
		int height = getter.GetHeight() ?? 0;
		Console.WriteLine($"Testing height: {height}");

		Assert.IsInstanceOfType<int>(height);
		Assert.IsInRange(100, 200, height, "height should be between 100 and 200");
	}
	[TestMethod]
	public async Task GetHobbies()
	{
		var getter = await GetGetter();
		string hobbies = getter.GetHobbies();
		Console.WriteLine($"Testing hobbies: {hobbies}");

		Assert.IsFalse(string.IsNullOrWhiteSpace(hobbies));
		Assert.IsGreaterThan(0, hobbies.Length);
	}
	[TestMethod]
	public async Task GetDesigner()
	{
		var getter = await GetGetter();
		string? designer = getter.GetDesigner();
		Console.WriteLine($"Testing designer: {designer}");

		if (designer is not null)
		{
			Assert.IsGreaterThan(0, designer.Length);
		}
	}
	[TestMethod]
	public async Task GetIllustrator()
	{
		var getter = await GetGetter();
		string? illustrator = getter.GetIllustrator();
		Console.WriteLine($"Testing illustrator: {illustrator}");

		if (illustrator is not null)
		{
			Assert.IsGreaterThan(0, illustrator.Length);
		}
	}
	[TestMethod]
	public async Task GetVoice()
	{
		var getter = await GetGetter();
		string voice = getter.GetVoice();
		Console.WriteLine($"Testing voice: {voice}");

		Assert.IsFalse(string.IsNullOrWhiteSpace(voice));
		Assert.IsGreaterThan(0, voice.Length);
	}
	[TestMethod]
	public async Task GetPageUrl()
	{
		var getter = await GetGetter();
		string pageUrl = getter.GetPageUrl();
		Console.WriteLine($"Testing pageUrl: {pageUrl}");

		Assert.IsFalse(string.IsNullOrWhiteSpace(pageUrl));
		Assert.IsGreaterThan(0, pageUrl.Length);
		Assert.StartsWith("https://bluearchive.wiki/wiki/", pageUrl);
	}
	[TestMethod]
	public async Task GetImageProfileUrl()
	{
		var getter = await GetGetter();
		string imageProfileUrl = getter.GetImageProfileUrl();
		Console.WriteLine($"Testing imageProfileUrl: {imageProfileUrl}");

		Assert.IsFalse(string.IsNullOrWhiteSpace(imageProfileUrl));
		Assert.IsGreaterThan(0, imageProfileUrl.Length);
		Assert.StartsWith("https://static.", imageProfileUrl);
		Assert.EndsWith(".png", imageProfileUrl);
	}
	[TestMethod]
	public async Task GetImageFullUrl()
	{
		var getter = await GetGetter();
		string imageFullUrl = await getter.GetImageFullUrl();
		Console.WriteLine($"Testing imageFullUrl: {imageFullUrl}");

		Assert.IsFalse(string.IsNullOrWhiteSpace(imageFullUrl));
		Assert.IsGreaterThan(0, imageFullUrl.Length);
		Assert.StartsWith("https://static.", imageFullUrl);
		Assert.EndsWith(".png", imageFullUrl);
	}
	[TestMethod]
	public async Task GetAudioUrl()
	{
		var getter = await GetGetter();

		string audioUrl = getter.GetAudioUrl();
		Console.WriteLine("Testing audioUrl: " + audioUrl);

		Assert.IsFalse(string.IsNullOrWhiteSpace(audioUrl));
		Assert.IsGreaterThan(0, audioUrl.Length);
		Assert.StartsWith("https://static.", audioUrl);
		Assert.EndsWith(".ogg", audioUrl);
	}
}