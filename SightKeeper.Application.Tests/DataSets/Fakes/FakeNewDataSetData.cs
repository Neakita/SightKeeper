using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeNewDataSetData : NewDataSetData
{
	public static FakeNewDataSetData CreateWithTags(params IEnumerable<string> tagsNames)
	{
		var tags = tagsNames.Select(name => new FakeNewTagData(name));
		return new FakeNewDataSetData(tags);
	}

	public static FakeNewDataSetData CreateWithKeyPointTags(params IEnumerable<string> keyPointTagsNames)
	{
		var keyPointTags = keyPointTagsNames.Select(name => new FakeNewTagData(name));
		FakeNewPoserTagData poserTag = new(keyPointTags);
		return new FakeNewDataSetData([poserTag]);
	}

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

	private FakeNewDataSetData(IEnumerable<NewTagData> newTags)
	{
		Name = string.Empty;
		Description = string.Empty;
		NewTags = newTags;
	}

	private FakeNewDataSetData(IEnumerable<NewPoserTagData> newTags)
	{
		Name = string.Empty;
		Description = string.Empty;
		Type = DataSetType.Poser2D;
		NewTags = newTags;
	}
}