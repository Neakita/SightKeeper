using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class SerializablePoser3DAsset : SerializableAsset
{
	public static SerializablePoser3DAsset Create(Poser3DAsset asset, ConversionSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		return new SerializablePoser3DAsset(
			screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			Convert(asset.Items, session));
	}

	private static ImmutableArray<SerializablePoser3DItem> Convert(IEnumerable<Poser3DItem> items, ConversionSession session)
	{
		return items.Select(item => SerializablePoser3DItem.Create(item, session)).ToImmutableArray();
	}

	public ImmutableArray<SerializablePoser3DItem> Items { get; }

	public SerializablePoser3DAsset(
		Id screenshotId,
		AssetUsage usage,
		ImmutableArray<SerializablePoser3DItem> items)
		: base(screenshotId, usage)
	{
		Items = items;
	}
}