using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Tests.Training;

internal sealed class FakeAsset : Asset
{
	public required DomainImage Image { get; init; }
	public AssetUsage Usage { get; set; }
}