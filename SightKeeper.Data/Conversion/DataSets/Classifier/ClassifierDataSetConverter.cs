using System.Collections.Immutable;
using SightKeeper.Data.Model.DataSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion.DataSets.Classifier;

internal sealed class ClassifierDataSetConverter
{
	public ClassifierDataSetConverter(ConversionSession session, ReadIdRepository<Image> imageRepository)
	{
		_tagsConverter = new TagsConverter(session);
		_assetsConverter = new ClassifierAssetsConverter(imageRepository, session);
		_weightsConverter = new WeightsConverter(session);
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
	private readonly WeightsConverter _weightsConverter;
}