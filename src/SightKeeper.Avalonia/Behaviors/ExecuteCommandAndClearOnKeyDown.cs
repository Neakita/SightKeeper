using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class ExecuteCommandAndClearOnKeyDown : Behavior<TextBox>
{
	public static readonly StyledProperty<Key> KeyProperty =
		AvaloniaProperty.Register<ExecuteCommandAndClearOnKeyDown, Key>(nameof(Key));

	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<ExecuteCommandAndClearOnKeyDown, ICommand?>(nameof(Command));

	public static readonly StyledProperty<object?> CommandParameterProperty =
		AvaloniaProperty.Register<ExecuteCommandAndClearOnKeyDown, object?>(nameof(CommandParameter));

	public Key Key
	{
		get => GetValue(KeyProperty);
		set => SetValue(KeyProperty, value);
	}

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

	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.KeyDown += OnKeyDown;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.KeyDown -= OnKeyDown;
	}

	private void OnKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.Key != Key || Command == null || !Command.CanExecute(CommandParameter))
			return;
		Guard.IsNotNull(AssociatedObject);
		Command.Execute(CommandParameter);
		AssociatedObject.Clear();
	}
}