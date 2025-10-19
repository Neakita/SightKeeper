using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

internal class PopulatableImageLookupper : ImageLookupperPopulator, ImageLookupper
{
	public void AddImages(IEnumerable<ManagedImage> images)
	{
		foreach (var image in images)
			AddImage(image);
	}

	public void AddImage(ManagedImage image)
	{
		var id = image.Id;
		_images.Add(id, image);
	}

	public void RemoveImage(ManagedImage image)
	{
		bool isRemoved = _images.Remove(image.Id);
		Guard.IsTrue(isRemoved);
	}

	public ManagedImage GetImage(Id id)
	{
		return _images[id];
	}

	public bool ContainsImage(Id id)
	{
		return _images.ContainsKey(id);
	}

	public void ClearImages()
	{
		_images.Clear();
	}

	private readonly Dictionary<Id, ManagedImage> _images = new();
}