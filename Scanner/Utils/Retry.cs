using System;
using System.Threading.Tasks;

namespace Scanner.Utils;

public static class Retry
{
	public const int MaxAttempts = 3;

	public static async Task<T> WithRetryAsync<T>(Func<Task<T>> action, string context, TimeSpan initialDelay)
	{
		for (int attempt = 1; ; attempt++)
		{
			try
			{
				return await action();
			}
			catch (Exception ex) when (attempt < MaxAttempts)
			{
				Console.WriteLine($"Retry {attempt}/{MaxAttempts} for {context}: {ex.Message}");
				await Task.Delay(initialDelay * attempt);
			}
		}
	}

	public static async Task WithRetryAsync(Func<Task> action, string context, TimeSpan initialDelay)
	{
		await WithRetryAsync(async () =>
		{
			await action();
			return 0;
		}, context, initialDelay);
	}
}
