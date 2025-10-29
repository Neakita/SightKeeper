using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Data.Services;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class ImageSetDeserializer(
	Factory<ImageSet> setFactory,
	Deserializer<IReadOnlyCollection<ManagedImage>> imagesDeserializer) :
	Deserializer<ImageSet>
{
	public ImageSet Deserialize(ref MemoryPackReader reader)
	{
		var set = setFactory.Create();
		var innerSet = set.GetInnermost<ImageSet>();
		ReadGeneralInfo(ref reader, innerSet);
		ReadImages(ref reader, innerSet);
		return set;
	}

	private static void ReadGeneralInfo(ref MemoryPackReader reader, ImageSet set)
	{
		var name = reader.ReadString();
		Guard.IsNotNull(name);
		var description = reader.ReadString();
		Guard.IsNotNull(description);
		set.Name = name;
		set.Description = description;
	}

	private void ReadImages(ref MemoryPackReader reader, ImageSet set)
	{
		var settableInitialImages = set.GetFirst<SettableInitialItems<ManagedImage>>();
		var images = imagesDeserializer.Deserialize(ref reader);
		settableInitialImages.EnsureCapacity(images.Count);
		foreach (var image in images)
			settableInitialImages.WrapAndInsert(image);
	}
}