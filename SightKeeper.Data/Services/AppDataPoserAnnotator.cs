using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Services;

public sealed class AppDataPoserAnnotator : PoserAnnotator, ObservablePoserAnnotator, IDisposable
{
	public IObservable<(PoserItem item, KeyPoint keyPoint)> KeyPointCreated => _keyPointCreated.AsObservable();
	public IObservable<(PoserItem item, KeyPoint keyPoint)> KeyPointDeleted => _keyPointDeleted.AsObservable();

	public AppDataPoserAnnotator(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public void CreateKeyPoint(PoserItem item, Tag tag, Vector2<double> position)
	{
		KeyPoint keyPoint;
		lock (_appDataLock)
			keyPoint = item.CreateKeyPoint(tag, position);
		_appDataAccess.SetDataChanged();
		_keyPointCreated.OnNext((item, keyPoint));
	}

	public void SetKeyPointPosition(KeyPoint keyPoint, Vector2<double> position)
	{
		lock (_appDataLock)
			keyPoint.Position = position;
		_appDataAccess.SetDataChanged();
	}

	public void SetKeyPointVisibility(KeyPoint3D keyPoint, bool isVisible)
	{
		lock (_appDataLock)
			keyPoint.IsVisible = isVisible;
		_appDataAccess.SetDataChanged();
	}

	public void DeleteKeyPoint(PoserItem item, Tag tag)
	{
		var keyPoint = item.KeyPoints.Single(keyPoint => keyPoint.Tag == tag);
		lock(_appDataLock)
			item.DeleteKeyPoint(keyPoint);
		_appDataAccess.SetDataChanged();
		_keyPointDeleted.OnNext((item, keyPoint));
	}

	public void Dispose()
	{
		_keyPointCreated.Dispose();
		_keyPointDeleted.Dispose();
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
	private readonly Subject<(PoserItem item, KeyPoint keyPoint)> _keyPointCreated = new();
	private readonly Subject<(PoserItem item, KeyPoint keyPoint)> _keyPointDeleted = new();
}