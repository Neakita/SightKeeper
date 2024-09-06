using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Conversion;

internal abstract class PoserDataSetConverter<TTag, TKeyPointTag, TAsset> : DataSetConverter<DataSet<TTag, TKeyPointTag, TAsset>>
	where TTag : PoserTag, TagsFactory<TTag>
	where TKeyPointTag : KeyPointTag<TTag>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	protected PoserDataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected static ImmutableArray<PackableTag> ConvertKeyPointTags(IEnumerable<KeyPointTag> keyPointTags)
	{
		return keyPointTags
			.Select((tag, index) => ConvertKeyPointTag((byte)index, tag))
			.ToImmutableArray();
	}

	protected static ImmutableArray<PackableNumericItemProperty> ConvertNumericItemProperties(
		IEnumerable<NumericItemProperty> properties) =>
		properties.Select(ConvertNumericItemProperty).ToImmutableArray();

	private static PackableTag ConvertKeyPointTag(byte id, KeyPointTag tag)
	{
		return new PackableTag(id, tag.Name, tag.Color);
	}

	private static PackableNumericItemProperty ConvertNumericItemProperty(NumericItemProperty property) =>
		new(property.Name, property.MinimumValue, property.MaximumValue);
}