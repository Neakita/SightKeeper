using System.Collections.Immutable;
using SightKeeper.Data.Binary.Services;
using ClassifierAsset = SightKeeper.Data.Binary.DataSets.Classifier.ClassifierAsset;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Classifier;

internal sealed class ClassifierAssetsConverter
{
	public ClassifierAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public ImmutableArray<ClassifierAsset> Convert(IEnumerable<Domain.Model.DataSets.Classifier.ClassifierAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private ClassifierAsset Convert(Domain.Model.DataSets.Classifier.ClassifierAsset asset, ConversionSession session)
	{
		return new ClassifierAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			session.Tags[asset.Tag]);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
}