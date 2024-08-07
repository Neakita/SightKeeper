using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class Poser3DAsset : Asset
{
	public static Poser3DAsset Create(Domain.Model.DataSets.Poser3D.Poser3DAsset asset, ConversionSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		return new Poser3DAsset(
			screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<Poser3DItem> Convert(IEnumerable<Domain.Model.DataSets.Poser3D.Poser3DItem> items, ConversionSession session)
	{
		return items.Select(item => Poser3DItem.Create(item, session)).ToImmutableArray();
	}

	public ImmutableArray<Poser3DItem> Items { get; }

	public Poser3DAsset(
		Id screenshotId,
		AssetUsage usage,
		ImmutableArray<Poser3DItem> items)
		: base(screenshotId, usage)
	{
		Items = items;
	}
}