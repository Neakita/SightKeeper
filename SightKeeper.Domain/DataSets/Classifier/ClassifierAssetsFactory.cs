using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Classifier;

internal sealed class ClassifierAssetsFactory : AssetsFactory<ClassifierAsset>
{
	public ClassifierAssetsFactory(TagsLibrary<Tag> tagsLibrary)
	{
		_tagsLibrary = tagsLibrary;
	}

	public ClassifierAsset CreateAsset()
	{
		return new ClassifierAsset(_tagsLibrary.Tags[0]);
	}

	private readonly TagsLibrary<Tag> _tagsLibrary;
}