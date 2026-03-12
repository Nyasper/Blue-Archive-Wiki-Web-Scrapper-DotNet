using System.Threading.Tasks;

namespace Main.FileHandler.FileGenerator;

public interface IFileGenerator<in T>
{
	Task<string> GenerateJsonData(T[] entity);
	Task<string> GenerateHtmlDataPreview(T[] entity);
}
