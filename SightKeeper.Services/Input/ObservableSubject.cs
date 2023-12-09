using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SightKeeper.Services.Input;

public sealed class ObservableSubject<T> : SubjectBase<T>
{
	public override bool HasObservers => _subject.HasObservers;
	public override bool IsDisposed => _subject.IsDisposed && _hasObservers.IsDisposed;
	public IObservable<bool> HasObserversObservable => _hasObservers.DistinctUntilChanged();

	public override IDisposable Subscribe(IObserver<T> observer)
	{
		var disposable = _subject.Finally(NotifyHasObservers).Subscribe(observer);
		NotifyHasObservers();
		return disposable;
	}

	public override void OnCompleted()
	{
		_subject.OnCompleted();
	}

	public override void OnError(Exception error)
	{
		_subject.OnError(error);
	}

	public override void OnNext(T value)
	{
		_subject.OnNext(value);
	}
	
	public override void Dispose()
	{
		_subject.Dispose();
		_hasObservers.Dispose();
	}

	private readonly Subject<T> _subject = new();
	private readonly BehaviorSubject<bool> _hasObservers = new(false);

	private void NotifyHasObservers()
	{
		_hasObservers.OnNext(HasObservers);
	}
}