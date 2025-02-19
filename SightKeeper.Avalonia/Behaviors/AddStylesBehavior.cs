using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

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

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(Target);
		Target.Styles.AddRange(Styles);
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(Target);
		Target.Styles.RemoveAll(Styles);
	}
}