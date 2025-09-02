using SightKeeper.Application.DataSets.Tags;

namespace SightKeeper.Application.Tests.DataSets.Fakes;

internal sealed class FakeNewTagData : NewTagData
{
	public string Name { get; }
	public uint Color { get; }

	public FakeNewTagData(string name)
	{
		Name = name;
		Color = default;
	}
}