using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Xaml.Interactivity;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class AddStylesBehavior : Behavior
{
	[field: AllowNull, MaybeNull] public Styles Styles => field ??= new Styles();

	public static readonly StyledProperty<StyledElement?> TargetProperty =
		AvaloniaProperty.Register<AddStylesBehavior, StyledElement?>(nameof(Target));

	public StyledElement? Target
	{
		get => GetValue(TargetProperty);
		set => SetValue(TargetProperty, value);
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property != TargetProperty)
			return;
		var (oldValue, newValue) = change.GetOldAndNewValue<StyledElement?>();
		oldValue?.Styles.RemoveAll(Styles);
		newValue?.Styles.AddRange(Styles);
	}
}