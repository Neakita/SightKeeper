namespace SightKeeper.Domain.Model;

public sealed class ModelConfig
{
	public string Name { get; set; }
	public string Content { get; set; }
	public ModelType ModelType { get; set; }

	public ModelConfig(string name, string content, ModelType modelType)
	{
		Name = name;
		Content = content;
		ModelType = modelType;
	}
}