using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeNewDataSetData : NewDataSetData
{
	public string Name { get; }
	public string Description { get; }
	public DataSetType Type { get; }
	public IEnumerable<NewTagData> NewTags { get; }

	public FakeNewDataSetData(string name, string description)
	{
		Name = name;
		Description = description;
		NewTags = Enumerable.Empty<NewTagData>();
	}

	public FakeNewDataSetData()
	{
		Name = string.Empty;
		Description = string.Empty;
		NewTags = Enumerable.Empty<NewTagData>();
	}

	public FakeNewDataSetData(DataSetType type)
	{
		Name = string.Empty;
		Description = string.Empty;
		Type = type;
		NewTags = Enumerable.Empty<NewTagData>();
	}

	public FakeNewDataSetData(string name)
	{
		Name = name;
		Description = string.Empty;
		NewTags = Enumerable.Empty<NewTagData>();
	}
}