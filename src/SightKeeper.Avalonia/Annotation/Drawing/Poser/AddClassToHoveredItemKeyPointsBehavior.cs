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
	public static readonly StyledProperty<string> KeyPointClassNameProperty =
		AvaloniaProperty.Register<AddClassToSelectedItemKeyPointsBehavior, string>(nameof(KeyPointClassName));

	public static readonly StyledProperty<string> ItemClassNameProperty =
		AvaloniaProperty.Register<AddClassToHoveredItemKeyPointsBehavior, string>(nameof(ItemClassName));

	public string KeyPointClassName
	{
		get => GetValue(KeyPointClassNameProperty);
		set => SetValue(KeyPointClassNameProperty, value);
	}

	public string ItemClassName
	{
		get => GetValue(ItemClassNameProperty);
		set => SetValue(ItemClassNameProperty, value);
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
		if (item is PoserItemViewModel poserItem)
		{
			var containers = GetAssociatedKeyPointContainers(poserItem);
			foreach (var container in containers)
				container.Classes.Add(KeyPointClassName);
		}
		if (item is KeyPointViewModel keyPoint)
		{
			var container = GetAssociatedItemContainer(keyPoint);
			container.Classes.Add(ItemClassName);
		}
	}

	private void OnPointerExited(ListBoxItem sender, PointerEventArgs args)
	{
		Guard.IsNotNull(AssociatedObject);
		var item = AssociatedObject.ItemFromContainer(sender);
		if (item is PoserItemViewModel poserItem)
		{
			var containers = GetAssociatedKeyPointContainers(poserItem);
			foreach (var container in containers)
			{
				bool isRemoved = container.Classes.Remove(KeyPointClassName);
				Guard.IsTrue(isRemoved);
			}
		}
		if (item is KeyPointViewModel keyPoint)
		{
			var container = GetAssociatedItemContainer(keyPoint);
			var isRemoved = container.Classes.Remove(ItemClassName);
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

	private Control GetAssociatedItemContainer(KeyPointViewModel keyPoint)
	{
		Guard.IsNotNull(AssociatedObject);
		var item = AssociatedObject.Items
			.OfType<PoserItemViewModel>()
			.Single(item => item.KeyPoints.Contains(keyPoint));
		var itemContainer = AssociatedObject.ContainerFromItem(item);
		Guard.IsNotNull(itemContainer);
		return itemContainer;
	}

	private static Control GuardNotNull(Control? control)
	{
		Guard.IsNotNull(control);
		return control;
	}
}