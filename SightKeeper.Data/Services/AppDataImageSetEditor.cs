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

	public AppDataImageSetEditor(ChangeListener changeListener, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_changeListener = changeListener;
		_appDataLock = appDataLock;
	}

	public void EditLibrary(ImageSet set, ImageSetData data)
	{
		lock (_appDataLock)
		{
			set.Name = data.Name;
			set.Description = data.Description;
		}
		_changeListener.SetDataChanged();
		_edited.OnNext(set);
	}

	private readonly ChangeListener _changeListener;
	private readonly Lock _appDataLock;
	private readonly Subject<ImageSet> _edited = new();
}