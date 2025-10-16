using NSubstitute;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests.Images;

public sealed class ImageSetPersistenceTests
{
	[Fact]
	public void ShouldPersistName()
	{
		const string name = "The name";
		var set = Substitute.For<ImageSet>();
		set.Name.Returns(name);
		var persistedSet = Persist(set);
		persistedSet.Received().Name = name;
	}

	[Fact]
	public void ShouldPersistDescription()
	{
		const string description = "The description";
		var set = Substitute.For<ImageSet>();
		set.Description.Returns(description);
		var persistedSet = Persist(set);
		persistedSet.Received().Description = description;
	}

	private readonly ImageSetSerializer _serializer = new(new SubstituteSerializer<IReadOnlyCollection<ManagedImage>>());

	private static ImageSetDeserializer Deserializer
	{
		get
		{
			var imageSetFactory = Substitute.For<ImageSetFactory<ImageSet>>();
			imageSetFactory.CreateImageSet().Returns(_ => Substitute.For<ImageSet, SettableInitialItems<ManagedImage>>());
			var imagesDeserializer = new SubstituteDeserializer<IReadOnlyCollection<ManagedImage>>(() => []);
			var imageLookupperPopulator = Substitute.For<ImageLookupperPopulator>();
			var imageSetWrapper = Substitute.For<ImageSetWrapper>();
			imageSetWrapper.Wrap(Arg.Any<ImageSet>()).Returns(call => call.Arg<ImageSet>());
			return new ImageSetDeserializer(imageSetFactory, imagesDeserializer, imageLookupperPopulator, imageSetWrapper);
		}
	}

	private ImageSet Persist(ImageSet set)
	{
		var bytes = _serializer.Serialize(set);
		return Deserializer.Deserialize(bytes);
	}
}