namespace Scanner.Model;
using static Utils.Constants;

public record class StudentDetailsItem
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
	public int? Age
	{
		get;
		set
		{
			field = value  ?? DefaultCharaDetailsFields.Age;
		}
	} = DefaultCharaDetailsFields.Age;
	public int? Height
	{
		get;
		set
		{
			field = value ??  DefaultCharaDetailsFields.Height;

		}
	} = DefaultCharaDetailsFields.Height;
	public string? Birthday
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Birthday : value;
		}
	} = DefaultCharaDetailsFields.Birthday;
	public string Hobbies
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Hobbies : value;
		}
	} = DefaultCharaDetailsFields.Hobbies;
	public string Designer
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Designer : value;
		}
	} = DefaultCharaDetailsFields.Designer;
	public string Illustrator
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Illustrator : value;
		}
	} = DefaultCharaDetailsFields.Illustrator;
	public string Voice
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Voice : value;
		}
	} = DefaultCharaDetailsFields.Voice;
	public string ImageProfileUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.ImageProfileUrl : value;
		}
	} = DefaultCharaDetailsFields.ImageProfileUrl;

	public string ImageFullUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.ImageFullUrl : value;
		}
	} = DefaultCharaDetailsFields.ImageFullUrl;
	public string AudioUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.AudioUrl : value;
		}
	} = DefaultCharaDetailsFields.AudioUrl;
}