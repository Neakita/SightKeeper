using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal sealed class KeyPointVisibilityBehavior : Behavior<StyledElement>
{
	public static readonly StyledProperty<object?> DataContextProperty =
		AvaloniaProperty.Register<KeyPointVisibilityBehavior, object?>(nameof(DataContext));

	public static readonly StyledProperty<InputElement?> ThumbProperty =
		AvaloniaProperty.Register<KeyPointVisibilityBehavior, InputElement?>(nameof(Thumb));

	public object? DataContext
	{
		get => GetValue(DataContextProperty);
		set => SetValue(DataContextProperty, value);
	}

	[ResolveByName]
	public InputElement? Thumb
	{
		get => GetValue(ThumbProperty);
		set => SetValue(ThumbProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == DataContextProperty)
			HandleDataContextChange(change);
		if (change.Property == ThumbProperty)
			HandleThumbChange(change);
	}

	private void HandleDataContextChange(AvaloniaPropertyChangedEventArgs change)
	{
		var newValue = change.NewValue;
		bool isInvisible = !(newValue as KeyPoint3DViewModel)?.IsVisible ?? false;
		AssociatedObject?.Classes.Set("invisible", isInvisible);
	}

	private void HandleThumbChange(AvaloniaPropertyChangedEventArgs change)
	{
		var (oldValue, newValue) = change.GetOldAndNewValue<InputElement?>();
		if (oldValue != null)
			oldValue.PointerPressed -= OnThumbPointerPressed;
		if (newValue != null)
			newValue.PointerPressed += OnThumbPointerPressed;
	}

	private void OnThumbPointerPressed(object? sender, PointerPressedEventArgs e)
	{
		if (DataContext is not KeyPoint3DViewModel keyPoint || !e.GetCurrentPoint(null).Properties.IsRightButtonPressed)
			return;
		keyPoint.IsVisible = !keyPoint.IsVisible;
		AssociatedObject?.Classes.Set("invisible", !keyPoint.IsVisible);
		e.Handled = true;
	}
}