using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public sealed class ModelConfig : Entity
{
	public int Id { get; private set; }
	public string Name { get; private set; }
	public string Content { get; private set; }

	public ModelConfig(string name, string content)
	{
		Name = name;
		Content = content;
	}
}