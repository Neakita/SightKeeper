using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Classifier;

internal sealed class ClassifierAssetsFactory : AssetsFactory<ClassifierAsset>
{
	public ClassifierAssetsFactory(TagsLibrary<Tag> tagsLibrary)
	{
		_tagsLibrary = tagsLibrary;
	}

	public ClassifierAsset CreateAsset(Image image)
	{
		return new ClassifierAsset(_tagsLibrary.Tags[0])
		{
			Image = image
		};
	}

	private readonly TagsLibrary<Tag> _tagsLibrary;
}