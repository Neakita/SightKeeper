using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal sealed class Poser2DDataSetConverter
{
	public Poser2DDataSetConverter(ConversionSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_tagsConverter = new PoserTagsConverter(session);
		_assetsConverter = new Poser2DAssetsConverter(session, screenshotsDataAccess);
		_weightsConverter = new PoserWeightsConverter(session);
	}

	public PackablePoser2DDataSet ConvertDataSet(Poser2DDataSet dataSet) => new()
	{
		Name = dataSet.Name,
		Description = dataSet.Description,
		Tags = _tagsConverter.ConvertTags(dataSet.TagsLibrary.Tags).ToImmutableArray(),
		Assets = _assetsConverter.ConvertAssets(dataSet.AssetsLibrary.Assets).ToImmutableArray(),
		Weights = _weightsConverter.ConvertWeights(dataSet.WeightsLibrary.Weights).ToImmutableArray()
	};

	private readonly PoserTagsConverter _tagsConverter;
	private readonly Poser2DAssetsConverter _assetsConverter;
	private readonly PoserWeightsConverter _weightsConverter;
}