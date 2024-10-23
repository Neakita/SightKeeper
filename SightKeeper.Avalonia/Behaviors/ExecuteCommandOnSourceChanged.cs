using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class ExecuteCommandOnSourceChanged : Behavior<Image>
{
	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<ExecuteCommandOnSourceChanged, ICommand?>(nameof(Command));

	public static readonly StyledProperty<bool> IgnoreNullValueProperty =
		AvaloniaProperty.Register<ExecuteCommandOnSourceChanged, bool>(nameof(IgnoreNullValue), true);

	public ICommand? Command
	{
		get => GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	public bool IgnoreNullValue
	{
		get => GetValue(IgnoreNullValueProperty);
		set => SetValue(IgnoreNullValueProperty, value);
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PropertyChanged += OnAssociatedObjectPropertyChanged;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.PropertyChanged -= OnAssociatedObjectPropertyChanged;
	}

	private void OnAssociatedObjectPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
	{
		if (e.Property != Image.SourceProperty)
			return;
		var oldValue = e.GetOldValue<IImage?>();
		OnSourceChanged(oldValue);
	}

	private void OnSourceChanged(IImage? oldValue)
	{
		if (IgnoreNullValue && oldValue == null)
			return;
		if (Command != null && Command.CanExecute(oldValue))
			Command.Execute(oldValue);
	}
}