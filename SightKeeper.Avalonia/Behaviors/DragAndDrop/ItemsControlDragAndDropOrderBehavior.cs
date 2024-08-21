using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.VisualTree;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors.DragAndDrop;

internal sealed class ItemsControlDragAndDropOrderBehavior : DragAndDropOrderBehavior<ItemsControl>
{
	protected override Control? FindAncestorItemContainer(Visual visual)
	{
		return visual.FindAncestorOfType<ContentPresenter>();
	}

	protected override Control CreateItemGhost(Control itemContainer)
	{
		Guard.IsNotNull(AssociatedObject);
		return new ContentPresenter
		{
			Content = ((ContentPresenter)itemContainer).Content,
			ContentTemplate = AssociatedObject.ItemTemplate
		};
	}
}