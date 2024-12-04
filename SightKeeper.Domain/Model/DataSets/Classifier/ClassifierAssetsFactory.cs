using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

internal sealed class ClassifierAssetsFactory : AssetsFactory<ClassifierAsset>
{
	public ClassifierAssetsFactory(TagsLibrary<Tag> tagsLibrary)
	{
		_tagsLibrary = tagsLibrary;
	}

	public override ClassifierAsset CreateAsset()
	{
		return new ClassifierAsset(_tagsLibrary.Tags.First());
	}

	private readonly TagsLibrary<Tag> _tagsLibrary;
}