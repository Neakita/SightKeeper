using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal abstract class DataSetConverter<TPackableDataSet>
	where TPackableDataSet : PackableDataSet, new()
{
	protected DataSetConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		ScreenshotsDataAccess = screenshotsDataAccess;
	}

	public virtual TPackableDataSet Convert(
		DataSet dataSet,
		ConversionSession session)
	{
		Guard.IsNotNull(session.GameIds);
		ushort? gameId = dataSet.Game == null ? null : session.GameIds[dataSet.Game];
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

	protected static PackableTag ConvertPlainTag(byte id, Tag tag)
	{
		return new PackableTag(id, tag.Name, tag.Color);
	}

	protected virtual ImmutableArray<PackableTag> ConvertPlainTags(
		IReadOnlyCollection<Tag> tags,
		ConversionSession session)
	{
		var builder = ImmutableArray.CreateBuilder<PackableTag>(tags.Count);
		byte index = 0;
		foreach (var tag in tags)
		{
			session.TagsIds.Add(tag, index);
			builder.Add(ConvertPlainTag(index, tag));
			index++;
		}
		return builder.DrainToImmutable();
	}

	protected ImmutableArray<PackablePlainWeights> ConvertPlainWeights(
		IReadOnlyCollection<Weights> weights,
		ConversionSession session)
	{
		var resultBuilder = ImmutableArray.CreateBuilder<PackablePlainWeights>(weights.Count);
		foreach (var item in weights.Cast<PlainWeights>())
		{
			var id = session.WeightsIdCounter++;
			resultBuilder.Add(ConvertWeights(id, item, ConvertWeightsTags(item.Tags)));
			session.WeightsIds.Add(item, id);
		}
		return resultBuilder.DrainToImmutable();
		ImmutableArray<byte> ConvertWeightsTags(IEnumerable<Tag> tags) => tags
			.Select(tag => session.TagsIds[tag])
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