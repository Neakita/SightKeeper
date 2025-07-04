using System.Collections.Immutable;
using SightKeeper.Data.Conversion.DataSets.Poser;
using SightKeeper.Data.Model.DataSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Conversion.DataSets.Poser2D;

internal sealed class Poser2DDataSetConverter
{
	public Poser2DDataSetConverter(ConversionSession session, ReadIdRepository<Image> imageRepository)
	{
		_tagsConverter = new PoserTagsConverter(session);
		_assetsConverter = new Poser2DAssetsConverter(session, imageRepository);
		_weightsConverter = new WeightsConverter(session);
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
	private readonly WeightsConverter _weightsConverter;
}