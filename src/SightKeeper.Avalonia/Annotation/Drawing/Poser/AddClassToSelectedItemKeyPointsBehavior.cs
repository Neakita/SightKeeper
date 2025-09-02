using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal sealed class AddClassToSelectedItemKeyPointsBehavior : Behavior<ListBox>
{
	public static readonly StyledProperty<string> ClassNameProperty =
		AvaloniaProperty.Register<AddClassToSelectedItemKeyPointsBehavior, string>(nameof(ClassName));

	public string ClassName
	{
		get => GetValue(ClassNameProperty);
		set => SetValue(ClassNameProperty, value);
	}

	protected override void OnAttached()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Selection.SelectionChanged += OnSelectionChanged;
	}

	protected override void OnDetaching()
	{
		Guard.IsNotNull(AssociatedObject);
		AssociatedObject.Selection.SelectionChanged -= OnSelectionChanged;
	}

	private void OnSelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs args)
	{
		Guard.IsNotNull(AssociatedObject);
		foreach (var container in args.SelectedItems.OfType<PoserItemViewModel>().SelectMany(GetAssociatedKeyPointViews))
			container.Classes.Add(ClassName);
		foreach (var container in args.DeselectedItems.OfType<PoserItemViewModel>().SelectMany(GetAssociatedKeyPointViews))
			container.Classes.Remove(ClassName);
	}

	private IEnumerable<Control> GetAssociatedKeyPointViews(PoserItemViewModel item)
	{
		Guard.IsNotNull(AssociatedObject);
		return AssociatedObject.Items
			.OfType<KeyPointViewModel>()
			.Where(keyPoint => keyPoint.Item == item)
			.Select(AssociatedObject.ContainerFromItem)
			.Select(GuardNotNull);
	}

	private static Control GuardNotNull(Control? control)
	{
		Guard.IsNotNull(control);
		return control;
	}
}