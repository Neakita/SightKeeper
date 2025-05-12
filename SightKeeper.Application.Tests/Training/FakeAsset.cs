using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.Training;

internal sealed class FakeAsset : Asset
{
	public required Image Image { get; init; }
	public AssetUsage Usage { get; set; }
}