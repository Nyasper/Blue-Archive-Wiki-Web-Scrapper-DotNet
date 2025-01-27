using System.Reflection;

namespace Scanner.Model;
public record class Student
{
  public required string charaName { get; init; }
  public string name { get; init; } = "no name";
  public string lastName { get; init; } = "no lastname";
  public string school { get; init; } = "no school";
  public string role { get; init; } = "no role";
  public string combatClass { get; init; } = "no combat class";
  public string weaponType { get; init; } = "no wepon type";
  public int? age { get; init; }
  public string? birthday { get; init; }
  public int? height { get; init; }
  public string? hobbies { get; init; }
  public string? designer { get; init; }
  public string? illustrator { get; init; }
  public string voice { get; init; } = "no voice";
  public string releaseDate { get; init; } = "no release date";
  public string skinSet { get; init; } = "default";
  public string pageUrl { get; init; } = "no page url";
  public string pageImageProfileUrl { get; init; } = "no image profile";
  public string pageImageFullUrl { get; init; } = "no image full";
  public string audioUrl { get; init; } = "no audio";

  public DateTime createdAt { get; init; } = DateTime.Now;
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
