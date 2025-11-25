namespace Scanner;

public interface IScanner<T>
{
	public Task<T> Scan(string nameParam);
	// public Task<T[]> ScanAll();
}