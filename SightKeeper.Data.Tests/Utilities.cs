using CommunityToolkit.Diagnostics;
using MemoryPack;
using NSubstitute;
using SightKeeper.Data.Model.Images;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	public static ImageSet CreateImageSet()
	{
		var factory = new StorableImageSetFactory(Substitute.For<ChangeListener>(), new Lock());
		return factory.CreateImageSet();
	}

	public static ImageSet Persist(this ImageSet set)
	{
		PersistenceBootstrapper.Setup(Substitute.For<ChangeListener>(), new Lock());
		var serialized = MemoryPackSerializer.Serialize(set);
		var imageSet = MemoryPackSerializer.Deserialize<ImageSet>(serialized);
		Guard.IsNotNull(imageSet);
		return imageSet;
	}
}