namespace Main.FileHandler.Creator;
using Repository;
using System.Text.Json;
using Utils;
using Scanner.Model;


public class Creator(IRepository<Student> repository) : ICreator
{
	public async Task<string> GenerateJsonData()
	{
		var students = await repository.GetAll();

		const string fileName = "data";
		string finalPath = Path.Join(Constants.DataPath, fileName + ".json");

		string jsonData = JsonSerializer.Serialize<IEnumerable<Student>>(students);
		await File.WriteAllTextAsync(finalPath, jsonData);

		Notifier.MessageTaskCompleted($"data json generated in: {finalPath}");
		return finalPath;
	}

	public async Task<string> GenerateHtmlImagePreview()
	{
		var students = await repository.GetAll();
		string htmlPath = await CreateHtmlImagesPreview(students);

		Notifier.MessageTaskCompleted($"Images preview HTML generated in: {htmlPath}");
		return htmlPath;
	}

	 private static async Task<string> CreateHtmlImagesPreview(IEnumerable<Student> students)
  {
    const string fileName = "imagesPreview";
    IGrouping<string, Student>[] allSchools = students.GroupBy((s) => s.school).ToArray();

    // Generar el contenido HTML
    const string htmlHeader = "<html>\n<head>\n<link rel=\"stylesheet\" href=\"style.css\">\n<title>Students Images Preview</title>\n</head>\n<body>";
    const string stylesCss = @"
			:root {
			    color-scheme: light dark;
			}

			* {
			    box-sizing: border-box;
			}

			body {
			    margin: 0;
			    padding: 0 15px;
			    background-color: rgb(26, 26, 26);
			}

			.schoolTitle {
			    font-size: 2.6;
			    margin: 15px 0;
			    text-align: center;
			}

			.schoolContainer {
			    border-radius: 10px;
			    display: grid;
			    grid-template-columns: repeat(6, 1fr);
			    place-items: center;
			    margin: 15px 0;
			    padding: 10px 20px;
			    background-color: rgba(53, 53, 53, 0.473);
			}

			.studentContainer > h2 {
			    text-align: center;
			    margin: 5px 0;
			}

			.imageContainer {
			    display: flex;
			    flex-direction: row;
			    align-items: center;
			    height: 160px;
			    width: 300px;
			    margin: 5px 0;
			}

			.imageContainer img {
			    height: 100%;
			    width: 100%;
			    object-fit: contain;
			    overflow: hidden;
			}";
    const string htmlFooter = $"\n<style>{stylesCss}\n</style>\n</body>\n</html>";
    List<string> schoolContainer = allSchools.Select(school => string.Join("\n", [$"\n<h2 class=\"schoolTitle\">{school.Key}</h2>\n  <div class=\"schoolContainer\">\n ",string.Join("\n",school.Select((student, index) =>
      $" <div class=\"studentContainer\">\n  <h2>{index + 1}: {student.charaName}</h2>\n  <div class=\"imageContainer\">\n   <img src=\"../media/{student.school}/{student.charaName}.png\" class=\"profileImage\" alt=\"profileImage of {student.charaName}\"></img>\n   <img src=\"../media/{student.school}/{student.charaName}_full.png\" class=\"fullImage\" alt=\"fullImage of {student.charaName}\">\n</div>\n</div>")),"</div>"])).ToList();

    string htmlContent = string.Join("\n", schoolContainer);

    // Concatenar todas las partes del HTML
    string finalHtml = $"{htmlHeader}\n{htmlContent}{htmlFooter}";

    string filePath = Path.Join(Constants.DataPath, fileName + ".html");
    await File.WriteAllTextAsync(filePath, finalHtml);
    return filePath;
  }
}