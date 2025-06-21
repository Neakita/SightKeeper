using CommunityToolkit.Diagnostics;
using MemoryPack;
using NSubstitute;
using SightKeeper.Data.Images;
using SightKeeper.Data.Model.DataSets;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	public static ImageSet CreateImageSet()
	{
		var factory = new StorableImageSetFactory(Substitute.For<ChangeListener>(), new Lock());
		return factory.CreateImageSet();
	}

	public static ClassifierDataSet CreateClassifierDataSet()
	{
		var factory = new StorableClassifierDataSetFactory(Substitute.For<ChangeListener>(), new Lock());
		return factory.CreateDataSet();
	}

	public static ImageSet Persist(this ImageSet set)
	{
		PersistenceBootstrapper.Setup(Substitute.For<ChangeListener>(), new Lock());
		var serialized = MemoryPackSerializer.Serialize(set);
		var persistedSet = MemoryPackSerializer.Deserialize<ImageSet>(serialized);
		Guard.IsNotNull(persistedSet);
		return persistedSet;
	}

	public static DataSet Persist(this DataSet set)
	{
		PersistenceBootstrapper.Setup(Substitute.For<ChangeListener>(), new Lock());
		var serialized = MemoryPackSerializer.Serialize(set);
		var persistedSet = MemoryPackSerializer.Deserialize<DataSet>(serialized);
		Guard.IsNotNull(persistedSet);
		return persistedSet;
	}

	public static Image CreateImage()
	{
		var set = CreateImageSet();
		return set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
	}
}