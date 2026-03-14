using FluentAssertions;

namespace Testing;

using Xunit;
using HtmlAgilityPack;
using Scanner.CharaDetails;
using Scanner.Utils;
using Scanner.Configuration;


public class GettersTest
{
	private HtmlDocument? _html;
	private HttpClient _httpClient;
	private readonly IHtmlHandler _htmlHandler;

	private const string StudentCharaName = "Eimi_(Swimsuit)";
	private const string ExpectedImageContentType = "image/png";
	private const string ExpectedAudioContentType = "application/ogg";

	public GettersTest()
	{
		_htmlHandler = new HtmlHandler();
		_httpClient = new HttpClient();
		_httpClient.DefaultRequestHeaders.Add("User-Agent",
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (HTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0");
	}
	
	private async Task<HtmlDocument> GetHtml()
	{
		return _html ??= await _htmlHandler.ScanHtml(Constants.BaseUrl + StudentCharaName);
	}
	private async Task<DetailsGetter> GetGetter() => new(await GetHtml(), StudentCharaName);
	

	[Fact]
	public async Task GetFullNameTest()
	{
		var getter = await GetGetter();
		(string name, string lastName) = getter.GetFullName();
		Console.WriteLine($"Testing name: {name}\nTesting lastName: {lastName}");

		name.Should().NotBeNullOrEmpty().And.NotBeNullOrWhiteSpace();
		lastName.Should().NotBeNullOrEmpty().And.NotBeNullOrWhiteSpace();
	}
	[Fact]
	public async Task GetAge()
	{
		var getter = await GetGetter();
		int? age = getter.GetAge();
		Console.WriteLine($"Testing age: {age}");

		if (age is not null)
		{
			int ageInt = (int)age;
			
			ageInt.Should().BeOfType(typeof(int));
			ageInt.Should().BeInRange(0, 50);
		}
	}
	[Fact]
	public async Task GetBirthDate()
	{
		var getter = await GetGetter();
		string? birthday = getter.GetBirthday();
		Console.WriteLine($"Testing birthday: {birthday}");

		birthday?.Length.Should().BeGreaterThan(0);
	}
	[Fact]
	public async Task GetHeight()
	{
		var getter = await GetGetter();
		int height = getter.GetHeight() ?? 0;
		Console.WriteLine($"Testing height: {height}");

		height.Should().BeOfType(typeof(int));
		height.Should().BeInRange(100, 200, "height should be between 100 and 200");
	}
	[Fact]
	public async Task GetHobbies()
	{
		var getter = await GetGetter();
		string hobbies = getter.GetHobbies();
		Console.WriteLine($"Testing hobbies: {hobbies}");

		hobbies.Should().NotBeNullOrWhiteSpace();
		hobbies.Length.Should().BeGreaterThan(0);
	}
	[Fact]
	public async Task GetDesigner()
	{
		var getter = await GetGetter();
		string? designer = getter.GetDesigner();
		Console.WriteLine($"Testing designer: {designer}");

		if (designer is not null)
		{
			designer.Length.Should().BeGreaterThan(0);
		}
	}
	[Fact]
	public async Task GetIllustrator()
	{
		var getter = await GetGetter();
		string? illustrator = getter.GetIllustrator();
		Console.WriteLine($"Testing illustrator: {illustrator}");

		if (illustrator is not null)
		{
			illustrator.Length.Should().BeGreaterThan(0);
		}
	}
	[Fact]
	public async Task GetVoice()
	{
		var getter = await GetGetter();
		string voice = getter.GetVoice();
		Console.WriteLine($"Testing voice: {voice}");

		voice.Should().NotBeNullOrWhiteSpace();
		voice.Length.Should().BeGreaterThan(0);
	}
	[Fact]
	public async Task GetPageUrl()
	{
		var getter = await GetGetter();
		string pageUrl = getter.GetPageUrl();
		Console.WriteLine($"Testing pageUrl: {pageUrl}");

		pageUrl.Should().NotBeNullOrWhiteSpace();
		pageUrl.Length.Should().BeGreaterThan(0);
		pageUrl.Should().StartWith("https://bluearchive.wiki/wiki/");
	}
	[Fact]
	public async Task GetImageProfileUrl()
	{
		var getter = await GetGetter();
		string imageProfileUrl = getter.GetImageProfileUrl();
		Console.WriteLine($"Testing imageProfileUrl: {imageProfileUrl}");

		imageProfileUrl.Should().NotBeNullOrWhiteSpace();
		imageProfileUrl.Length.Should().BeGreaterThan(0);
		imageProfileUrl.Should().StartWith("https://static.");
		imageProfileUrl.Should().EndWith(".png");
		
		var request = new HttpRequestMessage(HttpMethod.Head, imageProfileUrl);
		var response = await _httpClient.SendAsync(request);
		response.IsSuccessStatusCode.Should().BeTrue($"Invalid URL: {response.StatusCode}");
		
		string contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty; 
		
		contentType.Should().Be(ExpectedImageContentType);
	}
	[Fact]
	public async Task GetImageFullUrl()
	{
		var getter = await GetGetter();
		string imageFullUrl = await getter.GetImageFullUrl();
		Console.WriteLine($"Testing imageFullUrl: {imageFullUrl}");

		imageFullUrl.Should().NotBeNullOrWhiteSpace();
		imageFullUrl.Length.Should().BeGreaterThan(0);
		imageFullUrl.Should().StartWith("https://static.");
		imageFullUrl.Should().EndWith(".png");
		
		var request = new HttpRequestMessage(HttpMethod.Head, imageFullUrl);
		var response = await _httpClient.SendAsync(request, TestContext.Current.CancellationToken);
		response.IsSuccessStatusCode.Should().BeTrue($"Invalid URL: {response.StatusCode}");
		
		string contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty; 
		
		contentType.Should().Be(ExpectedImageContentType);
	}
	[Fact]
	public async Task GetAudioUrl()
	{
		var getter = await GetGetter();

		string audioUrl = getter.GetAudioUrl();
		Console.WriteLine("Testing audioUrl: " + audioUrl);

		audioUrl.Should().NotBeNullOrWhiteSpace();
		audioUrl.Length.Should().BeGreaterThan(0);
		audioUrl.Should().StartWith("https://static.");
		audioUrl.Should().EndWith(".ogg");
		

		var request = new HttpRequestMessage(HttpMethod.Head, audioUrl);
		var response = await _httpClient.SendAsync(request, TestContext.Current.CancellationToken);
		response.IsSuccessStatusCode.Should().BeTrue($"Invalid URL Status: {response.StatusCode}");
		
		var contentType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;
		Console.WriteLine("Content-Type: " + contentType);
		

		contentType.Should().Be(ExpectedAudioContentType);
	}
}