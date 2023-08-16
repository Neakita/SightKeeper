namespace SightKeeper.Domain.Model;

public sealed class ModelConfig
{
	public string Name { get; set; }
	public byte[] Content { get; set; }
	public ModelType ModelType { get; set; }

	public ModelConfig(string name, byte[] content, ModelType modelType)
	{
		Name = name;
		Content = content;
		ModelType = modelType;
	}

	public override string ToString() => Name;
}