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
  public void LogStudent()
  {
    var nl = Environment.NewLine;
    const string separetor = "-----------------------------------------------------------------------------------------------------------------";
    string[] valuesToPrint = ["charaName", "school", "age", "releaseDate", "skinSet", "pageUrl", "pageImageProfileUrl", "pageImageFullUrl", "audioUrl", "createdAt"];

    Console.WriteLine(nl + separetor);
    foreach (var propertyName in valuesToPrint)
    {
      var propertyInfo = this.GetType().GetProperty(propertyName);
      if (propertyInfo == null) continue;
      var propertyValue = propertyInfo.GetValue(this);
      Console.WriteLine($"{propertyName}: {propertyValue ?? "null"}");
    }
    Console.WriteLine(separetor);
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
