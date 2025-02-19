using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Avalonia.Annotation.Drawing.Poser;

internal sealed class AddClassToHoveredItemKeyPointsBehavior : Behavior<ListBox>
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
		InputElement.PointerEnteredEvent
			.AddClassHandler<ListBoxItem>(OnPointerEntered, RoutingStrategies.Tunnel)
			.DisposeWith(_disposable);
		InputElement.PointerExitedEvent
			.AddClassHandler<ListBoxItem>(OnPointerExited, RoutingStrategies.Tunnel)
			.DisposeWith(_disposable);
	}

	protected override void OnDetaching()
	{
		Guard.IsNotNull(AssociatedObject);
		_disposable.Dispose();
	}

	private readonly CompositeDisposable _disposable = new();

	private void OnPointerEntered(ListBoxItem sender, PointerEventArgs args)
	{
		Guard.IsNotNull(AssociatedObject);
		var item = AssociatedObject.ItemFromContainer(sender);
		if (item is not PoserItemViewModel poserItem)
			return;
		var containers = GetAssociatedKeyPointContainers(poserItem);
		foreach (var container in containers)
			container.Classes.Add(ClassName);
	}

	private void OnPointerExited(ListBoxItem sender, PointerEventArgs args)
	{
		Guard.IsNotNull(AssociatedObject);
		var item = AssociatedObject.ItemFromContainer(sender);
		if (item is not PoserItemViewModel poserItem)
			return;
		var containers = GetAssociatedKeyPointContainers(poserItem);
		foreach (var container in containers)
		{
			bool isRemoved = container.Classes.Remove(ClassName);
			Guard.IsTrue(isRemoved);
		}
	}

	private IEnumerable<Control> GetAssociatedKeyPointContainers(PoserItemViewModel item)
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