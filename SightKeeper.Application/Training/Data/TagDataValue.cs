namespace SightKeeper.Application.Training.Data;

public sealed class TagDataValue : TagData
{
	public string Name { get; }

	public TagDataValue(string name)
	{
		Name = name;
	}
}