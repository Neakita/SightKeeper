using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class ClearTextBoxWhenClickingBehavior : Behavior<Button>
{
	public static readonly StyledProperty<TextBox?> TextBoxProperty =
		AvaloniaProperty.Register<ClearTextBoxWhenClickingBehavior, TextBox?>(nameof(TextBox));

	[ResolveByName]
	public TextBox? TextBox
	{
		get => GetValue(TextBoxProperty);
		set => SetValue(TextBoxProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Click += OnClick;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Click -= OnClick;
	}

	private void OnClick(object? sender, RoutedEventArgs e)
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Command?.Execute(AssociatedObject.CommandParameter);
		TextBox?.Clear();
	}
}