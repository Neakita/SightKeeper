namespace SightKeeper.Application.DataSets.Tags;

public interface NewTagData : TagData
{
	string Name { get; }
	uint Color { get; }
}