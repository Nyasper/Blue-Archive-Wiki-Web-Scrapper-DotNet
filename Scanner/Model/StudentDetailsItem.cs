namespace Scanner.Model;
using static Utils.Constants;

public record class StudentDetailsItem
{
	public required string Name
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Name : value;
		}
	} = DefaultCharaDetailsFields.Name;

	public required string LastName
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.LastName : value;
		}
	} = DefaultCharaDetailsFields.LastName;
	public required int? Age
	{
		get;
		set
		{
			field = value  ?? DefaultCharaDetailsFields.Age;
		}
	} = DefaultCharaDetailsFields.Age;
	public required int? Height
	{
		get;
		set
		{
			field = value ??  DefaultCharaDetailsFields.Height;

		}
	} = DefaultCharaDetailsFields.Height;
	public required string? Birthday
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Birthday : value;
		}
	} = DefaultCharaDetailsFields.Birthday;
	public required string Hobbies
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Hobbies : value;
		}
	} = DefaultCharaDetailsFields.Hobbies;
	public required string? Designer
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Designer : value;
		}
	} = DefaultCharaDetailsFields.Designer;
	public required string? Illustrator
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Illustrator : value;
		}
	} = DefaultCharaDetailsFields.Illustrator;
	public required string Voice
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.Voice : value;
		}
	} = DefaultCharaDetailsFields.Voice;
	public required string ImageProfileUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.ImageProfileUrl : value;
		}
	} = DefaultCharaDetailsFields.ImageProfileUrl;

	public required string ImageFullUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.ImageFullUrl : value;
		}
	} = DefaultCharaDetailsFields.ImageFullUrl;
	public required string AudioUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaDetailsFields.AudioUrl : value;
		}
	} = DefaultCharaDetailsFields.AudioUrl;
}