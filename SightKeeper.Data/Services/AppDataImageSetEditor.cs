using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.ImageSets;
using SightKeeper.Application.ImageSets.Editing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataImageSetEditor : ImageSetEditor
{
	public IObservable<ImageSet> Edited => _edited.AsObservable();

	public AppDataImageSetEditor(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public void EditLibrary(ImageSet set, ImageSetData data)
	{
		lock (_appDataLock)
		{
			set.Name = data.Name;
			set.Description = data.Description;
		}
		_appDataAccess.SetDataChanged();
		_edited.OnNext(set);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
	private readonly Subject<ImageSet> _edited = new();
}