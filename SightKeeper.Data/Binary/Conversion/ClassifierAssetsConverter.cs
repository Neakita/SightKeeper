using System.Collections.Immutable;
using SightKeeper.Data.Binary.DataSets.Classifier;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Conversion;

internal sealed class ClassifierAssetsConverter
{
	public ClassifierAssetsConverter(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public ImmutableArray<SerializableClassifierAsset> Convert(IEnumerable<ClassifierAsset> assets, ConversionSession session)
	{
		return assets.Select(asset => Convert(asset, session)).ToImmutableArray();
	}

	private SerializableClassifierAsset Convert(ClassifierAsset asset, ConversionSession session)
	{
		return new SerializableClassifierAsset(
			_screenshotsDataAccess.GetId(asset.Screenshot),
			asset.Usage,
			session.Tags[asset.Tag]);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
}