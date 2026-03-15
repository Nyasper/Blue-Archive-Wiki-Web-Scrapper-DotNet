using System.Reflection;

namespace Scanner.Model;
public record class Student
{
	public required string CharaName { get; init; }
	public required string Name { get; init; }
	public required string LastName { get; init; }
	public required string School { get; init; }
	public required int? Age { get; init; }
	public required int? Height { get; init; }
	public required string? Birthday { get; init; }
	public required string? Hobbies { get; init; }
	public required string? Designer { get; init; }
	public required string? Illustrator { get; init; }
	public required string Voice { get; init; }
	public required string ReleaseDate { get; init; }
	public required string SkinSet { get; init; }
	public required string PageUrl { get; init; }
	public required string ImageProfileUrl { get; init; }
	public required string ImageFullUrl { get; init; }
	public required string SmallImageUrl { get; init; }
	public required string AudioUrl { get; init; }

	public DateTime CreatedAt { get; init; } = DateTime.Now;
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