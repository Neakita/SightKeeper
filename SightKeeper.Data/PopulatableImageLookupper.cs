using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data;

public class PopulatableImageLookupper : ImageLookupperPopulator, ImageLookupper
{
	public void AddImages(IEnumerable<StorableImage> images)
	{
		foreach (var image in images)
			AddImage(image);
	}

	public void AddImage(StorableImage image)
	{
		var id = image.Id;
		_images.Add(id, image);
	}

	public void RemoveImage(StorableImage image)
	{
		bool isRemoved = _images.Remove(image.Id);
		Guard.IsTrue(isRemoved);
	}

	public StorableImage GetImage(Id id)
	{
		return _images[id];
	}

	public void ClearImages()
	{
		_images.Clear();
	}

	private readonly Dictionary<Id, StorableImage> _images = new();
}