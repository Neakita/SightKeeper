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

	protected FileSystemScreenshotsDataAccess ScreenshotsDataAccess { get; }
	protected ConversionSession Session { get; }

	protected DataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session)
	{
		ScreenshotsDataAccess = screenshotsDataAccess;
		Session = session;
	}

	protected static PackableTag ConvertPlainTag(byte id, Tag tag)
	{
		return new PackableTag(id, tag.Name, tag.Color);
	}

	private static PackableComposition? ConvertComposition(Composition? composition) => composition switch
	{
		null => null,
		FixedTransparentComposition fixedTransparent => new PackableFixedTransparentComposition(
			fixedTransparent.MaximumScreenshotsDelay,
			fixedTransparent.Opacities),
		FloatingTransparentComposition floatingTransparent => new PackableFloatingTransparentComposition(
			floatingTransparent.MaximumScreenshotsDelay,
			floatingTransparent.SeriesDuration,
			floatingTransparent.PrimaryOpacity,
			floatingTransparent.MinimumOpacity),
		_ => throw new ArgumentOutOfRangeException()
	};

	private ImmutableArray<PackableScreenshot> ConvertScreenshots(ScreenshotsLibrary library)
	{
		return library.Screenshots.Select(ConvertScreenshot).ToImmutableArray();

		PackableScreenshot ConvertScreenshot(Screenshot screenshot) => new(
			ScreenshotsDataAccess.GetId(screenshot),
			screenshot.CreationDate,
			screenshot.Resolution);
	}
}

internal abstract class DataSetConverter<TTag, TAsset, TDataSet> : DataSetConverter<TDataSet>
	where TTag : PackableTag
	where TAsset : PackableAsset
	where TDataSet : PackableDataSet<TTag, TAsset>, new()
{
	public sealed override TDataSet Convert(DataSet dataSet)
	{
		var packable = base.Convert(dataSet);
		packable.Tags = ConvertTags(dataSet.TagsLibrary.Tags);
		packable.Assets = ConvertAssets(dataSet.AssetsLibrary.Assets);
		// ConvertWeights can be used in DataSetConverter<TDataSet>
		// except that its version of Convert method can't call ConvertTags to process tags ids in time
		packable.Weights = ConvertWeights(dataSet.WeightsLibrary.Weights);
		return packable;
	}

	protected DataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess, ConversionSession session) : base(
		screenshotsDataAccess, session)
	{
	}

	protected abstract ImmutableArray<PackableWeights> ConvertWeights(IReadOnlyCollection<Weights> weights);

	protected abstract ImmutableArray<TTag> ConvertTags(IReadOnlyCollection<Tag> tags);
	protected abstract ImmutableArray<TAsset> ConvertAssets(IReadOnlyCollection<Asset> assets);
}