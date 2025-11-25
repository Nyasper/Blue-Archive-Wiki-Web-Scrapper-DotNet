namespace Scanner.CharaDetails;

public interface IDetailsGetter
{
	(string name, string lastName) GetFullName();
	int? GetAge();
	string? GetBirthday();
	int? GetHeight();
	string GetHobbies();
	string? GetDesigner();
	string? GetIllustrator();
	string GetVoice();
	string GetImageProfileUrl();
	Task<string> GetImageFullUrl();
	string GetAudioUrl();
}