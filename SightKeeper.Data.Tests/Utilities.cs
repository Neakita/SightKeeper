using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Tests;

internal static class Utilities
{
	private static readonly Lazy<PersistenceServices> Services = new(PersistenceBootstrapper.Setup);

	public static ImageSet CreateImageSet()
	{
		var factory = Services.Value.ImageSetFactory;
		return factory.CreateImageSet();
	}

	public static ClassifierDataSet CreateClassifierDataSet()
	{
		var factory = Services.Value.ClassifierDataSetFactory;
		return factory.CreateDataSet();
	}

	public static T Persist<T>(this T value)
	{
		PersistenceBootstrapper.Setup();
		var serialized = MemoryPackSerializer.Serialize(value);
		var persistedValue = MemoryPackSerializer.Deserialize<T>(serialized);
		Guard.IsNotNull(persistedValue);
		return persistedValue;
	}

	public static Image CreateImage()
	{
		var set = CreateImageSet();
		return set.CreateImage(DateTimeOffset.UtcNow, new Vector2<ushort>(320, 320));
	}
}