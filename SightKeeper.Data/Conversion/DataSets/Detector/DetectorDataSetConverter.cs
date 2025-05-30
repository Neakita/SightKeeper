using System.Collections.Immutable;
using SightKeeper.Data.Model.DataSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion.DataSets.Detector;

internal sealed class DetectorDataSetConverter
{
	public DetectorDataSetConverter(ConversionSession session, ReadIdRepository<Image> imageRepository)
	{
		_tagsConverter = new TagsConverter(session);
		_assetsConverter = new DetectorAssetsConverter(imageRepository, session);
		_weightsConverter = new WeightsConverter(session);
	}

	public PackableDetectorDataSet ConvertDataSet(DetectorDataSet dataSet) => new()
	{
		Name = dataSet.Name,
		Description = dataSet.Description,
		Tags = _tagsConverter.ConvertTags(dataSet.TagsLibrary.Tags).ToImmutableArray(),
		Assets = _assetsConverter.ConvertAssets(dataSet.AssetsLibrary.Assets).ToImmutableArray(),
		Weights = _weightsConverter.ConvertWeights(dataSet.WeightsLibrary.Weights).ToImmutableArray()
	};

	private readonly TagsConverter _tagsConverter;
	private readonly DetectorAssetsConverter _assetsConverter;
	private readonly WeightsConverter _weightsConverter;
}