using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataProfilesDataAccess :
	ReadDataAccess<Profile>,
	ObservableDataAccess<Profile>,
	WriteDataAccess<Profile>,
	IDisposable
{
	public IReadOnlyCollection<Profile> Items => throw new NotImplementedException();

	public IObservable<Profile> Added => throw new NotImplementedException();

	public IObservable<Profile> Removed => throw new NotImplementedException();

	public AppDataProfilesDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
	}

	public void Add(Profile profile)
	{
		lock (_editingLock)
			_appDataAccess.Data.AddProfile(profile);
		_appDataAccess.SetDataChanged();
		_added.OnNext(profile);
	}

	public void Remove(Profile profile)
	{
		lock (_editingLock)
			_appDataAccess.Data.RemoveProfile(profile);
		_appDataAccess.SetDataChanged();
		_removed.OnNext(profile);
	}

	public void Dispose()
	{
		_added.Dispose();
		_removed.Dispose();
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _editingLock;
	private readonly Subject<Profile> _added = new();
	private readonly Subject<Profile> _removed = new();
}