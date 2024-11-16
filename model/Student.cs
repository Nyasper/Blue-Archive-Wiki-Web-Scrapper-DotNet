using System.Xml;

namespace BlueArchiveWebScrapper.model;

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
  public void LogStudent()
  {
    var nl = Environment.NewLine;
    const string Separetor = "-----------------------------------------------------------------------------------------------------------------";
    string[] valuesToPrint = ["charaName", "school", "age", "releaseDate", "skinSet", "pageUrl", "pageImageProfileUrl", "pageImageFullUrl", "audioUrl", "createdAt"];

    Console.WriteLine(nl + Separetor);
    foreach (var PropertyName in valuesToPrint)
    {
      var propertyInfo = this.GetType().GetProperty(PropertyName);
      if (propertyInfo == null) continue;
      var PropertyValue = propertyInfo.GetValue(this);
      Console.WriteLine($"{PropertyName}: {PropertyValue ?? "null"}");
    }
    Console.WriteLine(Separetor);
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
  public static readonly string[] NoSkinSet = [
    "kid"
  ];
}
