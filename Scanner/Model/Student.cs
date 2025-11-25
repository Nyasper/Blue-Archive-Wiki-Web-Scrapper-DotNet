using System.Reflection;

namespace Scanner.Model;
public record class Student
{
	public required string CharaName { get; set; }
	public required string Name { get; set; }
	public required string LastName { get; set; }
	public required string School { get; set; }
	public required int? Age { get; set; }
	public required int? Height { get; set; }
	public required string? Birthday { get; set; }
	public required string? Hobbies { get; set; }
	public required string? Designer { get; set; }
	public required string? Illustrator { get; set; }
	public required string Voice { get; set; }
	public required string ReleaseDate { get; set; }
	public required string SkinSet { get; set; }
	public required string PageUrl { get; set; }
	public required string ImageProfileUrl { get; set; }
	public required string ImageFullUrl { get; set; }
	public required string SmallImageUrl { get; set; }
	public required string AudioUrl { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.Now;
	public override string ToString()
	{
		var nl = Environment.NewLine;
		const string separetor = "-----------------------------------------------------------------------------------------------------------------";
		string[] valuesToPrintConstants = ["CharaName", "School", "Age", "ReleaseDate", "SkinSet", "PageUrl", "ImageProfileUrl", "ImageFullUrl", "AudioUrl", "CreatedAt"];

		string result = nl + separetor;
		PropertyInfo[] properties = GetType().GetProperties();
		var propertiesToPrint = properties.Where(p => valuesToPrintConstants.Contains(p.Name)).ToArray();
		foreach (var property in propertiesToPrint)
		{
			result += $"{nl}{property.Name}: {property.GetValue(this) ?? "null"}";
		}
		result += nl + separetor;
		return result;
	}
}