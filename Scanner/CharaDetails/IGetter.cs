namespace Scanner.CharaDetails;

public interface IGetter
{
	string GetName();
	string GetLastName();
	int? GetAge();
	string? GetBirthday();
	int? GetHeight();
	string GetHobbies();
	string? GetDesigner();
	string? GetIllustrator();
	string GetVoice();
	string GetPageUrl();
	string GetPageImageProfileUrl();
	Task<string> GetPageImageFullUrl();
	string GetAudioUrl();
}