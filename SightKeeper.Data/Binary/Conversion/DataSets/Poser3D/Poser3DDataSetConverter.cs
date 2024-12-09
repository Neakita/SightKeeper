using System.Collections.Immutable;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;

internal sealed class Poser3DDataSetConverter
{
	public Poser3DDataSetConverter(ConversionSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_tagsConverter = new PoserTagsConverter(session);
		_assetsConverter = new Poser3DAssetsConverter(session, screenshotsDataAccess);
		_weightsConverter = new PoserWeightsConverter(session);
	}

	public PackablePoser3DDataSet ConvertDataSet(Poser3DDataSet dataSet) => new()
	{
		Name = dataSet.Name,
		Description = dataSet.Description,
		Tags = _tagsConverter.ConvertTags(dataSet.TagsLibrary.Tags).ToImmutableArray(),
		Assets = _assetsConverter.ConvertAssets(dataSet.AssetsLibrary.Assets).ToImmutableArray(),
		Weights = _weightsConverter.ConvertWeights(dataSet.WeightsLibrary.Weights).ToImmutableArray()
	};

	private readonly PoserTagsConverter _tagsConverter;
	private readonly Poser3DAssetsConverter _assetsConverter;
	private readonly PoserWeightsConverter _weightsConverter;
}