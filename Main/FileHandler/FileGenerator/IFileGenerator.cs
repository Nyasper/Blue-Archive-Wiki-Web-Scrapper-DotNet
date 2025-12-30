namespace Main.FileHandler.FileGenerator;

public interface IFileGenerator
{
	Task<string> GenerateJsonData();
	Task<string> GenerateHtmlImagePreview();
}
