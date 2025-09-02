using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeNewPoserTagData : NewPoserTagData
{
	public string Name { get; }
	public uint Color { get; }
	public IEnumerable<NewTagData> KeyPointTags { get; }

	public FakeNewPoserTagData(string name, IEnumerable<NewTagData> keyPointTags)
	{
		Name = name;
		Color = default;
		KeyPointTags = keyPointTags;
	}

	public FakeNewPoserTagData(IEnumerable<NewTagData> keyPointTags)
	{
		Name = string.Empty;
		Color = default;
		KeyPointTags = keyPointTags;
	}
}