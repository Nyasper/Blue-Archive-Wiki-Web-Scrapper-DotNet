using System;
using System.Data.Common;
using System.Net.Http.Json;
using System.Xml;
using BlueArchiveWebScrapper.model;

namespace BlueArchiveWebScrapper;

public static class Notifier
{
  public static readonly string NL = Environment.NewLine;
  public static void NewBlankMessage(string message)
  {
    Console.Clear();
    Console.WriteLine(message);
  }
  public static void NormalMessage(string message)
  {
    Console.WriteLine(NL+message);
  }
  public static void MessageInitiatingTask(string message)
  {
    Console.WriteLine(NL+message+"... 🚀");
  }
  public static void MessageTaskCompleted(string message)
  {
    Console.WriteLine(NL+message+" ✔️");
  }
  public static void MessageNothingToDo(string message)
  {
    Console.WriteLine(message+" ✅");
  }
  public static void MessageTaskCancelled(string message)
  {
    Console.WriteLine(NL+message+" ❌");
  }
  public static void MessageFileMissing(string type, FileHandler.FileVerification file)
  {
    string genericMessage = $"\"{file.CharaName}\" from school \"{file.School}\" doesn't have";
    Console.WriteLine($"{genericMessage} {type}");
  }
  
  
  public static void LogStudentsList(string message,Student[] students)
  {
    NormalMessage(message);
    for (int i = 0; i < students.Length; i++)
    {
      Console.WriteLine($"{i+1}: {students[i].charaName} 💙");      
    }
  }
  public static void LogStudentsList(string message,CharaListInfo[] students)
  {
    NormalMessage(message+" 💚"+NL);
    for (int i = 0; i < students.Length; i++)
    {
      Console.WriteLine($"{i+1}: {students[i].name} 💙");      
    }
  }

}
