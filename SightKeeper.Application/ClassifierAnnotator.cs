using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application;

public class ClassifierAnnotator : ObservableAnnotator, IDisposable
{
	public IObservable<Screenshot> AssetsChanged => _assetsChanged.AsObservable();

	public virtual void SetTag(AssetsLibrary<ClassifierAsset> assetsLibrary, Screenshot screenshot, Tag tag)
	{
		var asset = assetsLibrary.GetOrMakeAsset(screenshot);
		asset.Tag = tag;
	}

	public virtual void DeleteAsset(AssetsLibrary<ClassifierAsset> assetsLibrary, Screenshot screenshot)
	{
		assetsLibrary.DeleteAsset(screenshot);
	}

	public void Dispose()
	{
		_assetsChanged.Dispose();
	}

	private readonly Subject<Screenshot> _assetsChanged = new();
}