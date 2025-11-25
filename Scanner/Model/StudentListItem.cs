using static Scanner.Utils.Constants;
namespace Scanner.Model;

public record class StudentListItem
{
	public required string CharaName
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.CharaName : value;
		}
	} = DefaultCharaListStudentFields.CharaName;

	public required string School
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.CharaName : value;
		}
	} = DefaultCharaListStudentFields.School;

	public required string ReleaseDate
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.CharaName : value;
		}
	} = DefaultCharaListStudentFields.ReleaseDate;

	public required string SkinSet
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.CharaName : value;
		}
	} = DefaultCharaListStudentFields.SkinSet;

	public required string SmallImgUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.SmallImgUrl : value;
		}
	} = DefaultCharaListStudentFields.SmallImgUrl;
	public required string PageUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.PageUrl : value;
		}
	} = DefaultCharaListStudentFields.PageUrl;
};