using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal abstract class DataSetConverter<TPackableDataSet>
	where TPackableDataSet : PackableDataSet, new()
{
	public virtual TPackableDataSet Convert(DataSet dataSet)
	{
		Guard.IsNotNull(Session.GameIds);
		ushort? gameId = dataSet.Game == null ? null : Session.GameIds[dataSet.Game];
		PackableComposition? composition = ConvertComposition(dataSet.Composition);
		ImmutableArray<PackableScreenshot> screenshots = ConvertScreenshots(dataSet.ScreenshotsLibrary);
		return new TPackableDataSet
		{
			Name = dataSet.Name,
			Description = dataSet.Description,
			GameId = gameId,
			Composition = composition,
			MaxScreenshotsWithoutAsset = dataSet.ScreenshotsLibrary.MaxQuantity,
			Screenshots = screenshots
		};
	}

	protected ConversionSession Session { get; }

	protected DataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session)
	{
		ScreenshotsDataAccess = screenshotsDataAccess;
		Session = session;
	}

	protected FileSystemScreenshotsDataAccess ScreenshotsDataAccess { get; }

	protected static PackableTag ConvertPlainTag(byte id, Tag tag)
	{
		return new PackableTag(id, tag.Name, tag.Color);
	}

	protected ImmutableArray<PackableTag> ConvertPlainTags(IReadOnlyCollection<Tag> tags)
	{
		var builder = ImmutableArray.CreateBuilder<PackableTag>(tags.Count);
		byte index = 0;
		foreach (var tag in tags)
		{
			Session.TagsIds.Add(tag, index);
			builder.Add(ConvertPlainTag(index, tag));
			index++;
		}
		return builder.DrainToImmutable();
	}

	protected ImmutableArray<PackablePlainWeights> ConvertPlainWeights(IReadOnlyCollection<Weights> weights)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePlainWeights>(weights.Count);
		foreach (var item in weights.Cast<PlainWeights>())
		{
			var id = Session.WeightsIdCounter++;
			resultBuilder.Add(ConvertWeights(id, item, ConvertWeightsTags(item.Tags)));
			Session.WeightsIds.Add(item, id);
		}
		return resultBuilder.DrainToImmutable();
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags
			.Select(tag => Session.TagsIds[tag])
			.ToImmutableArray();
	}

	private static PackablePlainWeights ConvertWeights(ushort id, Weights item, ImmutableArray<byte> tagIds) =>
		new(id, item.CreationDate, item.ModelSize, item.Metrics, item.Resolution, tagIds);

	private static PackableTransparentComposition? ConvertComposition(Composition? composition)
	{
		return composition switch
		{
			null => null,
			TransparentComposition transparentComposition => new PackableTransparentComposition(
				transparentComposition.MaximumScreenshotsDelay,
				transparentComposition.Opacities),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private ImmutableArray<PackableScreenshot> ConvertScreenshots(ScreenshotsLibrary library)
	{
		return library.Screenshots.Select(ConvertScreenshot).ToImmutableArray();
		PackableScreenshot ConvertScreenshot(Screenshot screenshot) => new(
			ScreenshotsDataAccess.GetId(screenshot),
			screenshot.CreationDate,
			screenshot.Resolution);
	}
}

internal abstract class DataSetConverter<TTag, TAsset, TWeights, TDataSet> : DataSetConverter<TDataSet>
	where TTag : PackableTag
	where TAsset : PackableAsset
	where TWeights : PackableWeights
	where TDataSet : PackableDataSet<TTag, TAsset, TWeights>, new()
{
	public sealed override TDataSet Convert(DataSet dataSet)
	{
		var packable = base.Convert(dataSet);
		packable.Tags = ConvertTags(dataSet.TagsLibrary.Tags);
		packable.Assets = ConvertAssets(dataSet.AssetsLibrary.Assets);
		packable.Weights = ConvertWeights(dataSet.WeightsLibrary.Weights);
		return packable;
	}

	protected DataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(screenshotsDataAccess, session)
	{
	}

	protected abstract ImmutableArray<TTag> ConvertTags(IReadOnlyCollection<Tag> tags);
	protected abstract ImmutableArray<TAsset> ConvertAssets(IReadOnlyCollection<Asset> assets);
	protected abstract ImmutableArray<TWeights> ConvertWeights(IReadOnlyCollection<Weights> weights);
}