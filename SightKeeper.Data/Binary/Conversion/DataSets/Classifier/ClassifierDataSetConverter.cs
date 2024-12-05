using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Classifier;

internal sealed class ClassifierDataSetConverter
{
	public ClassifierDataSetConverter(ConversionSession session, FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_tagsConverter = new TagsConverter(session);
		_assetsConverter = new ClassifierAssetsConverter(screenshotsDataAccess, session);
		_weightsConverter = new PlainWeightsConverter(session);
	}

	public PackableClassifierDataSet ConvertDataSet(ClassifierDataSet dataSet) => new()
	{
		Name = dataSet.Name,
		Description = dataSet.Description,
		Tags = _tagsConverter.ConvertTags(dataSet.TagsLibrary.Tags).ToImmutableArray(),
		Assets = _assetsConverter.ConvertAssets(dataSet.AssetsLibrary.Assets).ToImmutableArray(),
		Weights = _weightsConverter.ConvertWeights(dataSet.WeightsLibrary.Weights).ToImmutableArray()
	};

	private readonly TagsConverter _tagsConverter;
	private readonly ClassifierAssetsConverter _assetsConverter;
	private readonly PlainWeightsConverter _weightsConverter;
}