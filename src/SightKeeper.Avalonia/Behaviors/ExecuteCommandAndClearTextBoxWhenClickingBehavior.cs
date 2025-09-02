using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class ExecuteCommandAndClearTextBoxWhenClickingBehavior : Behavior<Button>
{
	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<ExecuteCommandAndClearTextBoxWhenClickingBehavior, ICommand?>(nameof(Command));

	public static readonly StyledProperty<object?> CommandParameterProperty =
		AvaloniaProperty.Register<ExecuteCommandAndClearTextBoxWhenClickingBehavior, object?>(nameof(CommandParameter));

	public static readonly StyledProperty<TextBox?> TextBoxProperty =
		AvaloniaProperty.Register<ExecuteCommandAndClearTextBoxWhenClickingBehavior, TextBox?>(nameof(TextBox));

	public ICommand? Command
	{
		get => GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public object? CommandParameter
	{
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}

	[ResolveByName]
	public TextBox? TextBox
	{
		get => GetValue(TextBoxProperty);
		set => SetValue(TextBoxProperty, value);
	}

	protected override void OnAttached()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Click += OnClick;
	}

	protected override void OnDetaching()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Click -= OnClick;
	}

	private void OnClick(object? sender, RoutedEventArgs e)
	{
		Command?.Execute(CommandParameter);
		TextBox?.Clear();
	}
}