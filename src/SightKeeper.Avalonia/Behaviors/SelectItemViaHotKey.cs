using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class SelectItemViaHotKey : Behavior<SelectingItemsControl>
{
	public static readonly StyledProperty<KeyModifiers> ModifiersProperty =
		AvaloniaProperty.Register<SelectItemViaHotKey, KeyModifiers>(nameof(Modifiers));

	public KeyModifiers Modifiers
	{
		get => GetValue(ModifiersProperty);
		set => SetValue(ModifiersProperty, value);
	}

	protected override void OnAttachedToVisualTree()
	{
		_topLevel = TopLevel.GetTopLevel(AssociatedObject);
		Guard.IsNotNull(_topLevel);
		_topLevel.KeyDown += OnKeyDown;
	}

	protected override void OnDetachedFromVisualTree()
	{
		Guard.IsNotNull(_topLevel);
		_topLevel.KeyDown -= OnKeyDown;
	}

	private TopLevel? _topLevel;

	private void OnKeyDown(object? sender, KeyEventArgs e)
	{
		if (e.KeyModifiers != Modifiers)
			return;
		var key = e.Key;
		if (key < Key.D1)
			return;
		if (key > Key.D9)
			return;
		var itemIndex = key - Key.D1;
		if (itemIndex >= AssociatedObject.Items.Count)
			return;
		AssociatedObject.SelectedIndex = itemIndex;
	}
}