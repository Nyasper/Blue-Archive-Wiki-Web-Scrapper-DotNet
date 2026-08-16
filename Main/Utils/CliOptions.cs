namespace Main.Utils;

public class CliOptions
{
	public bool AutoApprove { get; }

	public CliOptions(bool autoApprove)
	{
		AutoApprove = autoApprove;
	}
}
