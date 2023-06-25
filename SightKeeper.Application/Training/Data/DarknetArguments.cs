using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training.Data;

public sealed class DarknetArguments
{
	public ModelType ModelType { get; set; }
	public string Action { get; set; } = "train";
	public string DataPath { get; set; } = string.Empty;
	public string ConfigPath { get; set; } = string.Empty;
	public string? BaseWeightsPath { get; set; } = string.Empty;
	public bool DoNotShow { get; set; } = true;

	public override string ToString() => string.Join(' ', GetArguments());

	private IEnumerable<string> GetArguments()
	{
		yield return ModelType.ToString().ToLower();
		yield return Action;
		yield return DataPath;
		yield return ConfigPath;
		if (BaseWeightsPath != null)
			yield return BaseWeightsPath;
		if (DoNotShow)
			yield return "-dont_show"; 
	}
}