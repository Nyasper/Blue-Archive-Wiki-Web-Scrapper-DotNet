namespace Scanner.CharaDetails;

public interface IGetter
{
	string GetCharaName();
	string GetName();
	string GetLastName();
	string GetSchool();
	string GetRole();
	string GetCombatClass();
	string GetWeaponType();
	int? GetAge();
	string? GetBirthday();
	int? GetHeight();
	string GetHobbies();
	string? GetDesigner();
	string? GetIllustrator();
	string GetVoice();
	string GetReleaseDate();
	string GetSkinSet();
	string GetPageUrl();
	string GetPageImageProfileUrl();
	Task<string> GetPageImageFullUrl();
	string GetAudioUrl();
}