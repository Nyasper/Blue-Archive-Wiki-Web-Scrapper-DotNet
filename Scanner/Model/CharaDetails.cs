namespace Scanner.Model;
using static Utils.Constants;

public record class CharaDetails
{
	public string Name
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Name : value;
		}
	} = DefaultCharaDetailsFields.Name;

	public string LastName
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.LastName : value;
		}
	} = DefaultCharaDetailsFields.LastName;
}