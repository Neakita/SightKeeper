using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using HotKeys;
using HotKeys.Avalonia;
using HotKeys.Bindings;
using HotKeys.Gestures;
using Binding = HotKeys.Bindings.Binding;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class AddPseudoClassOnGesturePressed : Behavior<Visual>
{
	public static readonly StyledProperty<object?> GestureProperty =
		AvaloniaProperty.Register<AddPseudoClassOnGesturePressed, object?>(nameof(Gesture));

	public static readonly StyledProperty<string> ClassNameProperty =
		AvaloniaProperty.Register<AddPseudoClassOnGesturePressed, string>(nameof(ClassName));

	public object? Gesture
	{
		get => GetValue(GestureProperty);
		set => SetValue(GestureProperty, value);
	}

	public string ClassName
	{
		get => GetValue(ClassNameProperty);
		set => SetValue(ClassNameProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		var topLevel = TopLevel.GetTopLevel(AssociatedObject);
		Guard.IsNotNull(topLevel);
		var observableGesture = topLevel.ObserveInputStates().Filter().ToGesture();
		_bindingsManager = new BindingsManager(observableGesture);
		_binding = _bindingsManager.CreateBinding(context =>
		{
			Dispatcher.UIThread.Invoke(() => AssociatedObject.Classes.Add(ClassName));
			context.WaitForElimination();
			Dispatcher.UIThread.Invoke(() => AssociatedObject.Classes.Remove(ClassName));
		}, InputTypes.Hold);
		UpdateBinding();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == GestureProperty)
			UpdateBinding();
		if (change.Property == ClassNameProperty && AssociatedObject != null)
		{
			var (oldValue, newValue) = change.GetOldAndNewValue<string>();
			if (AssociatedObject.Classes.Remove(oldValue))
				AssociatedObject.Classes.Add(newValue);
		}
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(_binding);
		_binding.Dispose();
		Guard.IsNotNull(_bindingsManager);
		_bindingsManager.Dispose();
	}

	private BindingsManager? _bindingsManager;
	private Binding? _binding;

	private void UpdateBinding()
	{
		if (_bindingsManager == null || _binding == null)
			return;
		Gesture gesture = Gesture switch
		{
			KeyGesture keyGesture => new Gesture(keyGesture.Key),
			null => HotKeys.Gestures.Gesture.Empty,
			_ => throw new ArgumentOutOfRangeException(nameof(Gesture), Gesture, null)
		};
		_bindingsManager.SetGesture(_binding, gesture);
	}
}