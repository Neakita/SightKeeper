using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.Tests;

internal static class Utilities
{
	public static Image CreateImage()
	{
		ImageSet set = new();
		return set.CreateImage();
	}

	public static TagsLibrary<Tag> CreateTagsLibrary()
	{
		return new ClassifierDataSet().TagsLibrary;
	}
}