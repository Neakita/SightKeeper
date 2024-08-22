using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors.DragAndDrop;

internal sealed class ListBoxDragAndDropOrderBehavior : DragAndDropOrderBehavior
{
	protected override Control? FindAncestorItemContainer(Visual visual)
	{
		return visual.FindAncestorOfType<ListBoxItem>();
	}

	protected override Control CreateItemGhost(Control itemContainer)
	{
		Guard.IsNotNull(AssociatedObject);
		ListBoxItem itemGhost = new()
		{
			ContentTemplate = AssociatedObject.ItemTemplate,
			Content = ((ListBoxItem)itemContainer).Content,
			IsHitTestVisible = false,
			Opacity = 0.4
		};
		return itemGhost;
	}
}