using System;
using System.Reactive.Subjects;

namespace SightKeeper.Avalonia;

internal sealed class BehaviorObservable<T> : IObservable<T>
{
	public static implicit operator T(BehaviorObservable<T> observable)
	{
		return observable.Value;
	}

	public static implicit operator BehaviorObservable<T>(BehaviorSubject<T> observable)
	{
		return new BehaviorObservable<T>(observable);
	}

	public T Value => _subject.Value;

	public BehaviorObservable(BehaviorSubject<T> subject)
	{
		_subject = subject;
	}
	
	public IDisposable Subscribe(IObserver<T> observer) => _subject.Subscribe(observer);

	private readonly BehaviorSubject<T> _subject;
}