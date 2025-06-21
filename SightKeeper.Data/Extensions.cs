using FlakeId;
using SightKeeper.Data.Model.DataSets;
using SightKeeper.Data.Model.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Classifier;
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

	public static Image WithStreaming(this InMemoryImage image, FileSystemDataAccess dataAccess)
	{
		return new StreamableDataImage(image, dataAccess);
	}

	public static Image WithObservableAssets(this Image image)
	{
		return new ObservableAssetsImage(image);
	}

	public static ImageSet WithObservableImages(this ImageSet set)
	{
		return new ObservableImagesImageSet(set);
	}

	public static Id GetId(this Image image)
	{
		var inMemoryImage = UnWrapDecorator<InMemoryImage>(image);
		return inMemoryImage.Id;
	}

	public static TTarget UnWrapDecorator<TTarget>(this object source)
	{
		if (source is TTarget target)
			return target;
		if (source is Decorator<object> decorator)
			return UnWrapDecorator<TTarget>(decorator.Inner);
		throw new ArgumentException($"Provided object of type {source.GetType()} could not be unwrapped to {typeof(TTarget)}");
	}

	public static ClassifierDataSet WithTracking(this ClassifierDataSet set, ChangeListener listener)
	{
		return new TrackableClassifierDataSet(set, listener);
	}

	public static ClassifierDataSet WithLocking(this ClassifierDataSet set, Lock editingLock)
	{
		return new LockingClassifierDataSet(set, editingLock);
	}

	public static ClassifierDataSet WithDomainRules(this ClassifierDataSet set)
	{
		return new DomainClassifierDataSet(set);
	}
}