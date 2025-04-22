using System.Reflection;

namespace Scanner.Model;
public record class Student
{
	public required string charaName { get; set; }
	public string name { get; set; } = "no name";
	public string lastName { get; set; } = "no lastname";
	public string school { get; set; } = "no school";
	public string role { get; set; } = "no role";
	public string combatClass { get; set; } = "no combat class";
	public string weaponType { get; set; } = "no wepon type";
	public int? age { get; set; }
	public string? birthday { get; set; }
	public int? height { get; set; }
	public string? hobbies { get; set; }
	public string? designer { get; set; }
	public string? illustrator { get; set; }
	public string voice { get; set; } = "no voice";
	public string releaseDate { get; set; } = "no release date";
	public string skinSet { get; set; } = "default";
	public string pageUrl { get; set; } = "no page url";
	public string pageImageProfileUrl { get; set; } = "no image profile";
	public string pageImageFullUrl { get; set; } = "no image full";
	public string audioUrl { get; set; } = "no audio";

	public DateTime createdAt { get; set; } = DateTime.Now;
	public override string ToString()
	{
		var nl = Environment.NewLine;
		const string separetor = "-----------------------------------------------------------------------------------------------------------------";
		string[] valuesToPrintConstants = ["charaName", "school", "age", "releaseDate", "skinSet", "pageUrl", "pageImageProfileUrl", "pageImageFullUrl", "audioUrl", "createdAt"];

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
	public static readonly string[] Schools = [
		"Abydos",
		"Arius",
		"Gehenna",
		"Highlander",
		"Hyakkiyako",
		"Millennium",
		"Red Winter",
		"SRT",
		"Shanhaijing",
		"Trinity",
		"Valkyrie",
	];
	public static readonly string[] ExcludeSkinSets = [
		"kid"
	];
}