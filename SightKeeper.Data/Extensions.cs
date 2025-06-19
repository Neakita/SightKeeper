using FlakeId;
using SightKeeper.Data.Model.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal static class Extensions
{
	public static ImageSet WithDomainRules(this ImageSet set)
	{
		return new DomainImageSet(set);
	}

	public static ImageSet WithTracking(this ImageSet set, ChangeListener listener)
	{
		return new TrackableImageSet(set, listener);
	}

	public static ImageSet WithLocking(this ImageSet set, Lock editingLock)
	{
		return new LockingImageSet(set, editingLock);
	}

	public static ImageSet WithStreamableImages(this ImageSet set, FileSystemDataAccess dataAccess)
	{
		return new StreamableImagesSet(set, dataAccess);
	}

	public static ImageSet WithObservableImages(this ImageSet set)
	{
		return new ObservableImagesImageSet(set);
	}

	public static Id GetId(this Image image)
	{
		if (image is InMemoryImage inMemoryImage) 
			return inMemoryImage.Id;
		if (image is Decorator<InMemoryImage> decoratorOnInMemory)
			return decoratorOnInMemory.Inner.Id;
		if (image is Decorator<Image> decorator)
			return GetId(decorator.Inner);
		throw new ArgumentException($"Provided image could not be unwrapped to {typeof(InMemoryImage)}");
	}
}