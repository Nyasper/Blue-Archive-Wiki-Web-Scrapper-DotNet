namespace Scanner.Model;
using static Scanner.Utils.Constants;

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

	public string School
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.CharaName : value;
		}
	} = DefaultCharaListStudentFields.School;

	public string ReleaseDate
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.CharaName : value;
		}
	} = DefaultCharaListStudentFields.ReleaseDate;

	public string SkinSet
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.CharaName : value;
		}
	} = DefaultCharaListStudentFields.SkinSet;

	public string SmallImgUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.SmallImgUrl : value;
		}
	} = DefaultCharaListStudentFields.SmallImgUrl;
	public string PageUrl
	{
		get;
		set
		{
			field = string.IsNullOrEmpty(value) ? DefaultCharaListStudentFields.PageUrl : value;
		}
	} = DefaultCharaListStudentFields.PageUrl;
};