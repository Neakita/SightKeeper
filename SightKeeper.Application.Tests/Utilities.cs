using NSubstitute;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests;

internal static class Utilities
{
	public static ImageSetData CreateImageSetData(string name, string description = "")
	{
		var data = Substitute.For<ImageSetData>();
		data.Name.Returns(name);
		data.Description.Returns(description);
		return data;
	}

	public static ExistingImageSetData CreateExistingImageSetData(ImageSet set, string name, string description = "")
	{
		var data = Substitute.For<ExistingImageSetData>();
		data.Set.Returns(set);
		data.Name.Returns(name);
		data.Description.Returns(description);
		return data;
	}

	public static ReadRepository<T> CreateRepository<T>(IReadOnlyCollection<T> items)
	{
		var repository = Substitute.For<ReadRepository<T>>();
		repository.Items.Returns(items);
		return repository;
	}
}