using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public abstract class ClassifierAnnotator : ObservableAnnotator, IDisposable
{
	public IObservable<Image> AssetsChanged => _assetsChanged.AsObservable();

	public virtual void SetTag(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image, Tag tag)
	{
		var asset = assetsLibrary.GetOrMakeAsset(image);
		asset.Tag = tag;
	}

	public virtual void DeleteAsset(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image)
	{
		assetsLibrary.DeleteAsset(image);
	}

	public void Dispose()
	{
		_assetsChanged.Dispose();
	}

	private readonly Subject<Image> _assetsChanged = new();
}