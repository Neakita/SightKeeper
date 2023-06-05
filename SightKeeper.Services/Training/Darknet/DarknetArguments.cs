using SightKeeper.Domain.Model;

namespace SightKeeper.Services.Training.Darknet;

public sealed class DarknetArguments
{
	public ModelType ModelType { get; set; }
	public string Action { get; set; } = "train";
	public string DataPath { get; set; } = string.Empty;
	public string ConfigPath { get; set; } = string.Empty;
	public string? PreTrainedWeightsPath { get; set; } = string.Empty;
	public bool DoNotShow { get; set; } = true;

	public override string ToString() => string.Join(' ', GetArguments());

	private IEnumerable<string> GetArguments()
	{
		yield return ModelType.ToString().ToLower();
		yield return Action;
		yield return DataPath;
		yield return ConfigPath;
		if (PreTrainedWeightsPath != null)
			yield return PreTrainedWeightsPath;
		if (DoNotShow)
			yield return "-dont_show"; 
	}
}