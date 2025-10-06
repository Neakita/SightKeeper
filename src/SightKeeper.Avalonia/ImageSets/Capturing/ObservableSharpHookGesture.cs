using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using HotKeys;
using SharpHook;
using SharpHook.Reactive;
using Sightful.Avalonia.Controls.GestureBox;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

public sealed class ObservableSharpHookGesture : IObservable<GestureEdit>, IDisposable
{
	public ObservableSharpHookGesture(IReactiveGlobalHook hook)
	{
		hook.KeyPressed
			.Subscribe(OnKeyPressed)
			.DisposeWith(_constructorDisposable);
		hook.MousePressed
			.Subscribe(OnMousePressed)
			.DisposeWith(_constructorDisposable);
		hook.KeyReleased
			.Subscribe(OnKeyReleased)
			.DisposeWith(_constructorDisposable);
		hook.MouseReleased
			.Subscribe(OnMouseReleased)
			.DisposeWith(_constructorDisposable);
	}

	public IDisposable Subscribe(IObserver<GestureEdit> observer)
	{
		_observers.Add(observer);
		return Disposable.Create(observer, Unsubscribe);
	}

	private void OnKeyPressed(KeyboardHookEventArgs args)
	{
		var pressedKeysBuilder = _pressedGesture.Keys.ToBuilder();
		var key = args.Data.KeyCode;
		if (!pressedKeysBuilder.Add(key))
			return;
		_pressedGesture = new Gesture(pressedKeysBuilder.ToImmutable());
		var edit = new GestureEdit(_pressedGesture, false);
		NotifyObservers(edit);
	}

	private void OnMousePressed(MouseHookEventArgs args)
	{
		var pressedKeysBuilder = _pressedGesture.Keys.ToBuilder();
		var button = args.Data.Button;
		if (!pressedKeysBuilder.Add(button))
			return;
		_pressedGesture = new Gesture(pressedKeysBuilder.ToImmutable());
		var edit = new GestureEdit(_pressedGesture, false);
		NotifyObservers(edit);
	}

	private void OnKeyReleased(KeyboardHookEventArgs args)
	{
		var edit = new GestureEdit(_pressedGesture, true);
		NotifyObservers(edit);
		var key = args.Data.KeyCode;
		_pressedGesture = _pressedGesture.Without(key);
	}

	private void OnMouseReleased(MouseHookEventArgs args)
	{
		var edit = new GestureEdit(_pressedGesture, true);
		NotifyObservers(edit);
		var button = args.Data.Button;
		_pressedGesture = _pressedGesture.Without(button);
	}

	public void Dispose()
	{
		_constructorDisposable.Dispose();
	}

	private void Unsubscribe(IObserver<GestureEdit> observer)
	{
		_observers.Remove(observer);
	}

	private readonly CompositeDisposable _constructorDisposable = new();
	private readonly List<IObserver<GestureEdit>> _observers = new();
	private Gesture _pressedGesture = Gesture.Empty;

	private void NotifyObservers(GestureEdit gesture)
	{
		foreach (var observer in _observers)
			observer.OnNext(gesture);
	}
}