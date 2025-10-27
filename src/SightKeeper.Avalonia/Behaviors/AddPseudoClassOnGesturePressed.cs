using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using HotKeys;
using HotKeys.Avalonia;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class AddPseudoClassOnGesturePressed : Behavior<Visual>, ContinuousHandler
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

	public void Begin()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Classes.Add(ClassName);
	}

	public void End()
	{
		Guard.IsNotNull(AssociatedObject);
		bool isRemoved = AssociatedObject.Classes.Remove(ClassName);
		Guard.IsTrue(isRemoved);
	}

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		var topLevel = TopLevel.GetTopLevel(AssociatedObject);
		Guard.IsNotNull(topLevel);
		var observableGesture = topLevel.ObserveInputStates().ToGesture();
		_bindingsManager = new BindingsManager(observableGesture);
		_binding = _bindingsManager.CreateBinding(this);
		_binding.Gesture = ConvertGesture(Gesture);
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(_binding);
		_binding.Dispose();
		Guard.IsNotNull(_bindingsManager);
		_bindingsManager.Dispose();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == GestureProperty && _binding != null)
			_binding.Gesture = ConvertGesture(change.NewValue);
		if (change.Property == ClassNameProperty && AssociatedObject != null)
		{
			var (oldValue, newValue) = change.GetOldAndNewValue<string>();
			if (AssociatedObject.Classes.Remove(oldValue))
				AssociatedObject.Classes.Add(newValue);
		}
	}

	private BindingsManager? _bindingsManager;
	private Binding? _binding;

	private static Gesture ConvertGesture(object? gesture)
	{
		return gesture switch
		{
			null => HotKeys.Gesture.Empty,
			KeyGesture keyGesture => new Gesture(keyGesture.Key),
			_ => throw new ArgumentOutOfRangeException(nameof(gesture), gesture, null)
		};
	}
}