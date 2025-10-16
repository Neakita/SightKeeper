using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class ImageSetDeserializer(
	ImageSetFactory<ImageSet> setFactory,
	Deserializer<IReadOnlyCollection<ManagedImage>> imagesDeserializer,
	ImageLookupperPopulator lookupperPopulator,
	ImageSetWrapper wrapper) :
	Deserializer<ImageSet>
{
	public ImageSet Deserialize(ref MemoryPackReader reader)
	{
		var set = setFactory.CreateImageSet();
		ReadGeneralInfo(ref reader, set);
		ReadImages(ref reader, set);
		lookupperPopulator.AddImages(set.Images);
		return wrapper.Wrap(set);
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