namespace Main.FileHandler.Verifier;

public interface IDataVerifier
{
	Task<bool> VerifyDataAsync(string filePath);
	Task VerifyData(string filePath);
}